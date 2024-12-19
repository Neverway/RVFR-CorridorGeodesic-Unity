using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neverway.Framework.LogicSystem;

public class MoveOnLogic : LogicComponent
{
    [LogicComponentHandle] public LogicComponent inputSignal;
    [LogicComponentHandle] public LogicComponent pauseMovement;
    public float duration = 1f;
    
    public Transform startPosition, endPosition;
    public Transform[] objectsToMove;
    public Vector3[] storedPositions;


    public Vector3 lastPosition;

    private new void Awake()
    {
        base.Awake();
        Debug.LogWarning("TODO: Make sure this script doesnt break when rifts are active");
        transform.SetParent(null);
        transform.position = startPosition.position;
        lastPosition= transform.position;

        storedPositions = new Vector3[objectsToMove.Length];
        for (int i = 0; i < objectsToMove.Length; i++)
        {
            storedPositions[i] = objectsToMove[i].transform.position;
        }
    }
    private void LateUpdate()
    {
        if (pauseMovement.isPowered)
            return;

        Vector3 positionChange = transform.position - lastPosition;
        lastPosition = transform.position;

        for (int i = 0; i < objectsToMove.Length; i++)
        {
            objectsToMove[i].transform.position = storedPositions[i] + (transform.position - startPosition.position);
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
