using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnLogic : LogicComponent
{
    [LogicComponentHandle] public LogicComponent inputSignal;
    [LogicComponentHandle] public LogicComponent pauseMovement;
    public float duration = 1f;
    
    public Transform startPosition, endPosition;
    public Transform[] objectsToMove;



    public Vector3 lastPosition;

    private new void Awake()
    {
        base.Awake();
        Debug.LogWarning("TODO: Make sure this script doesnt break when rifts are active");
        transform.SetParent(null);
        transform.position = startPosition.position;
        lastPosition= transform.position;
    }
    private void Update()
    {
        if (pauseMovement.isPowered)
            return;

        Vector3 positionChange = transform.position - lastPosition;
        lastPosition = transform.position;

        foreach (Transform obj in objectsToMove)
        {
            obj.transform.position += positionChange;
        }
    }

    public override void SourcePowerStateChanged(bool powered)
    {
        base.SourcePowerStateChanged(powered);

        if (pauseMovement != null && pauseMovement.isPowered)
            return;

        Vector3 target = (inputSignal.isPowered ? endPosition.position : startPosition.position);
        transform.DOKill();
        transform.DOMove(target, 1f, false);
    }
}
