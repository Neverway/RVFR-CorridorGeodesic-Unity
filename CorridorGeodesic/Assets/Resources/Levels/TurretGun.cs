//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretGun : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=

    [SerializeField] private float fireDelay=0.2f;
    [SerializeField] private float time=0;
    [SerializeField] private float damage = -1f;
    [SerializeField] private GameObject flash;
    [SerializeField] private LayerMask mask;

    [SerializeField] private Transform gunBarrel;
    private float maxSpin = 720f;
    private float timeSinceFired = 0f;

    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
    
    }

    private void Update()
    {
        if (fireDelay <= 0)
        {
            Debug.LogError ("FireDelay is too low which causes an infinite loop. Returning.");
            return;
        }
        time += Time.deltaTime;
        while (time > fireDelay)
        {
            time -= fireDelay;
            DoRaycast ();
        }

        if (timeSinceFired < fireDelay)
        {
            gunBarrel.Rotate (maxSpin * Time.deltaTime, 0, 0);
        }
        timeSinceFired += Time.deltaTime;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=

    private void DoRaycast ()
    {
        RaycastHit hit;
        if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.TryGetComponent<Pawn> (out var pawn))
            {
                pawn.ModifyHealth (damage);
                flash.SetActive (true);
                StartCoroutine (FlashOffWorker());
                timeSinceFired = 0f;
            }
        }
        
    }

    private IEnumerator FlashOffWorker ()
    {
        yield return new WaitForSeconds (0.05f);
        flash.SetActive (false);
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}