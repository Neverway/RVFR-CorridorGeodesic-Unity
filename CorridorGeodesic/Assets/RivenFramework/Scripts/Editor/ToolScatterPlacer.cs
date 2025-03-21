//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.PackageManager;
using UnityEngine;
using Random = UnityEngine.Random;

[EditorTool("Scatter Placer Tool")]
public class ToolScatterPlacer : EditorTool
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public GameObject targetAsset;
    public Vector3 localPositionOffset;
    public Vector2 scaleRange;
    public Vector2 rotationYRange;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private ToolScatterPlacerUI targetEditorWindow;


    //=-----------------=
    // Mono Functions
    //=-----------------= 
    public void OnEnable()
    {
        targetEditorWindow = EditorWindow.GetWindow<ToolScatterPlacerUI>();
    }

    public override void OnToolGUI(EditorWindow window)
    {
        if (!(window is SceneView sceneView)) return;
        
        if (!targetEditorWindow)
        {
            Debug.LogWarning("Couldn't located Scatter Placer UI, did you open the window yet? (If not, go to the top bar 'Neverway/FScatterPlacerTool')");
            return;
        }
        
        var e = Event.current;
        
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            targetAsset = targetEditorWindow.targetAsset;
            localPositionOffset = targetEditorWindow.localPositionOffset;
            scaleRange = targetEditorWindow.scaleRange;
            rotationYRange = targetEditorWindow.rotationYRange;
            
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (!targetAsset) return;
                
                // Create asset
                var placedAsset = Instantiate(targetAsset, hit.point, new Quaternion());
                Undo.RegisterCreatedObjectUndo(placedAsset, "Place Asset");
                
                // Apply rotations
                var normalRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                var randomYRotation = Random.Range(rotationYRange.x, rotationYRange.y);
                var randomRotation = Quaternion.Euler(0, randomYRotation, 0);
                placedAsset.transform.rotation = normalRotation * randomRotation;

                // Apply position
                placedAsset.transform.position += placedAsset.transform.right*localPositionOffset.x;
                placedAsset.transform.position += placedAsset.transform.up*localPositionOffset.y;
                placedAsset.transform.position += placedAsset.transform.forward*localPositionOffset.z;

                // Apply scale
                var randomScale = Random.Range(scaleRange.x, scaleRange.y);
                var fixedRandomScale = Mathf.Round(randomScale * 100) / 100;
                Debug.Log(randomScale);
                Debug.Log(fixedRandomScale);
                placedAsset.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                
                // Eat the input so the scene view doesn't use it to select something
                e.Use();
            }
        }
        SceneView.RepaintAll(); // Ensure SceneView is updated during dragging
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}

public class ToolScatterPlacerUI : EditorWindow
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public GameObject targetAsset;
    public Vector3 localPositionOffset = new Vector3(0, 0, 0);
    public Vector2 scaleRange = new Vector2(0.5f, 2f);
    public Vector2 rotationYRange = new Vector2(0, 360);


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    [MenuItem("Neverway/ScatterPlacerTool")]
    public static void ShowWindow()
    {
        GetWindow<ToolScatterPlacerUI>("Scatter Placer");
    }
    
    public void OnGUI()
    {
        targetAsset = EditorGUILayout.ObjectField("", targetAsset, typeof(UnityEngine.GameObject)) as GameObject;
        localPositionOffset = EditorGUILayout.Vector3Field("Position Offset", localPositionOffset);
        scaleRange = EditorGUILayout.Vector2Field("Scale Range", scaleRange);
        rotationYRange = EditorGUILayout.Vector2Field("Y-axis Rotation Range", rotationYRange);
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}