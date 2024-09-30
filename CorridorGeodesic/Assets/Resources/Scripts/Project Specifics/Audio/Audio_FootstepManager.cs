//===================== (Neverway 2024) Written by _____ =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_FootstepManager: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private LayerMask ignoreMask;
    private Vector3 currentPos;
    private Vector3 lastPos;

    private float _delta;
    private float delta
    {
        get { return _delta; }
        set
        {
            if (_delta < 0.1f && value >= 0.1f)
                StartFootsteps();
            if (_delta >= 0.1f && value < 0.1f)
                StopFootsteps();

            _delta = value;
        }
    }

    //=-----------------=
    // Reference Variables
    //=-----------------=
    private EventInstance footstepsInstance;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        footstepsInstance = Audio_FMODAudioManager.CreateInstance(Audio_FMODEvents.Instance.footstepsConcrete);
    }
    private void Update()
    {
        footstepsInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
    }
    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f, ~ignoreMask))
        {
            currentPos = hit.point;

            currentPos.y = 0;

            delta = (lastPos - currentPos).magnitude / Time.deltaTime;

            lastPos = currentPos;
        }
        else
            delta = 0;
    }
    private void OnDestroy()
    {
        footstepsInstance.release();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    void StartFootsteps()
    {
        footstepsInstance.start();
    }
    void StopFootsteps()
    {
        footstepsInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}
