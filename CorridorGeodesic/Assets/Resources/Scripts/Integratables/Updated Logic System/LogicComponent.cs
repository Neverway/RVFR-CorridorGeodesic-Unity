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


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void OnDestroy()
    {
        isPowered = false;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    public virtual void SourcePowerStateChanged(bool powered)
    {

    }
    public virtual void LocalPowerStateChanged(bool powered)
    {

    }

    //=-----------------=
    // External Functions
    //=-----------------=
}
