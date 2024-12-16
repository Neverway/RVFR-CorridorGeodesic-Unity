//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Randomly change the size of a gameobject to give the apperence of a flicker
// Notes: We love Unity!!! (Editor note: We do not.)
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObject_Flicker : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField] private float size = .2f;
    [SerializeField] private float variance = 0.1f;


    //=-----------------=
    // Private Variables
    //=-----------------=
    

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        gameObject.transform.localScale = Vector3.one *  Random.Range (size - variance, size + variance);
    }
    
    
    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
