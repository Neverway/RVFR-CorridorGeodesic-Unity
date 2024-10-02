//===================== (Neverway 2024) Written by _____ =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_Oilable: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private float oilSpeedScale = 1;

    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Collider col;
    [SerializeField] private PhysicMaterial slipperymat;
    [SerializeField] private Graphics_ChangeMaterialProperty changeMaterialProperty;
	
    //=-----------------=
    // Mono Functions
    //=-----------------=
	
	
    //=-----------------=
    // Internal Functions
    //=-----------------=
	
	
    //=-----------------=
    // External Functions
    //=-----------------=
    public void AddOil(float amount)
    {
        changeMaterialProperty.ChangePropertyManual((float)changeMaterialProperty.GetProperty() + amount * oilSpeedScale);

        if ((float)changeMaterialProperty.GetProperty() > 0.3f)
            col.material = slipperymat;
    }
    public void RemoveOil(float amount)
    {
        changeMaterialProperty.ChangePropertyManual((float)changeMaterialProperty.GetProperty() - amount * oilSpeedScale);
    }
}
