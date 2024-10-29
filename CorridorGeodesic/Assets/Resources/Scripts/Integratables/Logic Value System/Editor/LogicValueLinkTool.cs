using Neverway.Framework.LogicValueSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.ShortcutManagement;
using UnityEngine;

//[EditorTool("Logic Value Link Tool")]
public class LogicValueLinkTool : EditorTool
{
    #region EditorTool Setup

    [InitializeOnLoadMethod]
    public static void Initialize()
    {
        LogicValueLinkToolSettings.Instance.OnSettingsValidate += OnSettingsChanged;
        OnSettingsChanged();
    }
    public static void OnSettingsChanged()
    {
        if (LogicValueLinkToolSettings.Instance.showSceneGUI)
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }
        else
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }
    }
    private static void OnSceneGUI(SceneView sceneview)
    {
        Rect area = sceneview.cameraViewport;
        Handles.BeginGUI();
        Rect toolButtonArea = area;
        toolButtonArea.size = new Vector2(40, 22);
        toolButtonArea.center = new Vector2(area.width / 2, 21);

        //Offset from probuilderbar
        toolButtonArea.x += 85;

        GUIStyle style = new GUIStyle(EditorStyles.miniButton);
        style.fixedHeight = toolButtonArea.height;

        if (IsCurrentTool != EditorGUI.Toggle(toolButtonArea, IsCurrentTool, style))
            SetAsCurrentTool();

        if (!IsCurrentTool)
        {
            try
            {
                BeforeGUI();
                toolButtonArea.y += toolButtonArea.height;
                EditorGUI.LabelField(toolButtonArea, $"links: {links.Length}");

                foreach (ValueLink link in links)
                {
                    link.DrawJustForDisplay();
                }
                Handle lastHandle = null;
                foreach (Handle handle in Handle.SortByDistanceToCamera(handles.ToArray()))
                {
                    lastHandle = handle;
                    handle.DrawJustForDisplay();
                }
                SceneView.RepaintAll();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                ClearALL();
            }
        }
        Handles.EndGUI();
    }
    public static bool IsCurrentTool => ToolManager.activeToolType == typeof(LogicValueLinkTool);

    public static Tool lastToolType;
    [Shortcut("Use Logic Value Link Tool", typeof(SceneView), KeyCode.L, ShortcutModifiers.Alt)]
    public static void SetAsCurrentTool()
    {
        if (IsCurrentTool && lastToolType != Tool.None)
        {
            Tools.current = lastToolType;
            ClearLastToolType();
            return;
        }

        lastToolType = Tools.current;
        ToolManager.SetActiveTool<LogicValueLinkTool>();
        RESET();
        ToolManager.activeToolChanged += ClearLastToolType;
    }
    public static void ClearLastToolType()
    {
        lastToolType = Tool.None;
        ToolManager.activeToolChanged -= ClearLastToolType;
    }
    public void OnEnable()
    {
        Undo.undoRedoPerformed += RESET;
        EditorApplication.hierarchyChanged += RESET;
        ClearALL();
    }
    public void OnDisable()
    {
        Undo.undoRedoPerformed -= RESET;
        EditorApplication.hierarchyChanged -= RESET;
        ClearALL();
        ClearLastToolType();
    }

    // Define the icon displayed in the toolbar
    public static GUIContent m_Icon;
    public override GUIContent toolbarIcon => ToolbarIconContent;
    public static GUIContent ToolbarIconContent
    {
        get 
        {
            if (m_Icon == null)
                m_Icon =  new GUIContent()
                {
                    image = EditorIcons.SettingsIcon,
                    //text = "Logic Value Link Tool",
                    tooltip = "Logic Value Link Tool (Alt + L)"
                };
            return m_Icon;
        }
    }
    #endregion

    public enum LogicValueLinkState { EditorToolClosed, LookingForOutput, SelectingOutputFromWindow, LookingForInput, SelectingInputFromWindow, LinkCompleted }
    public static LogicValueLinkState currentLogicValueLinkState = LogicValueLinkState.EditorToolClosed;

    private static LogicValueField grabbedOutput = null;
    private static LogicValueField targetInput = null;
    private static SelectorWindowNonGeneric currentWindow = null;
    public static bool NotCurrentlyDoingAnything => LookingForOutput && currentWindow == null;
    public static bool LookingForOutput => grabbedOutput == null && targetInput == null && IsCurrentTool;
    public static bool LookingForInput => grabbedOutput != null && targetInput == null;
    public static bool LinkHasFinished => grabbedOutput != null && targetInput != null;

    private static List<Handle> handles;
    private static Transform sceneViewCameraTransform;
    private static LogicValueField[] inputs;
    private static LogicValueField[] outputs;
    private static ValueLink[] links;
    private static Dictionary<Component, LogicValueField[]> logicValues;
    private static Component[] componentsWithLogicValues;
    private static Dictionary<Type, FieldInfo[]> logicValueFieldInfos;
    private static Type[] typesWithLogicValues;
    private static Component[] allComponents;

    public static GUIStyle labelStyle;

    public class LogicValueField
    {
        public FieldInfo fieldInfo;
        public LogicValue value;
        public string valueName;
        public Transform handleTarget;
        public Component onComponent;

        public bool IsInput => value is LogicInput;
        public LogicInput Input => value as LogicInput;
        public bool IsOutput => value is LogicOutput;
        public LogicOutput Output => value as LogicOutput;

        public Type LogicValueType => value.GetLogicValueType();

        public LogicValueField(FieldInfo field, Component onComponent)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            if (onComponent == null)
                throw new ArgumentNullException(nameof(onComponent));

            this.onComponent = onComponent;
            this.fieldInfo = field;
            this.value = (LogicValue)field.GetValue(onComponent);

            this.handleTarget = value.EditorHandles_GetHandle();
            if (handleTarget == null) 
                handleTarget = onComponent.transform;

            this.valueName = value.EditorHandles_GetCustomName();
            this.valueName = string.IsNullOrEmpty(valueName) ? field.HumanName() : valueName;
        }
    }
    public class Handle
    {
        public LogicValueLinkState currentStyleState;
        public HandleStyle currentStyle;

        public SelectorWindowNonGeneric window;
        public Transform handleTarget;
        public List<LogicValueField> inputs;
        public List<LogicValueField> outputs;
        
        public int OutputsAssigned => outputs.Count;
        public int InputsAssigned => inputs.Count;
        public int InputsFiltered => GetFilteredInputs().Length;
        public int ValuesAssigned => OutputsAssigned + InputsAssigned;
        public int ValuesFiltered => OutputsAssigned + InputsFiltered;

        public LogicValueField SingleValueFieldAssigned => 
            (InputsAssigned > 0 ? inputs[0] : 
            (OutputsAssigned > 0 ? outputs[0] : null));
        public LogicValueField SingleValueFieldFiltered =>
            (InputsFiltered > 0 ? GetFilteredInputs()[0] :
            (OutputsAssigned > 0 ? outputs[0] : null));

        public bool HasMultipleValuesAssigned => (inputs.Count + outputs.Count) > 1;

        private LogicValueField[] cachedFilteredInputs;
        private Type cachedFilteredInputsType;

        private const float HANDLESIZE_POI = 0.3f;
        private const float HANDLESIZE_UNIMPORTANT = 0.15f;

        public Handle(Transform handleTarget)
        {
            if (handleTarget == null)
                throw new ArgumentNullException(nameof(handleTarget));

            this.cachedFilteredInputsType = typeof(object);
            this.cachedFilteredInputs = new LogicValueField[0];
            this.window = null;
            this.handleTarget = handleTarget;
            this.inputs = new List<LogicValueField>();
            this.outputs = new List<LogicValueField>();

            this.currentStyle = new HandleStyle(this);
        }
        public void Add(params LogicValueField[] logicvalues)
        {
            foreach (LogicValueField value in logicvalues)
                (value.IsInput ? inputs : outputs).Add(value);

            cachedFilteredInputsType = null;
        }

        public void DrawJustForDisplay()
        {
            if(Handles.Button(handleTarget.position, capDirection, HANDLESIZE_UNIMPORTANT, HANDLESIZE_UNIMPORTANT, CapForDisplay))
            {
                GameObject targetObject = handleTarget.gameObject;
                if (ValuesAssigned == 0)
                {
                    LogicValueField[] valueFields = GetRelevantValueFields();
                    targetObject = valueFields[0].onComponent.gameObject;
                    for (int i = 1; i < valueFields.Length; i++)
                    {
                        if (valueFields[i].onComponent.gameObject != targetObject)
                        {
                            targetObject = handleTarget.gameObject;
                            break;
                        }
                    }
                }
                if (Event.current.modifiers == EventModifiers.Shift)
                {
                    UnityEngine.Object[] currentSelection = Selection.gameObjects;
                    if (!ArrayUtility.Contains(currentSelection, targetObject))
                        ArrayUtility.Add(ref currentSelection, targetObject);

                    Selection.activeGameObject = targetObject;
                    Selection.objects = currentSelection;
                }
                else
                    Selection.SetActiveObjectWithContext(targetObject, targetObject);
            }
            DrawLable(0.4f, false);
        }
        public void ProcessHandle()
        {
            if (currentStyleState != currentLogicValueLinkState)
            {
                currentStyleState = currentLogicValueLinkState;
                currentStyle = new HandleStyle(this);
            }

            if (window != null)
            {
                ProcessWindow();
                return;
            }

            if (LookingForOutput)
                ProcessOutputs();
            else
                ProcessInputs();
        }
        private void ProcessWindow()
        {
            if (window.GetIfOptionSelected())
            {
                LogicValueField value = (LogicValueField)window.GetOption();

                if (LookingForOutput && value.IsOutput)
                    grabbedOutput = value;

                if (LookingForInput && value.IsInput)
                    targetInput = value;

                window = null;
                currentWindow = null;
            }
        }
        private void ProcessOutputs()
        {
            if (OutputsAssigned == 0)
            {
                HandleDrawUnimportant();
                return;
            }

            if (HandleButtonPressed())
            {
                if (OutputsAssigned > 1)
                    currentWindow = window = new SelectorWindow<LogicValueField>("Select Output", outputs.ToArray(), ValueName);
                else
                    grabbedOutput = outputs[0];
            }
        }
        private void ProcessInputs()
        {
            if (InputsFiltered == 0)
            {
                HandleDrawUnimportant();
                return;
            }
            if (HandleButtonPressed())
            {
                if (InputsFiltered > 1)
                    currentWindow = window = new SelectorWindow<LogicValueField>("Select Input", GetFilteredInputs(), ValueName);
                else
                    targetInput = GetFilteredInputs()[0];
            }
        }
        private LogicValueField[] GetFilteredInputs(bool forceCache = false)
        {
            if (LookingForOutput)
                return inputs.ToArray();

            if (!forceCache)
                if (cachedFilteredInputsType == grabbedOutput.LogicValueType)
                    return cachedFilteredInputs;

            Type cacheType = cachedFilteredInputsType = grabbedOutput.LogicValueType;
            return cachedFilteredInputs = inputs.Where(o => cacheType.IsAssignableFrom(o.LogicValueType)).ToArray();
        }
        private bool HandleButtonPressed()
        {
            bool result = Handles.Button(handleTarget.position, capDirection, HANDLESIZE_POI, HANDLESIZE_POI, Cap);
            DrawLable(1f);
            return result;
        }
        private void HandleDrawUnimportant()
        {
            Color oldColor = Handles.color;
            Handles.color = new Color(1f, 1f, 1f, 0.5f);
            Handles.Button(handleTarget.position, capDirection, HANDLESIZE_UNIMPORTANT, HANDLESIZE_UNIMPORTANT, Cap);
            Handles.color = oldColor;

            DrawLable(0.5f);
        }

        private void DrawLable(float factor = 1f, bool affectAlpha = true)
        {
            string handleName;
            LogicValueField[] relevantValueFields = GetRelevantValueFields();
            if (relevantValueFields.Length == 1)
                handleName = relevantValueFields[0].valueName;
            else
                handleName = $"[ {handleTarget.name} ]";

            Vector3 labelPosition = handleTarget.position + (sceneViewCameraTransform.up * 0.3f * factor);
            float distanceFactor = Vector2.Distance(HandleUtility.WorldToGUIPoint(handleTarget.position), HandleUtility.WorldToGUIPoint(labelPosition));
            //distanceFactor /= 2f;
            distanceFactor = Mathf.Min(distanceFactor, 16f);
            GUIStyle myHandleStyle = new GUIStyle(labelStyle);
            myHandleStyle.fontSize = Mathf.RoundToInt(distanceFactor);
            Color newColor = myHandleStyle.normal.textColor;

            if (affectAlpha)
                newColor.a = factor;


            myHandleStyle.normal.textColor = newColor;
            //Vector3 shadowPos = labelPosition + (sceneViewCameraTransform.right + (sceneViewCameraTransform.up * -1)) * 0.1f;
            if (!IsUnimportant || !IsCurrentTool)
            {
                GUIStyle shadowStyle = new GUIStyle(myHandleStyle);
                shadowStyle.contentOffset = new Vector2(1, 1);
                shadowStyle.normal.textColor = Color.black;
                Handles.Label(labelPosition, handleName, shadowStyle);
            }
            else
                myHandleStyle.fontStyle = FontStyle.Normal;
            Handles.Label(labelPosition, handleName, myHandleStyle);
        }

        private Handles.CapFunction Cap =>
            GetRelevantValueFields().Length == 1 ? Handles.SphereHandleCap : 
            (GetRelevantValueFields().Length == 0 ? Handles.CircleHandleCap : Handles.CubeHandleCap);
        private Handles.CapFunction CapForDisplay =>
            GetRelevantValueFields().Length == 1 ? Handles.CircleHandleCap :
            (GetRelevantValueFields().Length == 0 ? Handles.SphereHandleCap : Handles.RectangleHandleCap);

        private Quaternion capDirection => (GetRelevantValueFields().Length > 1 ? Quaternion.identity :
            Quaternion.LookRotation(sceneViewCameraTransform.forward, sceneViewCameraTransform.up));

        private string ValueName(LogicValueField valueField) =>
            $"<size=10>{valueField.onComponent.name}</size>: <b>{valueField.valueName}</b>";

        private bool IsUnimportant => GetRelevantValueFields().Length == 0;
       
        private LogicValueField[] GetRelevantValueFields()
        {
            if (LookingForOutput)
            {
                return outputs.ToArray();
            }
            else if (LookingForInput)
            {
                return GetFilteredInputs();
            }
            List<LogicValueField> valueFields = new List<LogicValueField>();
            valueFields.AddRange(inputs);
            valueFields.AddRange(outputs);
            return valueFields.ToArray();
        }

        public static Handle[] CompactByDistance(float distance = 0.01f, params Handle[] handles)
        {
            float sqrDistance = distance * distance;
            List<Handle> newHandles = new List<Handle>();
            for (int i = 0; i < handles.Length; i++)
            {
                bool doAdd = true;
                //Check if any of the previously added handles are close to the one about to be added
                for (int e = 0; e < newHandles.Count; e++)
                {
                    Vector3 deltaPosition = newHandles[e].handleTarget.position - handles[i].handleTarget.position;
                    if (deltaPosition.sqrMagnitude < sqrDistance)
                    {
                        newHandles[e].Add(handles[i].inputs.ToArray());
                        newHandles[e].Add(handles[i].outputs.ToArray());
                        doAdd = false;
                    }
                }
                if (doAdd)
                {
                    Handle handleToAdd = new Handle(handles[i].handleTarget);
                    handleToAdd.Add(handles[i].inputs.ToArray());
                    handleToAdd.Add(handles[i].outputs.ToArray());

                    newHandles.Add(handleToAdd);
                }
            }
            return newHandles.ToArray();
        }
        public static Handle[] SortByDistanceToCamera(params Handle[] handles)
        {
            int n = handles.Length;
            bool swapped;

            // Outer loop to iterate through all elements
            for (int i = 0; i < n - 1; i++)
            {
                swapped = false;

                // Inner loop to compare adjacent elements
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (CompareDistanceToCamera(handles[j], handles[j + 1]))
                    {
                        // Swap if elements are in the wrong order
                        Handle temp = handles[j];
                        handles[j] = handles[j + 1];
                        handles[j + 1] = temp;
                        swapped = true;
                    }
                }

                // If no two elements were swapped, the array is already sorted
                if (!swapped)
                    break;
            }
            return handles;
        }
        public static bool CompareDistanceToCamera(Handle h1, Handle h2)
        {
            return (h1.handleTarget.position - sceneViewCameraTransform.position).sqrMagnitude <
                (h2.handleTarget.position - sceneViewCameraTransform.position).sqrMagnitude;
        }
    
    
        public struct HandleStyle
        {
            public LogicValueField[] relevantValues;

            //Handle Info
            public Handles.CapFunction handleCap;
            public float handleSize;
            public Transform handlePosition;
            public Color handleColor;

            //Label info
            public GUIStyle labelStyle;
            public GUIStyle labelShadow;
            public string labelText;

            public static bool IsUsingTool => currentLogicValueLinkState != LogicValueLinkState.EditorToolClosed;

            public HandleStyle(Handle handle)
            {
                this.relevantValues = handle.GetRelevantValueFields();
                this.handlePosition = handle.handleTarget;

                this.handleCap = GetCap(this.relevantValues);
                this.handleSize = 2f;
                this.handleColor = Color.white;

                this.labelStyle = null;
                this.labelShadow = null;
                this.labelText = null;
            }

            public bool Draw(Vector3 position)
            {

                return false;
            }
            public void DrawLable()
            {

            }


            public static Handles.CapFunction GetCap(LogicValueField[] relevantValues)
            {
                Handles.CapFunction capToReturn;

                //If there are NO relevant values:
                if (relevantValues.Length == 0)
                    capToReturn = Handles.CircleHandleCap;

                //If there is exactly ONE relevant value:
                else if (relevantValues.Length == 1)
                    capToReturn = (IsUsingTool ? Handles.SphereHandleCap : Handles.CircleHandleCap);

                //If there is MULTIPLE relevant values:
                else
                    capToReturn = (IsUsingTool ? Handles.CubeHandleCap : Handles.RectangleHandleCap);

                return capToReturn;
            }
        }
    
    }
    public struct ValueLink
    {
        public Type linkType;
        public Transform start;
        public Transform end;
        public ValueLink(Type linkType, Transform start, Transform end)
        {
            this.linkType = linkType;
            this.start = start;
            this.end = end;
        }

        public void Draw(Color color, float thickness)
        {
            Color old = Handles.color;
            Handles.color = color;

            Vector3 delta = end.position - start.position;
            Vector3 offsetDirection = Vector3.Cross(delta, sceneViewCameraTransform.forward);
            offsetDirection = offsetDirection.normalized * 0.05f;
            Handles.DrawLine(start.position + offsetDirection, end.position + offsetDirection, thickness);
            Handles.DrawLine(start.position - offsetDirection, end.position - offsetDirection, thickness);
            for (float i = 0; i < delta.magnitude - .1f; i += .3f)
            {
                Vector3 onLinePosition = start.position + (delta.normalized * i);
                Vector3 furtherUpOffset = (delta.normalized * .1f);
                Handles.DrawLine(onLinePosition + furtherUpOffset, onLinePosition + offsetDirection, thickness * 2f);
                Handles.DrawLine(onLinePosition + furtherUpOffset, onLinePosition - offsetDirection, thickness * 2f);
            }
            Handles.color = old;
        }
        public void DrawJustForDisplay()
        {
            //Color old = Handles.color;
            //Handles.color = Color.white;

            Vector3 delta = end.position - start.position;

            Vector3 offsetDirection = Vector3.Cross(delta, sceneViewCameraTransform.forward);
            offsetDirection = offsetDirection.normalized * 0.05f;

            //Vector3 gapPosition = start.position + (delta * 0.8f);
            //Vector3 gapOffset = delta.normalized * Mathf.Min(0.1f, delta.magnitude * 0.05f);

            EditorGUILayout.LabelField($"{start.position} -> {end.position}");
            Handles.DrawLine(start.position + offsetDirection, end.position, 3f);
            Handles.DrawLine(start.position - offsetDirection, end.position, 3f);

            //Handles.color = old;
        }
    }

    private static void CacheFieldInfos()
    {
        if (LookingForInput)
            return;

        if (logicValueFieldInfos != null)
            return;
        
        ClearLogicValuesCache();

        logicValueFieldInfos = new Dictionary<Type, FieldInfo[]>();

        allComponents = FindObjectsOfType<Component>();
        List<Type> allUniqueTypes = new List<Type>();

        foreach (Component c in allComponents)
        {
            Type componentType = c.GetType();
            if (!allUniqueTypes.Contains(componentType))
                allUniqueTypes.Add(componentType);
        }

        List<Type> uniqueTypesWithLogicValues = new List<Type>();
        foreach (Type currentType in allUniqueTypes)
        {
            FieldInfo[] fieldInfos = currentType
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(info => typeof(LogicValue).IsAssignableFrom(info.FieldType))
                .ToArray();

            if (fieldInfos.Length > 0)
            {
                uniqueTypesWithLogicValues.Add(currentType);
                logicValueFieldInfos.Add(currentType, fieldInfos);
            }
        }
        typesWithLogicValues = uniqueTypesWithLogicValues.ToArray();
    }
    private static void CacheLogicValues()
    {
        if (LookingForInput)
            return;

        if (logicValues != null)
            return;

        ClearLogicValuesCache();

        List<Component> componentsWithLogicValuesList = new List<Component>();
        logicValues = new Dictionary<Component, LogicValueField[]>();
        List<LogicValueField> inputsList = new List<LogicValueField>();
        List<LogicValueField> outputsList = new List<LogicValueField>();
        List<ValueLink> linksList = new List<ValueLink>();
        allComponents = FindObjectsOfType<Component>();
        foreach (Component currentComponent in allComponents)
        {
            Type currentComponentType = currentComponent.GetType();
            if (typesWithLogicValues.Contains(currentComponentType))
            {
                componentsWithLogicValuesList.Add(currentComponent);
                List<LogicValueField> logicValueList = new List<LogicValueField>();
                foreach (FieldInfo field in logicValueFieldInfos[currentComponentType])
                {
                    LogicValueField logicValueField = new LogicValueField(field, currentComponent);
                    logicValueList.Add(logicValueField);

                    if (logicValueField.IsOutput)
                        outputsList.Add(logicValueField);

                    else if (logicValueField.IsInput)
                    {
                        inputsList.Add(logicValueField);
                        LogicInput input = logicValueField.Input;
                        if (input.HasLogicOutputSource) 
                        {
                            LogicValue otherValue = input.GetSourceLogicOutput();
                            Transform startHandle = otherValue.EditorHandles_GetHandle();
                            Transform endHandle = input.EditorHandles_GetHandle();

                            if (endHandle == null)
                                endHandle = currentComponent.transform;

                            if (startHandle == null) 
                                startHandle = input.GetSourceLogicOutputComponent().transform;

                            if (startHandle != null && endHandle != null)
                                linksList.Add(new ValueLink(input.GetLogicValueType(), startHandle, endHandle));
                        }
                    }
                }
                logicValues.Add(currentComponent, logicValueList.ToArray());
            }
        }
        componentsWithLogicValues = componentsWithLogicValuesList.ToArray();
        inputs = inputsList.ToArray();
        outputs = outputsList.ToArray();
        links = linksList.ToArray();

        handles = new List<Handle>();
        AddValueFieldsToHandle(inputs);
        AddValueFieldsToHandle(outputs); 
    }

    private static void RESET()
    {
        ClearALL();
        SceneView.RepaintAll();
    }
    private static void ClearLogicValuesCacheAndSelectedValues()
    {
        ClearLogicValuesCache();
        ClearSelectedValues();
    }
    private static void ClearLogicValuesCache()
    {
        componentsWithLogicValues = null;
        logicValues = null;
        inputs = null;
        outputs = null;
        grabbedOutput = null;
        handles = null;
        links = null;
    }
    private static void ClearSelectedValues()
    {
        grabbedOutput = null;
        targetInput = null;
        currentWindow = null;
    }
    private static void ClearLogicValueFieldInfos()
    {
        logicValueFieldInfos = null;
        typesWithLogicValues = null;
        allComponents = null;
    }
    private static void ClearALL() 
    {
        ClearLogicValueFieldInfos();
        ClearLogicValuesCacheAndSelectedValues();
    }

    public override void OnToolGUI(EditorWindow window)
    {
        if (!(window is SceneView)) return;
        
        try
        {
            BeforeGUI();
            DrawGUI();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            ClearALL();
        }
        SceneView.RepaintAll();
    }
    public static void BeforeGUI()
    {
        if (NotCurrentlyDoingAnything && (Event.current.isKey))
            ClearALL();

        CacheFieldInfos();
        CacheLogicValues();

        if (labelStyle == null)
        {
            labelStyle = new GUIStyle();
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = Color.white;
        }

        if (sceneViewCameraTransform == null)
            sceneViewCameraTransform = SceneView.lastActiveSceneView.camera.transform;
    }
    public void DrawGUI()
    {
        foreach (ValueLink link in links)
        {
            link.Draw(Color.white, 1f);
        }
        foreach (Handle handle in Handle.SortByDistanceToCamera(handles.ToArray()))
        {
            handle.ProcessHandle();
        }
        if (LookingForOutput)
        {
            Vector3 mousePosition = sceneViewCameraTransform.position;
            Ray cursorForwardRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            mousePosition += cursorForwardRay.direction;
            Handles.DrawWireDisc(mousePosition, sceneViewCameraTransform.forward, 0.025f);
        }
        if (LookingForInput)
        {
            Vector3 mousePosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
            Handles.DrawLine(grabbedOutput.handleTarget.position, mousePosition, 8f);
        }
        if (LinkHasFinished)
        {
            if (!(targetInput.Input.HasLogicOutputSource &&
                targetInput.Input.GetSourceLogicOutputComponent() == grabbedOutput.onComponent &&
                targetInput.Input.GetSourceLogicOutput() == grabbedOutput.Output))
            {
                Undo.RecordObject(targetInput.onComponent,
                $"Assigning input \"{targetInput.valueName}\" on {targetInput.onComponent.name} " +
                $"to reference output \"{grabbedOutput.valueName}\" on {grabbedOutput.onComponent.name}");

                targetInput.Input.SetFieldReference(grabbedOutput.onComponent, grabbedOutput.fieldInfo.Name);
                targetInput.fieldInfo.SetValue(targetInput.onComponent, targetInput.value.GetClone());

                EditorUtility.SetDirty(targetInput.onComponent);
            }
            ClearALL();
        }
    }

    public interface SelectorWindowNonGeneric
    {
        public bool GetIfOptionSelected();
        public object GetOption();
    }
    public class SelectorWindow<T> : SelectorWindowNonGeneric
    {
        private GUIStyle buttonStyle;
        private string title;
        private T[] returnItems;
        private Func<T, string> itemToString;

        private T selectedItem;
        private bool itemHasBeenSelected;

        private GUID windowID;
        private Rect windowArea;

        public SelectorWindow(string title, T[] returnItems, Func<T, string> itemToString)
        {
            this.buttonStyle = new GUIStyle(EditorStyles.miniButton);
            this.buttonStyle.richText = true;

            this.title = title;
            this.returnItems = returnItems;
            this.itemToString = itemToString;
            this.itemHasBeenSelected = false;
            CalculateWindowArea();
        }
        private void CalculateWindowArea()
        {
            windowID = new GUID();

            float width = 100f;
            foreach (T item in returnItems)
                width = Mathf.Max(width, buttonStyle.CalcSize(new GUIContent(itemToString.Invoke(item))).x);
            float height = returnItems.Length * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

            windowArea.size = new Vector2(width, height);
            windowArea.center = Event.current.mousePosition;
        }

        public bool GetIfOptionSelected()
        {
            windowArea = GUILayout.Window(windowID.GetHashCode(), windowArea, LayoutWindow, title);
            return itemHasBeenSelected;
        }
        public object GetOption() => selectedItem;
        private void LayoutWindow(int window)
        {
            foreach (T item in returnItems)
            {
                if (GUILayout.Button(itemToString.Invoke(item), buttonStyle))
                {
                    selectedItem = item;
                    itemHasBeenSelected = true;
                }
            }
        }
    }

    public static void AddValueFieldsToHandle(params LogicValueField[] valueFields)
    {
        foreach(LogicValueField valueField in valueFields)
            AddValueFieldsToHandle(valueField);
    }
    public static void AddValueFieldsToHandle(LogicValueField valueField)
    {
        for (int i = 0; i < handles.Count; i++)
        {
            if (handles[i].handleTarget == valueField.handleTarget)
            {
                handles[i].Add(valueField);
                return;
            }
        }
        Handle newHandle = new Handle(valueField.handleTarget);
        newHandle.Add(valueField);
        handles.Add(newHandle);
    }
}
