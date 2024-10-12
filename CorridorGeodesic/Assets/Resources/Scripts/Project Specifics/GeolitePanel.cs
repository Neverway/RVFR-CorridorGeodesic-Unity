using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GeolitePanel : LogicComponent
{
    [LogicComponentHandle] public LogicComponent regeneratePanel;


    private Mesh_Slicable[] sliceableChildren;
    public Mesh_Slicable panel;
    public CorGeo_ActorData[] boundingPositions;
    [field:SerializeField] public bool IsBroken { get; private set; }
    [field: SerializeField] public bool IsIntersectingNullSpace { get; private set; }

    public float lastStableLerpAmount = 0;
    public float lastRegenLerpAmount = 0;
    public float DEBUG_lerpAmount = 0;
    public float ActualLerpAmount => 
        (Alt_Item_Geodesic_Utility_GeoGun.deployedRift ? Alt_Item_Geodesic_Utility_GeoGun.lerpAmount : 0f);

    private List<MeshCollider> allPanelPartColliders;
    private List<MeshRenderer> allPanelPartRenderers;
    private MeshCollider panelCollider;
    private MeshRenderer panelRenderer;

    public static int count;

    public void Awake()
    {
        panel.gameObject.name = "GEOLITEPANEL" + count++;

        allPanelPartColliders = new List<MeshCollider>();
        allPanelPartRenderers = new List<MeshRenderer>();
        panelCollider = panel.GetComponent<MeshCollider>();
        panelRenderer = panel.GetComponent<MeshRenderer>();

        //Find and remove slicable meshes so they can use their local transform for proper slicing
        sliceableChildren = GetComponentsInChildren<Mesh_Slicable>();
        foreach (Mesh_Slicable sliceable in sliceableChildren)
            sliceable.transform.SetParent(null);

        if (IsBroken)
            BreakPanel();
        else
            RegeneratePanel();
    }

    private new void OnEnable()
    {
        base.OnEnable();
        boundingPositions[0].OnRiftRestore += CheckBreakOnRiftClose;
        boundingPositions[0].OnRiftRestore += ClearSlicedParts;
    }
    private void OnDisable()
    {
        boundingPositions[0].OnRiftRestore -= CheckBreakOnRiftClose;
        boundingPositions[0].OnRiftRestore -= ClearSlicedParts;
    }

    private void OnValidate()
    {
        if (panel == null)
            return;

        panelCollider = panel.GetComponent<MeshCollider>();
        panelRenderer = panel.GetComponent<MeshRenderer>();
        SetActiveForFullPanel(!IsBroken);
    }


    public void Update()
    {
        if (regeneratePanel != null && regeneratePanel.isPowered)
            RegeneratePanel();

        DEBUG_lerpAmount = ActualLerpAmount;
        IsIntersectingNullSpace = IsPanelIntersectingNullSpace();

        //if (!IsIntersectingNullSpace)
        //    lastStableLerpAmount = ActualLerpAmount;


        if (ShouldBreakPanel())
            BreakPanel();

        if (IsBroken)
        {
            SetActiveForFullPanel(false); //Have to do this every frame to avoid other scripts overriding it???
            SetActiveForSlicedParts(false);
        }
        else
        {
            bool showParts = allPanelPartColliders.Count > 0;
            SetActiveForFullPanel(!showParts); //Have to do this every frame to avoid other scripts overriding it???
            SetActiveForSlicedParts(showParts);
        }

    }

    [ContextMenu("Break Panel")]
    public void BreakPanel()
    {
        IsBroken = true;
        GetSlicedParts();
    }

    [ContextMenu("Regenerate Panel")]
    public void RegeneratePanel()
    {
        IsBroken = false;
        lastStableLerpAmount = ActualLerpAmount;
        lastRegenLerpAmount = ActualLerpAmount;
        GetSlicedParts();
    }

    private bool ShouldBreakPanel()
    {
        if (regeneratePanel != null && regeneratePanel.isPowered)
            return false;

        if (IsBroken)
            return false;

        if (IsIntersectingNullSpace)
            return lastStableLerpAmount != ActualLerpAmount;

        return false;
    }
    private void CheckBreakOnRiftClose()
    {
        if (lastStableLerpAmount != 0f)
            BreakPanel();

        lastStableLerpAmount = 0f;
    }
    private void ClearSlicedParts()
    {
        allPanelPartColliders.Clear();
        allPanelPartRenderers.Clear();
    }
    private void GetSlicedParts()
    {
        //todo: THIS IS FUCKING AWFUL BUT I TRIED MY BEST
        ClearSlicedParts();
        foreach (GameObject slicable in Alt_Item_Geodesic_Utility_GeoGun.slicedMeshes)
        {
            if (slicable.name.StartsWith("[CUT] " + panel.gameObject.name))
            {
                allPanelPartColliders.Add(slicable.gameObject.GetComponent<MeshCollider>());
                allPanelPartRenderers.Add(slicable.gameObject.GetComponent<MeshRenderer>());
            }
        }
        if (Alt_Item_Geodesic_Utility_GeoGun.planeBMeshes != null)
        {
            foreach (Transform slicable in Alt_Item_Geodesic_Utility_GeoGun.planeBMeshes.transform)
            {
                if (slicable.name.StartsWith("[CUT] " + panel.gameObject.name))
                {
                    allPanelPartColliders.Add(slicable.gameObject.GetComponent<MeshCollider>());
                    allPanelPartRenderers.Add(slicable.gameObject.GetComponent<MeshRenderer>());
                }
            }
        }
        if (Alt_Item_Geodesic_Utility_GeoGun.nullSlices != null)
        {
            foreach (GameObject slicable in Alt_Item_Geodesic_Utility_GeoGun.nullSlices)
            {
                if (slicable != null && slicable.name.StartsWith("[CUT] " + panel.gameObject.name))
                {
                    allPanelPartColliders.Add(slicable.gameObject.GetComponent<MeshCollider>());
                    allPanelPartRenderers.Add(slicable.gameObject.GetComponent<MeshRenderer>());
                }
            }
        }
    }
    private void SetActiveForSlicedParts(bool active)
    {
        foreach (MeshCollider c in allPanelPartColliders)
            c.enabled = active;

        foreach (MeshRenderer r in allPanelPartRenderers)
            r.enabled = active;
    }
    private void SetActiveForFullPanel(bool active)
    {
        if (active)
            panel.gameObject.SetActive(true);

        panelCollider.enabled = active;
        panelRenderer.enabled = active;
    }

    private bool IsPanelIntersectingNullSpace()
    {
        CorGeo_ActorData.Space currentSpace = boundingPositions[0].space;

        foreach (CorGeo_ActorData pos in boundingPositions) //todo: optimize this to not check 0th position twice
        {
            if (pos.space != currentSpace || pos.space == CorGeo_ActorData.Space.Null)
                return ActualLerpAmount != lastStableLerpAmount;
        }
        return false;
    }
}
