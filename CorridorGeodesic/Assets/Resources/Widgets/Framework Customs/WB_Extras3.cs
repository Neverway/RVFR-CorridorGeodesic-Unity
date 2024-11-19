//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Neverway.Framework;

public class WB_Extras3 : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField] public Levelpack[] levelpacks;
    public Button[] extraButtons;
    private WorldLoader worldLoader;
    [SerializeField] private Button buttonBack;

    [Serializable]
    public struct Levelpack
    {
        public string buttonText;
        public SceneList sceneList;
    }

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

        for (int i = 0; i < extraButtons.Length && i < levelpacks.Length; i++)
        {
            int index = i; //Need to do this for AddListener

            extraButtons[index].interactable = levelpacks[index].sceneList != null && levelpacks[index].sceneList.sceneNames.Length > 0;
            extraButtons[index].transform.GetChild(0).GetComponent<TMP_Text>().text = levelpacks[index].buttonText;

            extraButtons[index].onClick.AddListener(() =>
            {
                if (!worldLoader)
                {
                    worldLoader = FindObjectOfType<WorldLoader>();
                }
                worldLoader.LoadWorld(levelpacks[index].sceneList.sceneNames[0]);

                OverrideLevelChangeExitWithSceneList levelOverrider = 
                    new GameObject("Levelpack Level_Exit Overrider").AddComponent<OverrideLevelChangeExitWithSceneList>();
                levelOverrider.Initialize(levelpacks[index].sceneList, true);

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
