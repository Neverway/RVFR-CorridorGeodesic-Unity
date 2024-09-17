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
    public Component[] componentsToEnableWhenActive;
    public Component[] componentsToDisableWhenActive;

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
        //base.OnEnable();
        //SourcePowerStateChanged(isPowered);
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

        //todo: Set this to use reflection to just find if it has an "enabled" property
        foreach (Component c in componentsToEnableWhenActive) 
        {
            if (c is Behaviour)
                ((Behaviour)c).enabled = powered;

            else if (c is Renderer)
                ((Renderer)c).enabled = powered;

            else if (c is Collider)
                ((Collider)c).enabled = powered;

            else
                Debug.LogWarning("Component " + c.GetType().Name + " on " + c.gameObject + " is not setup to be enabled/disabled yet");
        }

        foreach (Component c in componentsToDisableWhenActive)
        {
            if (c is Behaviour)
                ((Behaviour)c).enabled = !powered;

            else if (c is Renderer)
                ((Renderer)c).enabled = !powered;

            else if (c is Collider)
                ((Collider)c).enabled = !powered;

            else
                Debug.LogWarning("Component " + c.GetType().Name + " on " + c.gameObject + " is not setup to be enabled/disabled yet");
        }
    }
}
