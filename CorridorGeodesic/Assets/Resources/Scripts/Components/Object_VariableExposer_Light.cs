//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Exposes variables to control a Light component's properties in the level editor
// Notes: This script requires a Light component attached to the same GameObject.
// Added variable for spot angle and improved performance by only updating
// and grabbing the light when needed -Soulex
//
//=============================================================================

using UnityEngine;

/// <summary>
/// Exposes variables to control a Light component's properties in the level editor
/// </summary>
[RequireComponent(typeof(Light))]
public class Object_VariableExposer_Light : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public float intensity;
    public float range, angle;
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

    private void OnValidate()
    {
        UpdateLight();
    }
    private void Start()
    {
        UpdateLight();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=

    private void UpdateLight()
    {
        if(targetLight == null)
            targetLight = GetComponent<Light>();

        targetLight.intensity = intensity;
        targetLight.range = range;
        targetLight.spotAngle = angle;
        targetLight.color = new Color(colorRed, colorGreen, colorBlue);
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}
