//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose: Used for processing logic events for puzzle mechanics
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LogicComponent: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField] private bool _isPowered;

    public delegate void PowerStateChanged(bool powered);
    public event PowerStateChanged OnPowerStateChanged;

    public bool isPowered
    {
        get { return _isPowered; }
        set
        {
            if (_isPowered != value)
            {
                _isPowered = value;

                OnPowerStateChanged?.Invoke(_isPowered);
                LocalPowerStateChanged(_isPowered);
            }
        }
    }

    //=-----------------=
    // Private Variables
    //=-----------------=
    protected List<LogicComponent> subscribeLogicComponents = new List<LogicComponent>();

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        AutoSubscribe();
    }
    private void OnEnable()
    {
        //Fixes Animators to update its power state when re-enabled
        SourcePowerStateChanged(isPowered);
    }

    private void OnDestroy()
    {
        if (subscribeLogicComponents.Count <= 0)
            return;
        subscribeLogicComponents.ForEach(component =>
        {
            if (component)
                component.OnPowerStateChanged -= SourcePowerStateChanged;
        });

        isPowered = false;
    }
    void OnDrawGizmos()
    {
        if(subscribeLogicComponents.Count > 0)
        {
            subscribeLogicComponents.ForEach(c =>
            {
                if (!c)
                    return;
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(transform.position, 0.2f);

                Gizmos.color = Color.green;
                Gizmos.DrawSphere(c.transform.position, 0.2f);

                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, c.transform.position);
            });
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    public virtual void AutoSubscribe()
    {
        Subscribe();
    }
    public virtual void SourcePowerStateChanged(bool powered)
    {

    }
    public virtual void LocalPowerStateChanged(bool powered)
    {

    }
    void Subscribe()
    {
        if (subscribeLogicComponents.Count <= 0)
            return;
        subscribeLogicComponents.ForEach(component =>
        {
            if (component)
                component.OnPowerStateChanged += SourcePowerStateChanged;
        });
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}
