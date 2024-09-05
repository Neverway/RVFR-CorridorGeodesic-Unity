//===================== (Neverway 2024) Written by Connorses =====================
//
// Purpose: Manage toggling the state of all the LaserWallShooters that are part of a laser wall
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop_LaserBarrier_Object : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=

    private Prop_LaserBarrier_Segment[] laserWallShooters;


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        laserWallShooters = GetComponentsInChildren<Prop_LaserBarrier_Segment>();
    }

    private void Update()
    {
    
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=

    public void Activate ()
    {
        foreach(Prop_LaserBarrier_Segment wall in laserWallShooters)
        {
            wall.SetActive (true);
        }
    }

    public void Deactivate ()
    {
        foreach (Prop_LaserBarrier_Segment wall in laserWallShooters)
        {
            wall.SetActive (false);
        }
    }
}