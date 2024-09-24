//===================== (Neverway 2024) Written by Connorses =====================
//
// Purpose: Provides crush detection for pawns
// Notes: can be repurposed for other objects
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
    [SerializeField] private float crushDamageAmount = 40f;
    private int checkFrequency = 4;
    private int frameCount = 0;
    private Pawn pawn;

    public UnityEvent onCrushed {  get; private set; } = new UnityEvent();

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
        //skip detection if objects are frozen to calculate mesh colliders. (avoids false positives)
        if (Alt_Item_Geodesic_Utility_GeoGun.delayRiftCollapse)
        {
            return;
        }
        frameCount++;
        if (frameCount >= checkFrequency)
        {
            frameCount = 0;
            if (CheckForOverlaps ())
            {
                if (pawn)
                {
                    Debug.Log ("! Crush Damage !");
                    onCrushed?.Invoke ();
                    pawn.ModifyHealth (-crushDamageAmount);
                }
            }
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=

    private bool CheckForOverlaps ()
    {
        Collider[] colliders = Physics.OverlapCapsule (transform.position + transform.up * rayDistance.y, transform.position - transform.up * downDistance, rayDistance.x);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == gameObject)
            {
                continue;
            }
            if (collider.isTrigger)
            {
                continue;
            }
            if (collider.attachedRigidbody != null && !collider.attachedRigidbody.isKinematic) {
                continue;
            }
            return true;
        }
        return false;
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}