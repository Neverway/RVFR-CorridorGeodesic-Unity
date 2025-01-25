//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Provides a widget for selecting things like bonus content
// Notes:
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;

namespace Neverway.Framework.PawnManagement
{
    public class WB_Extras : MonoBehaviour
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
        private WorldLoader worldLoader;
        [SerializeField] private Button buttonBack, buttonExtra1, buttonExtra2, buttonExtra3;


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void Start()
        {
            buttonBack.onClick.AddListener(() => { Destroy(gameObject); });
            buttonExtra1.onClick.AddListener(() =>
            {
                if (!worldLoader)
                {
                    worldLoader = FindObjectOfType<WorldLoader>();
                }

                worldLoader.LoadWorld("_LevelEditor");
            });
        }


        //=-----------------=
        // Internal Functions
        //=-----------------=


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}