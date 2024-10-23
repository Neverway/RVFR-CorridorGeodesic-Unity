using Neverway.Framework.LogicValueSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.ShortcutManagement;
using UnityEngine;

[EditorTool("Logic Value Link Tool")]
public class LogicValueLinkTool : EditorTool
{
    #region EditorTool Setup
    public static Tool lastToolType;
    [Shortcut("Use Logic Value Link Tool", typeof(SceneView), KeyCode.L, ShortcutModifiers.Alt)]
    public static void SetAsCurrentTool()
    {
        if (lastToolType != Tool.None)
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
   
    // Define the icon displayed in the toolbar
    public static GUIContent m_Icon;
    public override GUIContent toolbarIcon
    {
        get 
        {
            if (m_Icon == null)
                m_Icon =  new GUIContent()
                {
                    image = EditorIcons.SettingsIcon,
                    text = "Logic Value Link Tool",
                    tooltip = "Logic Value Link Tool (Alt + L)"
                };
            return m_Icon;
        }
    }
    #endregion

    private const float SPHERE_HANDLE_SIZE = 0.3f;

    private LogicComponent firstLogicComponent = null; // Track the first object (where dragging starts)
    private Vector3 startHandlePosition;
    private bool isDragging = false;
    private int previousSceneObjectsCount = -1;
    private LogicComponent[] logicComponents;

    private static Transform sceneViewCameraTransform;
    private LogicInput[] inputs;
    private LogicOutput[] outputs;
    private ValueLink[] links;
    private Dictionary<Component, LogicValue[]> logicValues;
    private Component[] componentsWithLogicValues;
    private static Dictionary<Type, FieldInfo[]> logicValueFieldInfos;
    private Type[] typesWithLogicValues;
    private Component[] allComponents;

    private LinkerToolPopupWindow activeWindow;


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
            Handles.DrawLine(start.position, end.position, thickness);
            Handles.color = old;
        }
    }

    private bool CacheFieldInfos()
    {
        if (logicValueFieldInfos != null)
            return false;

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
        return true;
    }
    private void CacheLogicValues()
    {
        bool doRecalculate = CacheFieldInfos();

        if (!doRecalculate)
        {
            foreach (Component c in componentsWithLogicValues)
                doRecalculate |= c == null;

            if (doRecalculate)
                allComponents = FindObjectsOfType<Component>();
        }

        if (!doRecalculate)
            return;

        List<Component> componentsWithLogicValuesList = new List<Component>();
        logicValues = new Dictionary<Component, LogicValue[]>();
        List<LogicInput> inputsList = new List<LogicInput>();
        List<LogicOutput> outputsList = new List<LogicOutput>();
        List<ValueLink> linksList = new List<ValueLink>();
        foreach (Component currentComponent in allComponents)
        {
            Type currentComponentType = currentComponent.GetType();
            if (typesWithLogicValues.Contains(currentComponentType))
            {
                componentsWithLogicValuesList.Add(currentComponent);
                List<LogicValue> logicValueList = new List<LogicValue>();
                foreach (FieldInfo field in logicValueFieldInfos[currentComponentType])
                {
                    LogicValue value = field.GetValue(currentComponent) as LogicValue;
                    logicValueList.Add(value);

                    if (value is LogicOutput output)
                    {
                        outputsList.Add(output);
                    }
                    else if (value is LogicInput input)
                    {
                        inputsList.Add(input);
                        if (input.HasLogicOutputSource)
                        {
                            LogicValue otherValue = input.GetSourceLogicOutput(); 

                            Transform startHandle = otherValue.GetHandleTarget();
                            Transform endHandle = value.GetHandleTarget();
                            Debug.Log(startHandle + " | " + endHandle);

                            if (startHandle != null && endHandle != null)
                                linksList.Add(new ValueLink(value.GetLogicValueType(), startHandle, endHandle));
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
    }

    public override void OnToolGUI(EditorWindow window)
    {
        if (!(window is SceneView)) return;
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;

        if (sceneViewCameraTransform == null)
            sceneViewCameraTransform = SceneView.lastActiveSceneView.camera.transform;
        CacheLogicValues();

        foreach (Component c in componentsWithLogicValues)
        {
            if (Handles.Button(c.transform.position, Quaternion.identity, SPHERE_HANDLE_SIZE * 0.7f, SPHERE_HANDLE_SIZE, Handles.SphereHandleCap))
            {
                Debug.ClearDeveloperConsole(); 
                foreach(FieldInfo field in logicValueFieldInfos[c.GetType()])
                {
                    LogicValue val = field.GetValue(c) as LogicValue;
                    Debug.Log(field.FieldType.Generic(0).HumanName(true) + (val is LogicOutput ? " Output: " : " Input: ") + field.Name);
                }
            }
        }
        Debug.Log(links.Length);
        foreach(ValueLink link in links)
        {
            link.Draw(Color.white, 1f);
        }

        SceneView.RepaintAll();
        return;
        //Caching the Logic Components
        //if (logicComponents == null)
        //{
        logicComponents = FindObjectsOfType<LogicComponent>();
        //}

        if (isDragging && firstLogicComponent == null)
            isDragging = false;

        if (activeWindow != null)
        {
            activeWindow.RenderWindow();

            if (Event.current.type == EventType.MouseDown && !activeWindow.windowRect.Contains(Event.current.mousePosition))
                activeWindow.readyToDestroy = true;

            if (activeWindow.readyToDestroy)
                activeWindow = null;
        }
        else
        {
            foreach (LogicComponent logicComponent in logicComponents)
            {
                if (logicComponent == null) continue;

                LogicComponentHandleInfo[] infos = LogicComponentHandleInfo.GetFromType(logicComponent.GetType());

                if (infos.Length > 1)
                    style.normal.textColor = Color.yellow;
                else if (infos.Length > 0)
                    style.normal.textColor = Color.green;
                else
                    style.normal.textColor = Color.red;

                Handles.Label(logicComponent.transform.position + Vector3.up * 0.5f, logicComponent.name, style);

                if (Event.current.shift)
                {
                    HandleClearHandle(logicComponent, infos);
                    isDragging = false;
                }
                else if (isDragging)
                {
                    HandleInputHandle(logicComponent, infos);
                    //foreach (LogicComponentHandleInfo info in infos)
                    //    HandleInputHandle(logicComponent, info);
                }
                else
                {
                    HandleOutputHandle(logicComponent);
                }
            }
        }

        // If dragging and not releasing over a handle, draw a line to the current mouse position
        if (isDragging && firstLogicComponent != null)
        {
            Vector3 mousePosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
            Handles.DrawLine(startHandlePosition, mousePosition, 8f);
        }

        SceneView.RepaintAll(); // Ensure SceneView is updated during dragging
    }
    private void HandleClearHandle(LogicComponent logicComponent, LogicComponentHandleInfo[] infos)
    {
        if (infos.Length <= 0)
            return;

        // Position of the draggable handle (sphere)
        Handles.color = Color.red;
        Vector3 handlePosition =
            logicComponent.transform.position;

        // Handle clicking the sphere to start dragging
        if (Handles.Button(handlePosition, Quaternion.identity, SPHERE_HANDLE_SIZE * 0.7f, SPHERE_HANDLE_SIZE, Handles.SphereHandleCap))
        {
            if (infos.Length > 1)
                activeWindow = new LinkerToolPopupWindow(logicComponent, firstLogicComponent, infos, true);
            else if (infos.Length > 0)
            {
                Undo.RecordObject(logicComponent, "Clearing any LogicComponent inputs on " + logicComponent.name);
                infos[0].TryClear(logicComponent);
                EditorUtility.SetDirty(logicComponent);
            }

            //foreach (LogicComponentHandleInfo info in infos)
            //{
            //    Undo.RecordObject(logicComponent, "Clearing any LogicComponent inputs on " + logicComponent.name);
            //    info.TryClear(logicComponent);
            //    EditorUtility.SetDirty(logicComponent);
            //}
        }
    }
    private void HandleOutputHandle(LogicComponent logicComponent)
    {
        // Position of the draggable handle (sphere)
        Handles.color = (logicComponent.isPowered ? Color.cyan : new Color(0.3f, 0.3f, 1f));
        Vector3 handlePosition =
            logicComponent.transform.position;

        // Handle clicking the sphere to start dragging
        if (Handles.Button(handlePosition, Quaternion.identity, SPHERE_HANDLE_SIZE * 0.7f, SPHERE_HANDLE_SIZE, Handles.SphereHandleCap))
        {
            // Start dragging from this object
            firstLogicComponent = logicComponent;
            startHandlePosition = handlePosition;
            isDragging = true;
        }
    }

    private void HandleInputHandle(LogicComponent logicComponent, LogicComponentHandleInfo[] infos)
    {
        if (logicComponent == firstLogicComponent)
            return;

        // Position of the draggable handle (sphere)
        Handles.color = Color.green;
        Vector3 handlePosition = logicComponent.transform.position;
        //Vector3 handlePosition =
        //    logicComponent.transform.TransformPoint(
        //    new Vector3(info.attribute.xOffset, info.attribute.yOffset, info.attribute.zOffset));

        // Handle clicking the sphere to start dragging
        if (Handles.Button(handlePosition, Quaternion.identity, SPHERE_HANDLE_SIZE * 0.7f, SPHERE_HANDLE_SIZE, Handles.SphereHandleCap))
        {
            //Tring to check for infinite loops
            /*recursionInfiniteLoopProtection = 0;
            //if (IsInfiniteLoop(firstLogicComponent, logicComponent))
            //{
            //    Debug.LogWarning("That connection would cause an infinite loop silly!");
            //}
            //else
            //{
            //    // Try to assign the first LogicComponent to the next LogicComponent
            //    info.TryAssign(logicComponent, firstLogicComponent);
            //}*/

            // Try to assign the first LogicComponent to the next LogicComponent

            if (infos.Length > 1)
                activeWindow = new LinkerToolPopupWindow(logicComponent, firstLogicComponent, infos);
            else if (infos.Length > 0)
            {
                Undo.RecordObject(logicComponent, "Link LogicComponent Input from " + firstLogicComponent.name + " to " + logicComponent.name);
                infos[0].TryAssign(logicComponent, firstLogicComponent);
                EditorUtility.SetDirty(logicComponent);
            }

            isDragging = false;
        }
    }
    private int recursionInfiniteLoopProtection;
    private bool IsInfiniteLoop(LogicComponent startingComponent, LogicComponent currentComponent)
    {
        if (currentComponent == null)
            return false;

        if (recursionInfiniteLoopProtection++ > 5000)
        {
            Debug.LogError("You either got 5000 LogicComponents linked together and I stopped whatever madness you were trying to do, " +
                "or you have an infinite loop somewhere in your LogicComponents. (Or I possibly programmed this wrong)");
            return true;
        }

        if (startingComponent == currentComponent)
            return true;

        LogicComponentHandleInfo[] infos = LogicComponentHandleInfo.GetFromType(currentComponent.GetType());
        foreach (LogicComponentHandleInfo info in infos)
        {
            if (typeof(LogicComponent).IsAssignableFrom(info.field.FieldType))
            {
                LogicComponent linkedComponent = (LogicComponent)info.field.GetValue(currentComponent);
                if (IsInfiniteLoop(startingComponent, linkedComponent))
                    return true;
            }
            else if (typeof(List<LogicComponent>).IsAssignableFrom(info.field.FieldType))
            {
                List<LogicComponent> linkedComponentList = (List<LogicComponent>)info.field.GetValue(currentComponent);
                if (linkedComponentList == null)
                    return false;
                foreach (LogicComponent linkedComponent in linkedComponentList)
                {
                    if (IsInfiniteLoop(startingComponent, linkedComponent))
                        return true;
                }
            }
            else
            {
                Debug.LogWarning("Infinite loop check for LogicComponent chains was not defined for this type. Allowing link for now");
                return false;
            }
        }
        return false;
    }


    public class LinkerToolPopupWindow
    {
        public bool readyToDestroy;

        private bool clearing;

        public Rect windowRect;

        private GUID windowID;

        private LogicComponent logicComponent;
        private LogicComponent firstLogicComponent;
        private List<LogicComponentHandleInfo> infos = new List<LogicComponentHandleInfo>();

        public LinkerToolPopupWindow(LogicComponent logicComponent, LogicComponent firstLogicComponent, LogicComponentHandleInfo[] infos, bool clearing = false)
        {
            this.logicComponent = logicComponent;
            this.firstLogicComponent = firstLogicComponent;
            this.infos.AddRange(infos);
            this.clearing = clearing;

            windowRect.size = new Vector2(200, infos.Length * 20);
            windowRect.center = Event.current.mousePosition;

            windowID = new GUID();
        }
        public void RenderWindow()
        {
            GUILayout.Window(windowID.GetHashCode(), windowRect, LayoutWindow, "Which Signal?");
        }
        public void LayoutWindow(int window)
        {
            bool pressedButton = false;

            for (int i = 0; i < infos.Count; i++)
            {
                if (GUILayout.Button(infos[i].field.Name))
                {
                    pressedButton = true;
                    if (!clearing)
                        AssignLogicComponent(infos[i]);
                    else
                        UnAssignLogicComponent(infos[i]);
                }
            }

            if (pressedButton)
                readyToDestroy = true;
        }
        private void AssignLogicComponent(LogicComponentHandleInfo info)
        {
            Undo.RecordObject(logicComponent, "Link LogicComponent Input from " + firstLogicComponent.name + " to " + logicComponent.name);
            info.TryAssign(logicComponent, firstLogicComponent);
            EditorUtility.SetDirty(logicComponent);
        }
        private void UnAssignLogicComponent(LogicComponentHandleInfo info)
        {
            Undo.RecordObject(logicComponent, "Clearing any LogicComponent inputs on " + logicComponent.name);
            info.TryClear(logicComponent);
            EditorUtility.SetDirty(logicComponent);
        }
    }
}
