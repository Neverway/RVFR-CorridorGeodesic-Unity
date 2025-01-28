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

namespace Neverway.Framework.ApplicationManagement
{
public class WB_NotificationBox_Keyhint : MonoBehaviour
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
    [SerializeField] private Image_KeyHint keyHint;
    [SerializeField] private TMP_Text keyhintText;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        
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
    public void SetKeyHint(string _keyhintText, Sprite _keyhintImage)
    {
        keyhintText.text = _keyhintText;
        keyHint.enabled = false;
        keyHint.gameObject.GetComponent<Image>().sprite = _keyhintImage;
        GetComponent<Animator>().Play("WB_NotificationBox_Keyhint_appear");
    }
    public void SetKeyHint(string _keyhintText, string _targetActionMap, string _targetAction)
    {
        keyhintText.text = _keyhintText;
        keyHint.enabled = true;
        keyHint.targetActionMap = _targetActionMap;
        keyHint.targetAction = _targetAction;
        GetComponent<Animator>().Play("WB_NotificationBox_Keyhint_appear");
    }
}
}
