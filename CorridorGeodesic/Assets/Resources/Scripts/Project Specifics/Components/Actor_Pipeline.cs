//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor_Pipeline : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=

    [SerializeField] private Actor_Pipeline connectingTo = null;
    [SerializeField] private LayerMask layerMask;
    private RaycastHit hit;
    [SerializeField] private float rayDistance = 1f;
    private bool powered = false;
    [SerializeField] private bool powerSource = false;

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
        CheckForLine();
    }

    private void OnDisable ()
    {
        connectingTo = null;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    
    /// <summary>
    /// Checks if there's another pipeline object in front of this one.
    /// </summary>
    private void CheckForLine ()
    {
        bool hitPipe = false;
        if (Physics.Raycast (transform.position, transform.forward, out hit, rayDistance, layerMask))
        {
            if (hit.collider.gameObject.TryGetComponent<Actor_Pipeline> (out var _hitPipeline))
            {
                hitPipe = true;
                //todo: compare angles to see if connection is too sharp
                //todo: add variable for how sharp pipe can turn (vents can turn sharply, minecarts probably cant)
                //todo: allow for reverse connection for when an empty pipe is hooked up to active flow
                if (_hitPipeline != connectingTo)
                {
                    DisconnectPower ();
                }
                connectingTo = _hitPipeline;
                if (powered || powerSource)
                {
                    _hitPipeline.powered = true;
                }
            }
        }
        if (!hitPipe)
        {
            DisconnectPower ();
            connectingTo = null;
        }
    }

    private void DisconnectPower ()
    {
        if (connectingTo == null)
        {
            return;
        }
        connectingTo.powered = false;
        connectingTo.DisconnectPower ();
    }


    private void OnDrawGizmos ()
    {
        if (connectingTo == null)
        {
            return;
        }
        Debug.DrawLine(transform.position, connectingTo.transform.position, connectingTo.powered ? Color.red : Color.green);
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}