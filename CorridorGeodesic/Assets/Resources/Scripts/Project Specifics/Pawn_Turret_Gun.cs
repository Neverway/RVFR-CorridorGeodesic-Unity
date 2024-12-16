//===================== (Neverway 2024) Written by Connorses =====================
//
// Purpose: Controls the gun portion of the turrets
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neverway.Framework.PawnManagement;

public class Pawn_Turret_Gun : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=

    [SerializeField] private float fireDelay=0.2f;
    //[SerializeField] private float time=0;
    [SerializeField] private float damage = -1f;
    [SerializeField] private GameObject flash;
    [SerializeField] private LayerMask mask;

    [SerializeField] private Transform gunBarrel;
    private AudioSource audioSource;
    [SerializeField] private float maxSpinSpeed = 720f;
    private float timeSinceFired = 0f;

    [SerializeField] private float spottedDelay = 0.2f;
    private float hitPawnTime = 0f;
    private bool hitPawn = false;
    [SerializeField] AudioClip warningSound;

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
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (fireDelay <= 0)
        {
            Debug.LogError ("FireDelay is too low which causes an infinite loop. Returning.");
            return;
        }

        if (RaycastCheckForPawn ())
        {
            if (hitPawn == false)
            {
                PlayWarningSound ();
            }
            hitPawn = true;
        }
        else
        {
            hitPawn = false;
        }

        if (hitPawn)
        {
            hitPawnTime += Time.deltaTime;
        }
        else
        {
            hitPawnTime = 0f;
        }


        if (timeSinceFired < fireDelay)
        {
            gunBarrel.Rotate (maxSpinSpeed * Time.deltaTime, 0, 0);
        }
        timeSinceFired += Time.deltaTime;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=

    private bool RaycastCheckForPawn ()
    {
        RaycastHit hit;
        if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.TryGetComponent<Pawn> (out var pawn))
            {
                if (timeSinceFired > fireDelay && hitPawnTime > spottedDelay)
                {
                    pawn.ModifyHealth (damage);
                    flash.SetActive (true);
                    StartCoroutine (FlashOffWorker ());
                    timeSinceFired = 0f;
                    audioSource.Stop ();
                    audioSource.Play ();
                }
                return true;
            }
        }
        return false;
    }

    private IEnumerator FlashOffWorker ()
    {
        yield return new WaitForSeconds (0.05f);
        flash.SetActive (false);
    }

    private void PlayWarningSound ()
    {
        audioSource.PlayOneShot (warningSound);
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}