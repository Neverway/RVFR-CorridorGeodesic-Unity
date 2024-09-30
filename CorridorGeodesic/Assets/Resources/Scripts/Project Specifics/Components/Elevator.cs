//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Elevator : LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public UnityEvent OnReachFinalTarget;

    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private float moveSpeed = 3;
    private int targetIndex;
    private bool elevatorActive;

    private ElevatorState _currentState;
    private ElevatorState currentState
    {
        get { return _currentState; }
        set 
        {
            _currentState = value;
            SwitchState();
        }
    }
    [SerializeField] private ElevatorState startingState;
    private ElevatorState previousState;

    private EventInstance moveInstance;

    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField, LogicComponentHandle] private LogicComponent enableSignal;
    [SerializeField, LogicComponentHandle] private LogicComponent startSignal;
    [SerializeField, LogicComponentHandle] private LogicComponent stopSignal;
    [SerializeField, LogicComponentHandle] private LogicComponent forceOpenSignal;
    [SerializeField] private List<Transform> elevatorTargets = new List<Transform>();
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject doorOpenTrigger;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        targetIndex = GetStartingTargetIndex();

        currentState = startingState;

        moveInstance = Audio_FMODAudioManager.CreateInstance(Audio_FMODEvents.Instance.elevatorMove);
    }
    public override void OnEnable ()
    {
        base.OnEnable ();
        //SourcePowerStateChanged (isPowered);
    }
    private new void OnDestroy()
    {
        moveInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    private void Update()
    {
        moveInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position + Vector3.up));
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private IEnumerator MoveElevator()
    {
        Vector3 targetPos = elevatorTargets[targetIndex].position;

        while (transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        targetIndex++;

        if (targetIndex == elevatorTargets.Count)
        {
            currentState = ElevatorState.Idle;

            targetIndex = GetStartingTargetIndex();

            elevatorTargets.Reverse();

            OnReachFinalTarget?.Invoke();
        }
        else
            currentState = ElevatorState.Moving;
    }
    private void SwitchState()
    {
        switch (currentState)
        {
            case ElevatorState.Idle:
                IdleState();
                break;
            case ElevatorState.Moving:
                MovingState();
                break;
            default:
                break;
        }

        previousState = currentState;
    }
    private void IdleState()
    {
        StopAllCoroutines();

        bool state = isPowered ? isPowered : forceOpenSignal.isPowered;

        if (previousState != ElevatorState.Idle)
        {
            moveInstance.stop(STOP_MODE.ALLOWFADEOUT);
            if (state)
            {
                Audio_FMODAudioManager.PlayOneShot(Audio_FMODEvents.Instance.elevatorOpen, transform.position + Vector3.up * 2);
                Audio_FMODAudioManager.PlayOneShot(Audio_FMODEvents.Instance.elevatorReady, transform.position + Vector3.up * 2);
            }
            else
                Audio_FMODAudioManager.PlayOneShot(Audio_FMODEvents.Instance.elevatorClose, transform.position + Vector3.up * 2);
        }

        animator.SetBool("Powered", state);
    }
    private void MovingState()
    {
        StopAllCoroutines();

        if(previousState != ElevatorState.Moving)
        {
            moveInstance.start();
            Audio_FMODAudioManager.PlayOneShot(Audio_FMODEvents.Instance.elevatorClose, transform.position + Vector3.up * 2);
        }  

        animator.SetBool("Powered", false);
        
        StartCoroutine(MoveElevator());
    }
    private bool GetStopSignalPowerState()
    {
        if (stopSignal)
            return stopSignal.isPowered;
        else
            return false;
    }
    private int GetStartingTargetIndex()
    {
        return elevatorTargets.Count > 1 ? 1 : 0;
    }

    //=-----------------=
    // External Functions
    //=-----------------=
    public override void SourcePowerStateChanged(bool powered)
    {
        base.SourcePowerStateChanged(powered);

        isPowered = enableSignal ? enableSignal.isPowered : true;

        doorOpenTrigger.SetActive(!isPowered);

        if (isPowered)
        {
            if (startSignal && startSignal.isPowered)
                currentState = ElevatorState.Moving;
            else if (GetStopSignalPowerState() && currentState == ElevatorState.Moving)
                currentState = ElevatorState.Idle;
            else if (!GetStopSignalPowerState() && currentState == ElevatorState.Idle)
                currentState = ElevatorState.Idle;
        }
        else
            currentState = ElevatorState.Idle;
    }
}
