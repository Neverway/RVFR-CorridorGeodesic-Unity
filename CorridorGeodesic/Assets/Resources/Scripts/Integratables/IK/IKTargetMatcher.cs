//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neverway
{
public class IKTargetMatcher : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Transform target;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
    
    }

    private void Update()
    {
        transform.position = target.transform.position;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
}
