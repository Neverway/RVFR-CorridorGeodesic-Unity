//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes: This is currently CorGeo specific, it should be adopted into the framework
//
//=============================================================================

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Neverway.Framework
{
    public class WB_DeathScreen : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=


        //=-----------------=
        // Private Variables
        //=-----------------=


        //=-----------------=
        // Reference Variables
        //=-----------------=
        private InputActions.FirstPersonShooterActions fpsActions;
        private WorldLoader worldLoader;


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void Start()
        {
            // Setup inputs
            fpsActions = new InputActions().FirstPersonShooter;
            fpsActions.Enable();
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                worldLoader = FindObjectOfType<WorldLoader>();
                worldLoader.ForceLoadWorld(SceneManager.GetActiveScene().name, 0.25f);
            }
        }

        //=-----------------=
        // Internal Functions
        //=-----------------=


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}