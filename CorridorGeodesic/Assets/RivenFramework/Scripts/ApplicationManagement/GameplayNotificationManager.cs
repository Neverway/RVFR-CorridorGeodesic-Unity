//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using Neverway.Framework.PawnManagement;
using UnityEngine;

namespace Neverway.Framework.ApplicationManagement
{
public class GameplayNotificationManager : MonoBehaviour
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
    [SerializeField] private GameObject notificationBoxWidget;


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
    public void DisplayKeyHint(float _duration, string _keyhintText, Sprite _keyhintImage)
    {
        if (!GameInstance.GetWidget(notificationBoxWidget.name))
        {
            GameInstance.AddWidget(notificationBoxWidget);
        }
        FindObjectOfType<WB_NotificationBox>().DisplayKeyHint(_duration, _keyhintText, _keyhintImage);
    }
    public void DisplayKeyHint(float _duration, string _keyhintText, string _targetActionMap, string _targetAction)
    {
        if (!GameInstance.GetWidget(notificationBoxWidget.name))
        {
            GameInstance.AddWidget(notificationBoxWidget);
        }
        FindObjectOfType<WB_NotificationBox>().DisplayKeyHint(_duration, _keyhintText, _targetActionMap, _targetAction);
    }
}
}
