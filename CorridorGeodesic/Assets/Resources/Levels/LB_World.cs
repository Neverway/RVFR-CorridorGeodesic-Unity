//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes: 
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LB_World : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public bool shouldHaveGeoGun=true;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private GameInstance gameInstance;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        gameInstance = FindObjectOfType<GameInstance>();
        gameInstance.UI_ShowHUD();
        if (shouldHaveGeoGun)
        {
            if (FindObjectOfType<Pawn_ItemInventory>())
            {
                FindObjectOfType<Pawn_ItemInventory>().GiveGeoGun();
            }
        }
    }
    private void Update()
    {
        if (shouldHaveGeoGun)
        {
            if (FindObjectOfType<Pawn_ItemInventory>())
            {
                FindObjectOfType<Pawn_ItemInventory>().GiveGeoGun();
            }
        }
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
