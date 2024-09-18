using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnLogic : LogicComponent
{
    [LogicComponentHandle] public LogicComponent inputSignal;
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
    }
    private void Update()
    {
        Vector3 positionChange = lastPosition - transform.position;
        lastPosition = transform.position;

        foreach (Transform obj in objectsToMove)
        {
            obj.transform.position += positionChange;
        }
    }

    public override void SourcePowerStateChanged(bool powered)
    {
        base.SourcePowerStateChanged(powered);

        Vector3 target = (powered ? endPosition.position : startPosition.position);
        transform.DOKill();
        transform.DOMove(target, 1f, false);
    }
}
