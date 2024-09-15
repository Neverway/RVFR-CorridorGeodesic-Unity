using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEditor;
using UnityEngine;
using JetBrains.Annotations;
using System.Reflection;
using System.Linq;


//Made by Errynei


#if UNITY_EDITOR
[EditorTool("Logic Component Link Tool")]
public class LogicLinkerEditorTool : EditorTool
{
    private const float SPHERE_HANDLE_SIZE = 0.3f;


    private GUIContent _iconContent;
    private LogicComponent firstLogicComponent = null; // Track the first object (where dragging starts)
    private Vector3 startHandlePosition;
    private bool isDragging = false;
    private int previousSceneObjectsCount = -1;
    private LogicComponent[] logicComponents;

    private void OnEnable()
    {
        _iconContent = new GUIContent(EditorGUIUtility.IconContent("AvatarInspector/DotSelection").image, "Link Tool");
        previousSceneObjectsCount = -1;
        logicComponents = null;
    }

    public override GUIContent toolbarIcon => _iconContent;

    public override void OnToolGUI(EditorWindow window)
    {
        if (!(window is SceneView)) return;

        //Caching the Logic Components
        //if (logicComponents == null)
        //{
            logicComponents = FindObjectsOfType<LogicComponent>();
        //}

        if (isDragging && firstLogicComponent == null)
            isDragging = false;

        foreach (LogicComponent logicComponent in logicComponents)
        {
            if (logicComponent == null) continue;

            if (Event.current.shift)
            {
                HandleClearHandle(logicComponent);
                isDragging = false;
            }
            else if (isDragging)
            {
                LogicComponentHandleInfo[] infos = LogicComponentHandleInfo.GetFromType(logicComponent.GetType());
                foreach (LogicComponentHandleInfo info in infos)
                    HandleInputHandle(logicComponent, info);
            }
            else
            {
                HandleOutputHandle(logicComponent);
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
    private void HandleClearHandle(LogicComponent logicComponent)
    {
        // Position of the draggable handle (sphere)
        Handles.color = Color.red;
        Vector3 handlePosition =
            logicComponent.transform.position;

        // Handle clicking the sphere to start dragging
        if (Handles.Button(handlePosition, Quaternion.identity, SPHERE_HANDLE_SIZE * 0.7f, SPHERE_HANDLE_SIZE, Handles.SphereHandleCap))
        {
            LogicComponentHandleInfo[] infos = LogicComponentHandleInfo.GetFromType(logicComponent.GetType());
            foreach (LogicComponentHandleInfo info in infos)
            {
                info.TryClear(logicComponent);
            }
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

    private void HandleInputHandle(LogicComponent logicComponent, LogicComponentHandleInfo info)
    {
        if (logicComponent == firstLogicComponent)
            return;

        // Position of the draggable handle (sphere)
        Handles.color = Color.green;
        Vector3 handlePosition =
            logicComponent.transform.TransformPoint(
            new Vector3(info.attribute.xOffset, info.attribute.yOffset, info.attribute.zOffset));

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
            info.TryAssign(logicComponent, firstLogicComponent);

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
                foreach(LogicComponent linkedComponent in linkedComponentList)
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
}
#endif