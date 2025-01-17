//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neverway.Framework.ApplicationManagement
{
public class WB_NotificationBox : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    private float timeTillDeath;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Transform root;
    [SerializeField] private GameObject keyhintWidget;


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
    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(timeTillDeath);
        //Destroy(gameObject);
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void DisplayKeyHint(float _duration, string _keyhintText, Sprite _keyhintImage)
    {
        StopCoroutine(DeathTimer());
        StartCoroutine(DeathTimer());
        var keyhint = Instantiate(keyhintWidget, root);
        keyhint.GetComponent<WB_NotificationBox_Keyhint>().SetKeyHint(_keyhintText, _keyhintImage);
        Destroy(keyhint, _duration);
    }
    
    public void DisplayKeyHint(float _duration, string _keyhintText, string _targetActionMap, string _targetAction)
    {
        StopCoroutine(DeathTimer());
        StartCoroutine(DeathTimer());
        var keyhint = Instantiate(keyhintWidget, root);
        keyhint.GetComponent<WB_NotificationBox_Keyhint>().SetKeyHint(_keyhintText, _targetActionMap, _targetAction);
        Destroy(keyhint, _duration);
    }
}
}
