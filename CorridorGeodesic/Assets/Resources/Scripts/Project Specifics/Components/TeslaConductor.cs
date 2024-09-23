using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;

public class TeslaConductor : MonoBehaviour, TeslaPowerSource
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Transform zapPosition;

    //=-----------------=
    // Private Variables
    //=-----------------=
    private List<TeslaReciever> powering;
    private LightningLine lineEffect;
    private TeslaPowerSource powerSource;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private IEnumerator Start()
    {
        yield return null;
        OnEnable();
    }
    public void Update()
    {
        if (!IsTeslaPowered())
            return;

        if (!IsTransformInTeslaRange(powerSource.GetZapTargetTransform()))
        {
            powerSource = null;
            StartCoroutine(DestroyLightning(lineEffect));
            return;
        }

        CheckToCreateLightning();

        if (lineEffect != null)
            lineEffect.SetStartAndEndPoints(GetZapTargetTransform().position, powerSource.GetZapTargetTransform().position);

        foreach (TeslaConductor conductor in TeslaManager.conductors)
        {
            if (!conductor.IsTeslaPowered())
            {
                if (IsTransformInTeslaRange(conductor.GetZapTargetTransform()))
                    conductor.SetPowerSource(this);
            }
        }
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

    public bool IsTransformInTeslaRange(Transform obj)
    {
        float distance = Vector3.Distance(GetZapTargetTransform().position, obj.position);

        return distance <= TeslaManager.MIN_DISTANCE;
    }

    public void SetPowerSource(TeslaPowerSource newSource)
    {
        if (!newSource.IsTeslaPowered())
            return;

        powerSource = newSource;

        

        if (newSource is TeslaSender)
            return;

        
    }

    public bool IsTeslaPowered() => powerSource != null && powerSource.IsTeslaPowered();

    public Transform GetZapTargetTransform() => zapPosition;

    public void CheckToCreateLightning()
    {
        if (!IsTeslaPowered() || lineEffect != null)
            return;

        lineEffect = Instantiate(TeslaManager.lightningLinePrefab);
        lineEffect.source1 = this;
        lineEffect.source2 = powerSource;
        lineEffect.SetStartAndEndPoints(GetZapTargetTransform().position, powerSource.GetZapTargetTransform().position);
    }

    public IEnumerator DestroyLightning(LightningLine lightning)
    {
        lightning.SetStartAndEndPoints(Vector3.zero, Vector3.zero);
        yield return null;
        if (lightning != null)
            Destroy(lightning.gameObject);
    }
}
