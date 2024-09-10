//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opt_Lifetime: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private DisableType disableType;
    [SerializeField] private float lifeTime = 1;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void OnEnable()
    {
        StartCoroutine(WaitLifetime());
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    IEnumerator WaitLifetime()
    {
        yield return new WaitForSeconds(lifeTime);

        switch (disableType)
        {
            case DisableType.Destroy:
                Destroy(gameObject);
                break;
            case DisableType.Disable:
                gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}
