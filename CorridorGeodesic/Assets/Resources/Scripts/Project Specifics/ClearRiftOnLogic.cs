using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearRiftOnLogic : LogicComponent
{
    [LogicComponentHandle] public LogicComponent inputSignal;
    [DebugReadOnly] public Pawn_WeaponInventory nixieCross;
    private bool continuePowered = false;

    public void Update()
    {
        if (continuePowered || inputSignal != null && inputSignal.isPowered)
        {
            if (Alt_Item_Geodesic_Utility_GeoGun.currentState == RiftState.None)
            {
                isPowered = true;
                continuePowered = false;
            }
            else
            {
                continuePowered = true;
                ClearGeoGunRifts();
            }
        }
        else
            isPowered = false;
    }

    public void ClearGeoGunRifts()
    {
        if (nixieCross == null)
        {
            nixieCross = FindObjectOfType<Pawn_WeaponInventory>();
            if (nixieCross == null)
                return;
        }
        nixieCross.ClearGeoGunRifts();
    }
}
