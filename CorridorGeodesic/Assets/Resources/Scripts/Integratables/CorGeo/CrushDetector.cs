//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;
using UnityEngine.Events;

public class CrushDetector : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=

    [SerializeField] private Vector3 rayDistance;
    [SerializeField] private float downDistance;
    [SerializeField] private LayerMask layerMask;
    private int checkFrequency = 4;
    private int frameCount = 0;
    private Pawn pawn;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=

    private void Start ()
    {
        pawn = GetComponent<Pawn>();
    }

    private void FixedUpdate ()
    {
        frameCount++;
        if (frameCount >= checkFrequency)
        {
            frameCount = 0;
            if (CheckOppositeRays ())
            {
                if (pawn)
                {
                    pawn.ModifyHealth (-55);
                }
            }
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=

    private bool CheckOppositeRays ()
    {
        if (Physics.Raycast (transform.position, transform.right, rayDistance.x, layerMask)
            && Physics.Raycast (transform.position, -transform.right, rayDistance.x, layerMask))
        {
            return true;
        }
        if (Physics.Raycast (transform.position, transform.forward, rayDistance.z, layerMask)
            && Physics.Raycast (transform.position, -transform.forward, rayDistance.z, layerMask))
        {
            return true;
        }
        if (Physics.Raycast (transform.position, transform.up, rayDistance.y, layerMask)
            && Physics.Raycast (transform.position, -transform.up, downDistance, layerMask))
        {
            return true;
        }
        return false;
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}