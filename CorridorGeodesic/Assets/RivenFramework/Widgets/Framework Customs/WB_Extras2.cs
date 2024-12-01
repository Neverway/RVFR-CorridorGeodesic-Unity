//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Provides a widget for selecting things like bonus content
// Notes: Modified version for CorGeo
//
//=============================================================================

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Neverway.Framework;

namespace Neverway.CorGeo
{
    public class WB_Extras2 : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        public string[] sceneID;
        public string[] buttonText;
        public Button[] extraButtons;
        private WorldLoader worldLoader;
        [SerializeField] private Button buttonBack;


        //=-----------------=
        // Private Variables
        //=-----------------=


        //=-----------------=
        // Reference Variables
        //=-----------------=


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void Start()
        {
            buttonBack.onClick.AddListener(() => { Destroy(gameObject); });

            for (int i = 0; i < extraButtons.Length && i < sceneID.Length; i++)
            {
                int index = i;
                extraButtons[index].interactable = sceneID[index] != null;
                extraButtons[index].transform.GetChild(0).GetComponent<TMP_Text>().text = buttonText[index];
                extraButtons[index].onClick.AddListener(() =>
                {
                    if (!worldLoader)
                    {
                        worldLoader = FindObjectOfType<WorldLoader>();
                    }

                    worldLoader.LoadWorld(sceneID[index]);
                });
            }
        }

        private void Update()
        {

        }

        //=-----------------=
        // Internal Functions
        //=-----------------=


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}