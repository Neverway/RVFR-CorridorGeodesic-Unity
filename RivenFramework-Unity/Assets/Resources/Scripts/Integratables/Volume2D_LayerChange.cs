//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes: Layer mask ids: 1-6, 2-7, 3-8
//
//=============================================================================

using UnityEngine;

public class Volume2D_LayerChange : Volume
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("Which layer should the entity switch to when entering the trigger. P.S DON'T FORGET TO SWITCH THE TRIGGERS LAYER!!")]
    [Range(0, 2)] public int exitLayer;
    public float fallTime;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private new void OnTriggerEnter2D(Collider2D _other)
    {
        base.OnTriggerEnter2D(_other); // Call the base class method
        if (_other.CompareTag("Pawn"))
        {
            SetTargetDepth(targetEnt.GetComponent<Object_DepthAssigner2D>());
        }
        if (_other.CompareTag("PhysProp") && targetProp)
        {
            if (targetProp.AssociatedGameObject.GetComponent<Object_Grabbable>())
            {
                if (targetProp.AssociatedGameObject.GetComponent<Object_Grabbable>().isHeld)
                {
                    return;
                }
            }
            SetTargetDepth(targetProp.AssociatedGameObject.GetComponent<Object_DepthAssigner2D>());
        }
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void SetTargetDepth(Object_DepthAssigner2D _targetObject)
    {
        _targetObject.depthLayer = exitLayer;
        if (fallTime <= 0) return;
        _targetObject.fallTime = fallTime;
        _targetObject.Fall();
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    
}
