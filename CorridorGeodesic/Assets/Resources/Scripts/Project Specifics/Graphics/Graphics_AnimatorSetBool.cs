//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private float setBoolDelay = 0;
	
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
        StopAllCoroutines();
        anim.SetBool(propertyName, state);
    }
    public void SetBoolDelay(bool state)
    {
        StartCoroutine(Delay(state));
    }
    IEnumerator Delay(bool state)
    {
        yield return new WaitForSeconds(setBoolDelay);
        anim.SetBool(propertyName, state);
    }
	
    //=-----------------=
    // External Functions
    //=-----------------=
}
[System.Serializable]
public class Graphics_AnimatorSetBoolDelayEvent : UnityEvent<bool, float>
{
}