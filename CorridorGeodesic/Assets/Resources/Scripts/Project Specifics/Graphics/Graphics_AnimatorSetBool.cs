//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphics_AnimatorSetBool: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private Animator anim;
    [SerializeField] private string propertyName;
	
    //=-----------------=
    // Reference Variables
    //=-----------------=
	
	
    //=-----------------=
    // Mono Functions
    //=-----------------=
	
	
    //=-----------------=
    // Internal Functions
    //=-----------------=
	public void SetBool(bool state)
    {
        anim.SetBool(propertyName, state);
    }
	
    //=-----------------=
    // External Functions
    //=-----------------=
}