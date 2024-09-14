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
    public delegate void LifetimeOut();
    public event LifetimeOut OnLifetimeOut;

    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private DisableType disableType;
    [SerializeField] private float lifeTime = 1;
    [SerializeField] private bool startTimerOnEnable;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void OnEnable()
    {
        if(startTimerOnEnable)
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
            case DisableType.Event:
                OnLifetimeOut?.Invoke();
                break;
            default:
                break;
        }
    }

    //=-----------------=
    // External Functions
    //=-----------------=
    public void StartTimer()
    {
        if(!startTimerOnEnable)
            StartCoroutine(WaitLifetime());
    }
    public void StopTimer()
    {
        StopAllCoroutines();
    }
}
