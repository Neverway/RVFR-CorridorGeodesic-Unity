//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn_WeaponInventory : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public bool hasGeoGun;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private GameObject geoGun;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
    
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
    public void GiveGeoGun()
    {
        print("GIVE ME THE DANG GUN pls ~Liz");
        geoGun.SetActive(true);
    }
    
    public void TakeGeoGun()
    {
        geoGun.SetActive(false);
    }
}
