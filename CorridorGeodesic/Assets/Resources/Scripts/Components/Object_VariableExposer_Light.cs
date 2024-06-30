//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Exposes variables to control a Light component's properties.
// Notes: This script requires a Light component attached to the same GameObject.
//
//=============================================================================

using UnityEngine;

[RequireComponent(typeof(Light))]
public class Object_VariableExposer_Light : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public float intensity;
    public float range;
    public float colorRed, colorGreen, colorBlue;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Light targetLight;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        targetLight = GetComponent<Light>();
    }

    private void Update()
    {
        targetLight.intensity = intensity;
        targetLight.range = range;
        targetLight.color = new Color(colorRed, colorGreen, colorBlue);
    }

    
    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
