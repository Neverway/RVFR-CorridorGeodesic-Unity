//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Widget for a local or steam-powered leaderboard
// Notes:
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;

namespace Neverway.Framework.PawnManagement
{
    public class WB_Ranking : MonoBehaviour
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
        [SerializeField] private Button buttonBack;


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void Start()
        {
            buttonBack.onClick.AddListener(() => { Destroy(gameObject); });
        }

        //=-----------------=
        // Internal Functions
        //=-----------------=


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}