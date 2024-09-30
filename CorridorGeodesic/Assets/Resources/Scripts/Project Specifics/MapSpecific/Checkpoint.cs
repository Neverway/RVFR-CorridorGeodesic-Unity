using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : LogicComponent
{
    public bool clearsCheckpointsInstead = false;
    [Space]
    public string uniqueCheckpointName;
    [LogicComponentHandle] public LogicComponent triggerCheckpoint;
    [DebugReadOnly, SerializeField] string DEBUG_lastCheckpoint;


    public static string lastCheckpointName;


#if UNITY_EDITOR
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

        if (AmIThisCheckpoint(lastCheckpointName))
        {
            FindObjectOfType<Pawn>().transform.position = transform.position;
            Debug.Log("ELLO Did this work?");
        }
    }

    public void Update()
    {
        DEBUG_lastCheckpoint = lastCheckpointName;
    }
    public override void OnEnable()
    {
        //Stopping OnEnable to trigger checkpoints
    }

    public override void SourcePowerStateChanged(bool powered)
    {
        base.SourcePowerStateChanged(powered);

        if (triggerCheckpoint.isPowered)
        {
            if (clearsCheckpointsInstead)
            {
                ClearCheckpoints();
                return;
            }

            lastCheckpointName = uniqueCheckpointName;
            Debug.Log("Set Checkpoint to: " + uniqueCheckpointName);
        }

        isPowered = AmIThisCheckpoint(lastCheckpointName);

    }

    public bool AmIThisCheckpoint(string checkpoint)
    {
        return checkpoint != null && checkpoint == uniqueCheckpointName;
    }

    public static void ClearCheckpoints()
    {
        lastCheckpointName = null;
        Debug.Log("Clearing checkpoints");
    }
}
