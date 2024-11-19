using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Neverway.Framework.LogicSystem;

//[RequireComponent(typeof(Mesh_Slicable))]
//[RequireComponent(typeof(SlicedPartsReference))]
//[RequireComponent(typeof(Volume_TriggerEvent))]
public class SlicableTriggerVolume : LogicComponent
{
    public Transform targetObject;

    [DebugReadOnly] public Mesh_Slicable meshSlicable;
    [DebugReadOnly] public SlicedPartsReference partsReference;
    [DebugReadOnly] public Volume_TriggerEvent volumeTrigger;

    public void OnValidate()
    {
        if (targetObject == null)
            return;

        meshSlicable = targetObject.GetComponent<Mesh_Slicable>();
        partsReference = targetObject.GetComponent<SlicedPartsReference>();
        volumeTrigger = targetObject.GetComponent<Volume_TriggerEvent>();
    }

    public new void Awake()
    {
        base.Awake();
        transform.SetParent(null);
    }

    public void Update()
    {
        bool shouldBePowered = false;
        List<Mesh_Slicable> slicables = partsReference.GetMeshes;
        foreach(Mesh_Slicable slicable in slicables)
        {
            Debug.Log("RAH Checking " + slicable.name);
            shouldBePowered = shouldBePowered || slicable.GetComponent<Volume_TriggerEvent>().isPowered;
        }
        isPowered = shouldBePowered;
    }
}
