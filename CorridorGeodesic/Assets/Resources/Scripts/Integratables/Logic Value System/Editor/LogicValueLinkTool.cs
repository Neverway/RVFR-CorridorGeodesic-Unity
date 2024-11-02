using Neverway.Framework.LogicValueSystem;
using Neverway.Framework.Utility.StateMachine;
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
using static LogicValueLinkTool;

public class LogicValueLinkTool : EditorTool
{
    #region EditorTool Setup ------------------------------------------------------------------------------------------------------------------------

    [InitializeOnLoadMethod]
    public static void Initialize()
    {
        LogicValueLinkToolSettings.Instance.OnSettingsValidate += OnSettingsChanged;
        Undo.undoRedoPerformed += NewStateMachine;
        OnSettingsChanged();
        NewStateMachine();
    }
    public static void OnSettingsChanged()
    {
        if (LogicValueLinkToolSettings.Instance.EditorToolUsesSceneGUI)
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
        }
        else
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        m_Icon = null;
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
        NewStateMachine();
        ToolManager.activeToolChanged += ClearLastToolType;
    }
    public static void ClearLastToolType()
    {
        lastToolType = Tool.None;
        ToolManager.activeToolChanged -= ClearLastToolType;
    }
    public void OnDisable()
    {
        ClearLastToolType();
    }

    // Define the icon displayed in the toolbar
    private static GUIContent m_Icon;
    public override GUIContent toolbarIcon => ToolbarIconContent;
    public static GUIContent ToolbarIconContent
    {
        get 
        {
            if (m_Icon == null)
            {
                Texture icon = LogicValueLinkToolSettings.Instance.toolbarIcon;
                if (icon == null)
                    icon = EditorIcons.SettingsIcon;

                m_Icon =  new GUIContent()
                {
                    image = icon,
                    text = "Logic Value Link Tool",
                    tooltip = "Logic Value Link Tool (Alt + L)"
                };
            }
            return m_Icon;
        }
    }
    #endregion --------------------------------------------------------------------------------------------------------------------------------------

    private static ValueLinkMachine stateMachine;
    private static bool wasInPlayMode = false;
    private static void NewStateMachine() => stateMachine = new ValueLinkMachine(new ValueLinkMachine.StartingState());

    private static void OnSceneGUI(SceneView sceneview)
    {
        if (!IsCurrentTool)
            OnMainGUI(sceneview);
    }
    public override void OnToolGUI(EditorWindow window)
    {
        if (window is SceneView sceneview)
            OnMainGUI(sceneview);
    }
    public static void OnMainGUI(SceneView sceneview)
    {
        if ((wasInPlayMode ^ Application.isPlaying))
        {
            wasInPlayMode = Application.isPlaying;
            NewStateMachine();
            return;
        }
        if (stateMachine == null)
        {
            NewStateMachine();
            return;
        }

        stateMachine.DoUpdate(sceneview);
        SceneView.RepaintAll();
    }

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
        public Transform handleTarget;
        public LogicValueField[] baseValues;
        public LogicValueField[] filteredValues;

        public const float HANDLESIZE_POI = 0.3f;
        public const float HANDLESIZE_UNIMPORTANT = 0.15f;
        public const float HANDLESIZE_DISPLAY = 0.07f;


        public Handle(Transform handleTarget)
        {
            if (handleTarget == null)
                throw new ArgumentNullException(nameof(handleTarget));

            this.handleTarget = handleTarget;
            baseValues = new LogicValueField[0];
            filteredValues = new LogicValueField[0];
        }
        public void AddBaseValues(params LogicValueField[] logicvalues)
        {
            List<LogicValueField> modifiedValues = new List<LogicValueField>();
            modifiedValues.AddRange(baseValues);
            modifiedValues.AddRange(logicvalues);
            baseValues = modifiedValues.ToArray();
            filteredValues = baseValues;
        }

        public void SetValueFilter(Func<LogicValueField, bool> valueFilter)
        {
            filteredValues = baseValues.Where(valueFilter).ToArray();
        }
        
        public void DrawJustForDisplay()
        {
            if(Handles.Button(handleTarget.position, capDirection, HANDLESIZE_DISPLAY, HANDLESIZE_DISPLAY, CapForDisplay))
            {
                if (!LogicValueLinkToolSettings.Instance.selectTransformWhenNotUsingTool)
                    return;

                GameObject targetObject = handleTarget.gameObject;
                if (filteredValues.Length > 0)
                {
                    targetObject = filteredValues[0].onComponent.gameObject;
                    for (int i = 1; i < filteredValues.Length; i++)
                    {
                        if (filteredValues[i].onComponent.gameObject != targetObject)
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
            DrawLable(0.6f, false);
        }
        public bool DrawHandleAndGetPressed()
        {
            if (filteredValues.Length == 0)
            {
                HandleDrawUnimportant();
                return false;
            }
            return HandleButtonPressed();
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
            if (baseValues.Length == 1)
                handleName = baseValues[0].valueName;
            else if (filteredValues.Length == 1)
                handleName = filteredValues[0].valueName;
            else
                handleName = $"[ {handleTarget.name} ]";

            Vector3 labelPosition = handleTarget.position + (ValueLinkMachine.sceneViewCameraTransform.up * 0.3f * factor);
            float distanceFactor = Vector2.Distance(HandleUtility.WorldToGUIPoint(handleTarget.position), HandleUtility.WorldToGUIPoint(labelPosition));
            //distanceFactor /= 2f;
            distanceFactor = Mathf.Min(distanceFactor, 16f);
            GUIStyle myHandleStyle = new GUIStyle(stateMachine.labelStyle);
            myHandleStyle.fontSize = Mathf.RoundToInt(distanceFactor);
            Color newColor = (myHandleStyle.normal.textColor + Handles.color) / 2;

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
            filteredValues.Length == 1 ? Handles.SphereHandleCap : 
            (filteredValues.Length == 0 ? Handles.CircleHandleCap : Handles.CubeHandleCap);
        private Handles.CapFunction CapForDisplay =>
            filteredValues.Length == 1 ? Handles.CircleHandleCap :
            (filteredValues.Length == 0 ? Handles.SphereHandleCap : Handles.RectangleHandleCap);

        private Quaternion capDirection => ((IsCurrentTool && filteredValues.Length > 1) ? Quaternion.identity :
            Quaternion.LookRotation(ValueLinkMachine.sceneViewCameraTransform.forward, ValueLinkMachine.sceneViewCameraTransform.up));

        private string ValueName(LogicValueField valueField) =>
            $"<size=10>{valueField.onComponent.name}</size>: <b>{valueField.valueName}</b>";

        private bool IsUnimportant => filteredValues.Length == 0;

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
                        newHandles[e].AddBaseValues(handles[i].baseValues);
                        doAdd = false;
                    }
                }
                if (doAdd)
                {
                    Handle handleToAdd = new Handle(handles[i].handleTarget);
                    handleToAdd.AddBaseValues(handles[i].baseValues);

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
            return (h1.handleTarget.position - ValueLinkMachine.sceneViewCameraTransform.position).sqrMagnitude <
                (h2.handleTarget.position - ValueLinkMachine.sceneViewCameraTransform.position).sqrMagnitude;
        }
    
    
        public struct HandleDrawInfo
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

            public static bool IsUsingTool => IsCurrentTool;

            public HandleDrawInfo(Handle handle)
            {
                this.relevantValues = handle.filteredValues;
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
            Vector3 offsetDirection = Vector3.Cross(delta, ValueLinkMachine.sceneViewCameraTransform.forward);
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
            Vector3 delta = end.position - start.position;

            Vector3 offsetDirection = Vector3.Cross(delta, ValueLinkMachine.sceneViewCameraTransform.forward);
            offsetDirection = offsetDirection.normalized * Handle.HANDLESIZE_DISPLAY;

            Vector3 targetLocation = start.position + delta.normalized * (delta.magnitude - Handle.HANDLESIZE_DISPLAY - 0.01f);

            Handles.DrawLine(start.position + offsetDirection, targetLocation, 1f);
            Handles.DrawLine(start.position - offsetDirection, targetLocation, 1f);
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
        public void SetWindowPosition(Vector3 scenePosition)
        {
            
            windowArea.center = HandleUtility.WorldToGUIPoint(scenePosition);
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


















    public class ValueLinkMachine : StateMachine<ValueLinkMachine>
    {
        //todo: Update window position to handle when moving scene view
        public static Dictionary<Type, FieldInfo[]> typeToLogicValueFields { get; private set; }
        public Handle[] handles;
        public ValueLink[] links;
        private Func<LogicValueField, bool> _handleFilter;
        public Func<LogicValueField, bool> HandleFilter
        {
            get { return (_handleFilter == null) ? NoFilter : _handleFilter; }
            set { _handleFilter = value; }
        }
        public Func<LogicValueField, bool> NoFilter => (v => true);

        public Rect sceneViewportArea { get; private set; }
        private Vector3 mousePosition;
        public GUIStyle labelStyle;
        public static Transform sceneViewCameraTransform;
        public bool IsCurrentStateRemovingLinks => currentState is RemovingLinkState ||
            (currentState is WindowValueSelectState windowState && windowState.comingFromState is RemovingLinkState);

        public LogicValueField selectedOutput = null;

        public ValueLinkMachine(State<ValueLinkMachine> startingState) : base(startingState) 
        {
            this.labelStyle = new GUIStyle();
            this.labelStyle.alignment = TextAnchor.MiddleCenter;
            this.labelStyle.fontStyle = FontStyle.Bold;
            this.labelStyle.normal.textColor = Color.white;

            if (typeToLogicValueFields == null)
                InitializeTypeFieldInfos();

            RefreshHandlesAndLinks();
        }

        public void DoUpdate(SceneView sceneView)
        {
            if (sceneView == null) return;

            Transform sceneCamera = sceneView.camera.transform;
            if (sceneCamera != null)
                sceneViewCameraTransform = sceneCamera;

            this.sceneViewportArea = sceneView.cameraViewport;
            DoUpdate();
        }
        public override void DoUpdate()
        {
            if (currentState is StartingState)
            {
                TryDrawToolButton();
                base.DoUpdate();
                return;
            }

            if (currentState is RefreshableState && (Event.current.isKey))
                RefreshHandlesAndLinks();

            if (currentState is not WindowValueSelectState)
            {
                mousePosition = sceneViewCameraTransform.position;
                Ray cursorForwardRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                mousePosition += cursorForwardRay.direction;
            }

            if (IsCurrentTool ^ (currentState is not ToolNotActiveState))
            {
                ChangeState(new StartingState());
                return;
            }
            if (IsCurrentTool)
            {
                bool isPressingShift = Event.current.shift;
                if (isPressingShift ^ IsCurrentStateRemovingLinks)
                {
                    ChangeState(isPressingShift ? new RemovingLinkState() : new StartingState());
                    return;
                }
            }


            TryDrawToolButton();

            base.DoUpdate();
        }

        public void InitializeTypeFieldInfos()
        {
            typeToLogicValueFields = new Dictionary<Type, FieldInfo[]>();

            //Get all types from all assemblies that are MonoBehaviours
            Type[] allMonoBehvaiourTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => typeof(MonoBehaviour).IsAssignableFrom(t) && !t.IsAbstract)
                .ToArray();

            //Loop through all the MonoBehvaiour types to see if they have LogicValue fields
            foreach (Type currentType in allMonoBehvaiourTypes)
            {
                //Get all FieldInfos for type LogicValue
                FieldInfo[] fieldInfos = currentType
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(info => typeof(LogicValue).IsAssignableFrom(info.FieldType))
                    .ToArray();

                //If there is at least 1 LogicValue field, add it to the type-to-logicvalues dictionary
                if (fieldInfos.Length > 0)
                    typeToLogicValueFields.Add(currentType, fieldInfos);
            }
        }
        public void RefreshHandlesAndLinks()
        {
            //Find all logic value fields from all monobehaviours
            LogicValueField[] logicValueFields = FindLogicValueFieldsInScene();

            //Setup handle based on logic values found
            handles = LogicValueFieldsToHandles(logicValueFields);

            foreach (Handle handle in handles)
                handle.SetValueFilter(HandleFilter);

            //Setup links based on logic values found
            links = LogicValueFieldsToLinks(logicValueFields);
        }

        private LogicValueField[] FindLogicValueFieldsInScene()
        {
            List<LogicValueField> logicValueFieldsToReturn = new List<LogicValueField>();

            foreach (MonoBehaviour currentMonoBehaviour in FindObjectsOfType<MonoBehaviour>())
            {
                Type currentComponentType = currentMonoBehaviour.GetType();

                //If this monobehvaiour type is not in the dictionary of types that have logic value fields, skip it
                if (!typeToLogicValueFields.ContainsKey(currentComponentType))
                    continue;

                //Get the FieldInfos from the type-to-logicvaluefields dictionary and store the information from it to return
                foreach (FieldInfo field in typeToLogicValueFields[currentComponentType])
                    logicValueFieldsToReturn.Add(new LogicValueField(field, currentMonoBehaviour));

            }
            return logicValueFieldsToReturn.ToArray();
        }
        private Handle[] LogicValueFieldsToHandles(LogicValueField[] valueFields)
        {
            List<Handle> handlesToReturn = new List<Handle>();

            foreach (LogicValueField valueField in valueFields)
            {
                //If handleTarget was already used in previous handle, add this value to that handle instead to group them
                bool handleFound = false;
                for (int i = 0; !handleFound && i < handlesToReturn.Count; i++)
                {
                    if (handlesToReturn[i].handleTarget == valueField.handleTarget)
                    {
                        handlesToReturn[i].AddBaseValues(valueField);
                        handleFound = true;
                    }
                }
                if (handleFound) continue;

                //Otherwise, create a new handle as this handleTarget has not been used for a handle yet
                Handle newHandle = new Handle(valueField.handleTarget);
                newHandle.AddBaseValues(valueField);
                handlesToReturn.Add(newHandle);
            }
            return handlesToReturn.ToArray();
        }
        private ValueLink[] LogicValueFieldsToLinks(LogicValueField[] valueFields)
        {
            List<ValueLink> linksToReturn = new List<ValueLink>();

            foreach (LogicValueField valueField in valueFields)
            {
                //Value has to be an input and have an output source attached to 
                if (!(valueField.IsInput && valueField.Input.HasLogicOutputSource))
                    continue;

                LogicInput inputValue = valueField.Input;
                LogicValue outputValue = inputValue.GetSourceLogicOutput();

                //Set up the start position of the link
                Transform startHandle = outputValue.EditorHandles_GetHandle();
                if (startHandle == null)
                    startHandle = inputValue.GetSourceLogicOutputComponent().transform;

                //Set up the end position of the link
                Transform endHandle = inputValue.EditorHandles_GetHandle();
                if (endHandle == null)
                    endHandle = valueField.onComponent.transform;

                //If start and end positions are set, go ahead and add that link to the list
                if (startHandle != null && endHandle != null)
                    linksToReturn.Add(new ValueLink(inputValue.GetLogicValueType(), startHandle, endHandle));
            }
            return linksToReturn.ToArray();
        }

        public void TryDrawToolButton()
        {
            //If tool button was turned off in settings, skip drawing the button
            if (!LogicValueLinkToolSettings.Instance.showToolButton)
                return;

            Handles.BeginGUI();

            Rect toolButtonArea = sceneViewportArea;
            toolButtonArea.size = new Vector2(30, 30);
            toolButtonArea.center = new Vector2(sceneViewportArea.width / 2, 21);

            //Offset from probuilder bar
            toolButtonArea.x += 85;

            GUIStyle style = new GUIStyle(EditorStyles.miniButton);
            style.fixedHeight = toolButtonArea.height;

            if (IsCurrentTool != EditorGUI.Toggle(toolButtonArea, IsCurrentTool, style))
                SetAsCurrentTool();

            GUIStyle alignedCenter = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
            EditorGUI.LabelField(toolButtonArea, new GUIContent(ToolbarIconContent.image), EditorStyles.centeredGreyMiniLabel);

            Handles.EndGUI();
        }
        public void DrawLinkFromOutputToMouse()
        {
            Handles.DrawLine(selectedOutput.handleTarget.position, mousePosition, 8f);
        }
        public void DrawCircleAroundMouse()
        {
            Handles.DrawWireDisc(mousePosition, sceneViewCameraTransform.forward, 0.015f);
        }
        
        public Handle DrawHandlesAndGetPressedStandard()
        {
            Handle pressedHandle = null;
            foreach (Handle handle in Handle.SortByDistanceToCamera(handles))
            {
                if (handle.DrawHandleAndGetPressed())
                    pressedHandle = handle;
            }
            return pressedHandle;
        }
        public void DrawLinksStandard()
        {
            foreach (ValueLink link in links)
            {
                link.Draw(Color.white, 1f);
            }
        }

        public void DrawHandlesSimplified()
        {
            foreach (Handle handle in Handle.SortByDistanceToCamera(handles))
                handle.DrawJustForDisplay();
        }
        public void DrawLinksSimplified()
        {
            foreach (ValueLink link in links)
            {
                link.DrawJustForDisplay();
            }
        }



        public interface RefreshableState { }
        public abstract class ValueLinkState : State<ValueLinkMachine>
        {
            public abstract Func<LogicValueField, bool> GetValueFilter();
            public override void OnEnter(State<ValueLinkMachine> lastState)
            {
                if (lastState != null)
                    StateMachine.HandleFilter = GetValueFilter();
            }
        }
        public class StartingState : ValueLinkState
        {
            public override Func<LogicValueField, bool> GetValueFilter() => StateMachine.NoFilter;

            public override void OnEnter(State<ValueLinkMachine> lastState) { }

            public override void OnUpdate() 
            {
                StateMachine.selectedOutput = null;
                base.OnEnter(null);
                if (IsCurrentTool)
                    stateMachine.ChangeState(new SelectingOutputState());
                else
                    stateMachine.ChangeState(new ToolNotActiveState());
            }
        }
        public class ToolNotActiveState : ValueLinkState, RefreshableState
        {
            public override Func<LogicValueField, bool> GetValueFilter() => StateMachine.NoFilter;

            public override void OnEnter(State<ValueLinkMachine> lastState) 
            {
                base.OnEnter(lastState);
                StateMachine.RefreshHandlesAndLinks();
            }

            public override void OnUpdate()
            {
                if (LogicValueLinkToolSettings.Instance.showLinksWhenNotUsingTool)
                {
                    StateMachine.DrawLinksSimplified();
                    StateMachine.DrawHandlesSimplified();
                }
            }
        }
        
        public class SelectingOutputState : SelectingHandleState, RefreshableState
        {
            public override Func<LogicValueField, bool> GetValueFilter() => (v => v.IsOutput);
            protected override string GetSelectorWindowTitle() => "Select output to link from";

            public override void OnUpdate()
            {
                StateMachine.DrawCircleAroundMouse();
                base.OnUpdate();
            }
            protected override void OnSelectionFinished(LogicValueField selectedValue)
            {
                stateMachine.selectedOutput = selectedValue;
                stateMachine.ChangeState(new SelectingInputState());
            }
        }
        public class SelectingInputState : SelectingHandleState
        {
            public override Func<LogicValueField, bool> GetValueFilter() => 
                (v => v.IsInput && v.LogicValueType.IsAssignableFrom(StateMachine.selectedOutput.LogicValueType));
            protected override string GetSelectorWindowTitle() => "Select input to link to";
            public override void OnUpdate()
            {
                StateMachine.DrawLinkFromOutputToMouse();
                base.OnUpdate();
            }

            protected override void OnSelectionFinished(LogicValueField selectedValue)
            {
                LogicValueField selectedInput = selectedValue;
                LogicValueField selectedOutput = StateMachine.selectedOutput;

                if (!(selectedInput.Input.HasLogicOutputSource &&
                selectedInput.Input.GetSourceLogicOutputComponent() == selectedOutput.onComponent &&
                selectedInput.Input.GetSourceLogicOutput() == selectedOutput.Output))
                {
                    Undo.RecordObject(selectedInput.onComponent,
                    $"Assigning input \"{selectedInput.valueName}\" on {selectedInput.onComponent.name} " +
                    $"to reference output \"{selectedOutput.valueName}\" on {selectedOutput.onComponent.name}");

                    selectedInput.Input.SetFieldReference(selectedOutput.onComponent, selectedOutput.fieldInfo.Name);
                    selectedInput.fieldInfo.SetValue(selectedInput.onComponent, selectedInput.value.GetClone());

                    EditorUtility.SetDirty(selectedInput.onComponent);
                }

                StateMachine.selectedOutput = null;
                StateMachine.ChangeState(new StartingState());
            }
        }
        public class RemovingLinkState : SelectingHandleState, RefreshableState
        {
            public override Func<LogicValueField, bool> GetValueFilter() => (v => v.IsInput && v.Input.HasLogicOutputSource);
            protected override string GetSelectorWindowTitle() => "Select input to unlink";

            public override void OnUpdate()
            {
                Color oldColor = Handles.color;
                Handles.color = Color.red;
                base.OnUpdate();
                Handles.color = oldColor;
            }
            protected override void OnSelectionFinished(LogicValueField selectedValue)
            {
                LogicValueField selectedInput = selectedValue;

                if (selectedInput.Input.HasLogicOutputSource)
                {
                    Undo.RecordObject(selectedInput.onComponent,
                    $"Clearing output source from input \"{selectedInput.valueName}\" on {selectedInput.onComponent.name} ");

                    selectedInput.Input.SetFieldReference(null, "");
                    selectedInput.fieldInfo.SetValue(selectedInput.onComponent, selectedInput.value.GetClone());

                    EditorUtility.SetDirty(selectedInput.onComponent);
                }
                StateMachine.ChangeState(new StartingState());
            }
        }
        public abstract class SelectingHandleState : ValueLinkState
        {
            protected abstract string GetSelectorWindowTitle();

            public override void OnEnter(State<ValueLinkMachine> lastState) 
            {
                base.OnEnter(lastState);

                if (lastState is WindowValueSelectState windowState && windowState.comingFromState == this)
                {
                    OnSelectionFinished(windowState.GetSelectedValue());
                }
                else
                {
                    StateMachine.RefreshHandlesAndLinks();
                }
            }

            public override void OnUpdate() 
            {
                StateMachine.DrawLinksStandard();
                Handle pressedHandle = StateMachine.DrawHandlesAndGetPressedStandard();
                if (pressedHandle != null)
                {
                    if (pressedHandle.filteredValues.Length == 1)
                    {
                        OnSelectionFinished(pressedHandle.filteredValues[0]);
                    }
                    else if (pressedHandle.filteredValues.Length > 1)
                    {
                        StateMachine.mousePosition = pressedHandle.handleTarget.position;
                        StateMachine.ChangeState(new WindowValueSelectState(
                            GetSelectorWindowTitle(), 
                            pressedHandle.filteredValues, 
                            pressedHandle.handleTarget));
                    }
                }
            }

            protected abstract void OnSelectionFinished(LogicValueField selectedValue);
        }
        public class WindowValueSelectState : ValueLinkState
        {
            public override Func<LogicValueField, bool> GetValueFilter() => comingFromState.GetValueFilter();
            
            public ValueLinkState comingFromState;
            public SelectorWindow<LogicValueField> window;
            public Transform windowAttachedTo;

            public WindowValueSelectState(string title, LogicValueField[] valuesToSelect, Transform windowAttachedTo)
            {
                window = new SelectorWindow<LogicValueField>(title, valuesToSelect, ValueName);
                this.windowAttachedTo = windowAttachedTo;
            }

            public override void OnEnter(State<ValueLinkMachine> lastState)
            {
                if (lastState == null)
                {
                    throw new Exception("LogicValueLinkTool: Entering Window Value Select State when previous state is null?");
                }

                comingFromState = lastState as ValueLinkState;
                base.OnEnter(lastState);
            }

            public override void OnUpdate()
            {
                window.SetWindowPosition(windowAttachedTo.position);

                Color oldColor = Handles.color;
                if (comingFromState is RemovingLinkState)
                    Handles.color = Color.red;

                StateMachine.DrawLinksStandard();
                StateMachine.DrawHandlesAndGetPressedStandard();

                if (comingFromState is SelectingInputState)
                {
                    StateMachine.DrawLinkFromOutputToMouse();
                }

                if (window.GetIfOptionSelected())
                {
                    StateMachine.ChangeState(comingFromState);
                }

                Handles.color = oldColor;
            }

            public LogicValueField GetSelectedValue() => window.GetOption() as LogicValueField;

            private string ValueName(LogicValueField valueField) =>
                $"<size=10>{valueField.onComponent.name}</size>: <b>{valueField.valueName}</b>";

        }

    }
}



