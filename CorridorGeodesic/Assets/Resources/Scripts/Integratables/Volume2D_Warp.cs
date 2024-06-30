//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;

public class Volume2D_Warp : Volume2D
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string warpID;
    public string exitWarpID;
    public float exitOffsetX, exitOffsetY;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private Transform exitTransform;
    private Vector3 exitOffset;


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private new void OnTriggerEnter2D(Collider2D _other)
    {
        base.OnTriggerEnter2D(_other); // Call the base class method
        if (!GetExitWarp()) return; 
        if (_other.CompareTag("Pawn"))
        {
            targetEnt.transform.position = GetExitWarp().position+exitOffset;
            if (targetEnt.isPossessed) { // TODO Add player screen fade here
            }
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
            targetProp.AssociatedGameObject.transform.position = GetExitWarp().position+exitOffset;
        }
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private Transform GetExitWarp()
    {
        exitOffset = new Vector3(exitOffsetX, exitOffsetY);
        
        if (exitWarpID == "") return null;
        
        foreach (var warp in FindObjectsOfType<Volume2D_Warp>())
        {
            if (warp.warpID == exitWarpID)
            {
                return warp.transform;
            }
        }

        return null;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
