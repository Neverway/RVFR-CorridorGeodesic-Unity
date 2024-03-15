//========== Neverway 2023 Project Script | Written by Unknown Dev ============
// 
// Type: 
// Purpose: 
// Applied to: 
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[CreateAssetMenu(fileName="Item", menuName="Neverway/ScriptableObjects/Item")]
public class Item : ScriptableObject
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string itemName;
    public string description;
    public bool isDiscardable = true;
    public Sprite inventoryIcon;
    [Header("0 - Utility, 1 - Weapon, 2 - Magic, 3 - Defense")]
    [Range(0,3)] public int category;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}

