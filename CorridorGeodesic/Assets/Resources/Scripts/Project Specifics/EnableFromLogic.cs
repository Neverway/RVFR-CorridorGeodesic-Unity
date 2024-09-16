using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnableFromLogic : LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Transform[] objectsToEnableWhenActive;
    public Transform[] objectsToDisableWhenActive;

    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField, LogicComponentHandle] private LogicComponent inputSignal;

    //=-----------------=
    // Reference Variables
    //=-----------------='
    [Header("References")]
    [HideInInspector][SerializeField] private UnityEvent onPowered;
    [HideInInspector][SerializeField] private UnityEvent onUnpowered;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    protected new void OnEnable()
    {
        base.OnEnable();
        SourcePowerStateChanged(isPowered);
    }
    //=-----------------=
    // Internal Functions
    //=-----------------=

    //=-----------------=
    // External Functions
    //=-----------------=
    public override void SourcePowerStateChanged(bool powered)
    {
        isPowered = powered;

        foreach(Transform t in objectsToEnableWhenActive)
            t.gameObject.SetActive(powered);

        foreach (Transform t in objectsToDisableWhenActive)
            t.gameObject.SetActive(!powered);

    }
}
