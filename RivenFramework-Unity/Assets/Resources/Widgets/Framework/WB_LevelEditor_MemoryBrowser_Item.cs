//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WB_LevelEditor_MemoryBrowser_Item : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string tileId;
    public string tileName;
    public Sprite tileSprite;


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
        transform.GetChild(0).GetComponent<Image>().sprite = tileSprite;
        transform.GetChild(2).GetComponent<TMP_Text>().text = tileName;
        if (tileName == "")
        {
            transform.GetChild(2).GetComponent<TMP_Text>().text = tileId;
        }
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void SendTileToHotbar()
    {
        var LevelEditorWidget = FindObjectOfType<WB_LevelEditor>();
        LevelEditorWidget.SetCurrentHotBarTile(tileId);
    }
}
