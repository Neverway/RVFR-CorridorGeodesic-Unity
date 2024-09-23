using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaConductor : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [DebugReadOnly] public bool isRecievingPower;

    //=-----------------=
    // Private Variables
    //=-----------------=
    private List<TeslaReciever> powering;
    private List<LightningLine> lineEffects;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private IEnumerator Start()
    {
        yield return null;
        OnEnable();
    }
    private void OnEnable()
    {
        if (!TeslaManager.conductors.Contains(this))
            TeslaManager.conductors.Add(this);
    }
    private void OnDisable()
    {
        TeslaManager.conductors.Remove(this);
    }
}
