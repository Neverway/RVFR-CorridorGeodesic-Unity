//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using UnityEngine;
using Neverway.Framework;
using Neverway.Framework.PawnManagement;

public class Enemy_Chaser : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=

    [SerializeField] private float tickRate;
    private float tickTimer;
    [SerializeField] private LayerMask rayLayerMask;
    private Transform target;

    private Vector3 targetPos;
    [SerializeField] private float targetStopDistance = 1f;

    [SerializeField] private bool moving = false;
    [SerializeField] private bool lostPlayer = false;
    [SerializeField] private float moveForce = 6f;

    [SerializeField] private float moveDelay = 0.5f;

    //todo: make the target a no-crush actor data so it moves with rifts.
    //todo: detect getting stuck and set moving to false
    //todo: detect going a very long time without seeing someone and set moving to false


    //=-----------------=
    // Reference Variables
    //=-----------------=

    private Rigidbody rb;
    private CameraManager cameraManager;

    //=-----------------=
    // Mono Functions
    //=-----------------=

    private IEnumerator Start ()
    {
        rb = GetComponent<Rigidbody> ();

        Pawn pawn = null;
        while (pawn == null)
        {
            if (Time.timeSinceLevelLoad > 3f)
            {
                Debug.LogError ("Took too long to find a Pawn object in the scene.");
                Destroy (this);
                yield break;
            }
            pawn = FindObjectOfType<Pawn> ();
            yield return null;
        }

        if (pawn == null)
        {
            Debug.LogError ("Couldn't find Pawn object in scene.");
            Destroy (this);
            yield break;
        }
        target = pawn.transform;
    }

    private void Update ()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer > tickRate)
        {
            tickTimer = 0;
            OnTick ();
        }
    }

    private void FixedUpdate ()
    {
        if (moving)
        {
            SearchForPlayer ();
            if (Vector3.Distance (rb.position, targetPos) < targetStopDistance)
            {
                moving = false;
                return;
            }
            rb.AddForce ((targetPos - rb.position).normalized * moveForce, ForceMode.Acceleration);
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void OnTick ()
    {
        //todo:
        //  raycast at player pawn
        //  move toward last known location
        //  stop at ledges?
        
        if (target == null)
        {
            return;
        }

        SearchForPlayer ();
    }

    private void SearchForPlayer ()
    {
        if (Physics.Raycast (transform.position, target.position - transform.position, out RaycastHit hitInfo, 100f, rayLayerMask))
        {
            if (hitInfo.collider.gameObject == target.gameObject)
            {
                if (lostPlayer) //if the player was out of sight and found again, delay the retargeting.
                {
                    moving = false;
                    lostPlayer = false;
                }
                targetPos = target.position;
                targetPos.y = transform.position.y;
                StartCoroutine (DelayedMove ());
            }
            else //if we don't see the player
            {
                if (moving)
                {
                    lostPlayer = true;
                }
            }
        }
    }

    private IEnumerator DelayedMove ()
    {
        yield return new WaitForSeconds (moveDelay);
        moving = true;
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}