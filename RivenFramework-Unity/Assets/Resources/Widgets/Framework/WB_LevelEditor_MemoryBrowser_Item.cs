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
    public string tileID;
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
        transform.GetChild(2).GetComponent<TMP_Text>().text = tileID;
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
        LevelEditorWidget.SetCurrentHotBarTile(tileID);
    }
}
