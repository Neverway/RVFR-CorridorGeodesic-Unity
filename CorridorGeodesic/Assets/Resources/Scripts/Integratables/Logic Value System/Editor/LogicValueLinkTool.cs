using DG.Tweening;
using Neverway.Framework.LogicValueSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Plastic.Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.Rendering;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.MessageBox;

//[EditorTool("Logic Value Link Tool")]
public class LogicValueLinkTool : EditorTool
{
    #region EditorTool Setup
    public static bool ShowGUIInScene = false;

    [InitializeOnLoadMethod]
    public static void StartGUIToggled()
    {
        ShowGUIInScene = false;
        ToggleSceneGUI();
    }
    [MenuItem("Neverway/Logic Value Link Tool/Toggle Scene GUI")]
    public static void ToggleSceneGUI()
    {
        ShowGUIInScene = !ShowGUIInScene;
        if (ShowGUIInScene)
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }
        else
        {
            SceneView.duringSceneGui -= OnSceneGUI;
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
        //style.padding = new RectOffset();
        style.fixedHeight = toolButtonArea.height;
        //EditorGUI.DrawRect(toolButtonArea, Color.white);

        if (IsCurrentTool != EditorGUI.Toggle(toolButtonArea, IsCurrentTool, style))
            SetAsCurrentTool();

        if (IsCurrentTool)
        {

        }
        else
        {
            
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

    private const float HANDLESIZE_POI = 0.3f;
    private const float HANDLESIZE_UNIMPORTANT = 0.15f;

    private static LogicValueField grabbedOutput = null;
    private static LogicValueField targetInput = null;
    private static SelectorWindowNonGeneric currentWindow = null;
    public bool NotCurrentlyDoingAnything => LookingForOutput && currentWindow == null;
    public static bool LookingForOutput => grabbedOutput == null && targetInput == null;
    public static bool LookingForInput => grabbedOutput != null && targetInput == null;
    public static bool LinkHasFinished => grabbedOutput != null && targetInput != null;

    private List<Handle> handles;
    private static Transform sceneViewCameraTransform;
    private LogicValueField[] inputs;
    private LogicValueField[] outputs;
    private ValueLink[] links;
    private Dictionary<Component, LogicValueField[]> logicValues;
    private Component[] componentsWithLogicValues;
    private Dictionary<Type, FieldInfo[]> logicValueFieldInfos;
    private Type[] typesWithLogicValues;
    private Component[] allComponents;

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
        public SelectorWindowNonGeneric window;
        public Transform handleTarget;
        public List<LogicValueField> inputs;
        public List<LogicValueField> outputs;
        
        public bool HasOutputs => outputs.Count > 0;
        public bool HasMultipleOutputs => outputs.Count > 1;
        public bool HasInputs => ValidInputs.Length > 0;
        public bool HasMultipleInputs => ValidInputs.Length > 1;
        public LogicValueField[] ValidInputs => GetCachedValidInputs();

        private LogicValueField[] cachedValidInputs;
        private Type cachedValidInputsType;

        public Handle(Transform handleTarget)
        {
            if (handleTarget == null)
                throw new ArgumentNullException(nameof(handleTarget));

            this.cachedValidInputsType = typeof(object);
            this.cachedValidInputs = new LogicValueField[0];
            this.window = null;
            this.handleTarget = handleTarget;
            this.inputs = new List<LogicValueField>();
            this.outputs = new List<LogicValueField>();
        }
        public void Add(params LogicValueField[] logicvalues)
        {
            foreach (LogicValueField value in logicvalues)
                (value.IsInput ? inputs : outputs).Add(value);

            cachedValidInputsType = null;
        }

        public void ProcessHandle()
        {
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
            if (!HasOutputs)
            {
                HandleDrawUnimportant();
                return;
            }

            if (HandleButtonPressed())
            {
                if (HasMultipleOutputs)
                    currentWindow = window = new SelectorWindow<LogicValueField>("Select Output", outputs.ToArray(), ValueName);
                else
                    grabbedOutput = outputs[0];
            }
        }
        private void ProcessInputs()
        {
            if (!HasInputs)
            {
                HandleDrawUnimportant();
                return;
            }
            if (HandleButtonPressed())
            {
                if (HasMultipleInputs)
                    currentWindow = window = new SelectorWindow<LogicValueField>("Select Input", GetCachedValidInputs(), ValueName);
                else
                    targetInput = GetCachedValidInputs()[0];
            }
        }
        private LogicValueField[] GetCachedValidInputs(bool forceCache = false)
        {
            if (LookingForOutput)
                return inputs.ToArray();

            if (!forceCache)
                if (cachedValidInputsType == grabbedOutput.LogicValueType)
                    return cachedValidInputs;

            Type cacheType = cachedValidInputsType = grabbedOutput.LogicValueType;
            return cachedValidInputs = inputs.Where(o => cacheType.IsAssignableFrom(o.LogicValueType)).ToArray();
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

        private void DrawLable(float factor = 1f)
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
            newColor.a = factor;
            myHandleStyle.normal.textColor = newColor;
            //Vector3 shadowPos = labelPosition + (sceneViewCameraTransform.right + (sceneViewCameraTransform.up * -1)) * 0.1f;
            if (!IsUnimportant)
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
                return GetCachedValidInputs();
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
    }

    private void CacheFieldInfos()
    {
        if (!LookingForOutput)
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
    private void CacheLogicValues()
    {
        if (!LookingForOutput)
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

    private void RESET()
    {
        ClearALL();
        SceneView.RepaintAll();
    }
    private void ClearLogicValuesCacheAndSelectedValues()
    {
        ClearLogicValuesCache();
        ClearSelectedValues();
    }
    private void ClearLogicValuesCache()
    {
        componentsWithLogicValues = null;
        logicValues = null;
        inputs = null;
        outputs = null;
        grabbedOutput = null;
        handles = null;
        links = null;
    }
    private void ClearSelectedValues()
    {
        grabbedOutput = null;
        targetInput = null;
        currentWindow = null;
    }
    private void ClearLogicValueFieldInfos()
    {
        logicValueFieldInfos = null;
        typesWithLogicValues = null;
        allComponents = null;
    }
    private void ClearALL() 
    {
        ClearLogicValueFieldInfos();
        ClearLogicValuesCacheAndSelectedValues();
    }

    public override void OnToolGUI(EditorWindow window)
    {
        if (!(window is SceneView)) return;
        if (NotCurrentlyDoingAnything && (Event.current.isKey))
            ClearALL();

        try
        {
            CacheFieldInfos();
            CacheLogicValues();

            DrawGUI();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            ClearALL();
        }
        SceneView.RepaintAll();
    }
    public void DrawGUI()
    {
        if (labelStyle == null)
        {
            labelStyle = new GUIStyle();
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = Color.white;
        } 

        if (sceneViewCameraTransform == null)
            sceneViewCameraTransform = SceneView.lastActiveSceneView.camera.transform;

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

    public void AddValueFieldsToHandle(params LogicValueField[] valueFields)
    {
        foreach(LogicValueField valueField in valueFields)
            AddValueFieldsToHandle(valueField);
    }
    public void AddValueFieldsToHandle(LogicValueField valueField)
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
