//===================== (Neverway 2024) Written by Connorses =====================
//
// Purpose: Manage toggling the state of all the LaserWallShooters that are part of a laser wall
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWall_Object : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=

    private LaserWallShooter[] laserWallShooters;


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        laserWallShooters = GetComponentsInChildren<LaserWallShooter>();
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
        foreach(LaserWallShooter wall in laserWallShooters)
        {
            wall.SetActive (true);
        }
    }

    public void Deactivate ()
    {
        foreach (LaserWallShooter wall in laserWallShooters)
        {
            wall.SetActive (false);
        }
    }
}