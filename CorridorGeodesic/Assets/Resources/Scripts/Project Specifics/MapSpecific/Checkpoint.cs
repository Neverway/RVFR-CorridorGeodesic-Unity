using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : LogicComponent
{
    public bool clearsCheckpointsInstead = false;
    [Space]
    public string uniqueCheckpointName;
    [Tooltip("A checkpoint of a lower rank than what checkpoint you have currently will not get powered. Unless -1, in which case it ignores ranks")]
    public int checkpointRank = -1;
    [Tooltip("If true, checkpoint will become powered as soon as you activate it. Otherwise, only becomes powered upon death if checkpoint is at or lower than current checkpoint rank")]
    public bool doPowerOnActivation = false;

    [LogicComponentHandle] public LogicComponent triggerCheckpoint;
    [IsDomainReloaded] public static string lastCheckpointName;
    [IsDomainReloaded] public static int lastCheckpointRank = -1;

    private bool hasInitialized = false;

#if UNITY_EDITOR
    [DebugReadOnly, SerializeField] string DEBUG_lastCheckpoint;
    [DebugReadOnly, SerializeField] int DEBUG_lastRank;

    public void Update()
    {
        DEBUG_lastCheckpoint = lastCheckpointName;
        DEBUG_lastRank = lastCheckpointRank;
    }
    public void OnValidate()
    {
        if (!Application.isPlaying)
        {
            uniqueCheckpointName = Guid.NewGuid().ToString();
        }
    }
#endif

    

    public IEnumerator Start()
    {
        yield return null;
        yield return null;
        yield return null; //wait 3 frames to make extra sure I guess?

        if (AmIThisCheckpoint(lastCheckpointName) && !clearsCheckpointsInstead)
        {
            FindObjectOfType<Pawn>().transform.position = transform.position;
        }

        hasInitialized = true;
        isPowered = ShouldBePowered();
    }


    
    public override void OnEnable()
    {
        //Stopping OnEnable to trigger checkpoints
    }

    public override void SourcePowerStateChanged(bool powered)
    {
        if (!hasInitialized)
            return;

        base.SourcePowerStateChanged(powered);

        if (triggerCheckpoint.isPowered)
        {
            if (clearsCheckpointsInstead)
            {
                ClearCheckpoints();
                return;
            }
            else if (AmIHigherRank())
            {
                SetCheckpoint();
            }
        }
        if (doPowerOnActivation)
            isPowered = ShouldBePowered();
    }

    public bool AmIThisCheckpoint(string checkpoint)
    {
        return checkpoint != null && checkpoint == uniqueCheckpointName;
    }
    public bool AmIHigherRank()
    {
        return checkpointRank == -1 || checkpointRank > lastCheckpointRank;
    }
    public bool ShouldBePowered()
    {
        if (lastCheckpointRank == -1)
        {
            return AmIThisCheckpoint(lastCheckpointName);
        }
        else
        {
            return !AmIHigherRank();
        }
    }

    public void SetCheckpoint()
    {
        lastCheckpointName = uniqueCheckpointName;
        lastCheckpointRank = checkpointRank;
    }

    public static void ClearCheckpoints()
    {
        lastCheckpointName = null;
        lastCheckpointRank = -1;
        //Debug.Log("Clearing checkpoints");
    }


    //=----Reload Static Fields----=
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitializeStaticFields()
    {
        ClearCheckpoints();
    }
}
