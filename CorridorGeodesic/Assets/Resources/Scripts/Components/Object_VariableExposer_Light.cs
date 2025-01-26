//===================== (Neverway 2024) Written by Liz M., Changes by Andre B. =====================
//
// Purpose: Exposes variables to control a Light component's properties.
// Notes: Added variable for spot angle and Improved performance by only updating and
// grabbing the light when needed, rather than every frame -Andre B.
//
//=============================================================================

using UnityEngine;

namespace Neverway.Framework.Cartographer
{
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
            if (targetLight == null)
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
}