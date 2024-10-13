//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phys_Oilable : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private float oilSpeedScale = 1;

    private bool _burning;
    private bool burning
    {
        get { return _burning; }
        set
        {
            if(_burning == false && value)
            {
                StopAllCoroutines();
                StartCoroutine(Burn());
            }
            _burning = value;
        }
    }

    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Collider col;
    [SerializeField] private PhysicMaterial slipperymat;
    [SerializeField] private Graphics_ChangeMaterialProperty changeMaterialProperty;
    [SerializeField] private GameObject flameParticles;

    private float oilAmount => (float)changeMaterialProperty.GetProperty();

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void FixedUpdate()
    {
        if(!burning && oilAmount > 0.3f && OilManager.Instance.BurnAtPositionSingle(transform.position))
        {
            burning = true;
        }

        if (burning)
            OilManager.Instance.StartFlameSingle(transform.position);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    IEnumerator Burn()
    {
        flameParticles.transform.localScale = Vector3.one;
        flameParticles.SetActive(true);
        while (oilAmount > 0)
        {
            RemoveOil(Time.deltaTime * 0.2f);
            yield return null;
        }
        float timer = 0.25f;

        burning = false;

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            flameParticles.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, 1 - timer/0.25f);
            yield return null;
        }

        flameParticles.SetActive(false);
    }

    //=-----------------=
    // External Functions
    //=-----------------=
    public void AddOil(float amount)
    {
        changeMaterialProperty.ChangePropertyManual(Mathf.Clamp01(oilAmount + amount * oilSpeedScale));

        if (oilAmount > 0.3f)
            col.material = slipperymat;
    }
    public void RemoveOil(float amount)
    {
        changeMaterialProperty.ChangePropertyManual(Mathf.Clamp01(oilAmount - amount * oilSpeedScale));

        if (oilAmount <= 0.3f)
            col.material = null;
    }
}
