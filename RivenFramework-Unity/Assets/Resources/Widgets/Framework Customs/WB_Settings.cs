//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;

public class WB_Settings : MonoBehaviour
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
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
