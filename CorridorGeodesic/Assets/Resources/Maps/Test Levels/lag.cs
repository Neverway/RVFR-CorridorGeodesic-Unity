//===================== (Neverway 2024) Written by _____ =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lag : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    private float sigma = 10f;

    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField, Range(0, 100)] private int lagAmount = 100;


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        for (int i = 0; i < lagAmount * 10; i++)
        {
            sigma *= Mathf.Lerp(sigma, i, sigma/10f);
            sigma /= 2.1452f;

            Debug.LogError($"SKibii{sigma}");
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
