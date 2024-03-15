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

[CreateAssetMenu(fileName="Item_Utility_Throwable", menuName="Neverway/ScriptableObjects/Items/Utility_Throwable")]
public class Item_Utility_Throwable : Item
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public float baseDamage;
    public Vector2 forceStrength;
    public float removeForceDelay;
    public string damageType;
    public float speed;
    public float drag;
    [Tooltip("This is an object that will be instantiated when the object collides with something")] public GameObject hitObject;
    [Tooltip("This is how long the object will exist for before destructing and creating the hit object")] public float lifetime;


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

