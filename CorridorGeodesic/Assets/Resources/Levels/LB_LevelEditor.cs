//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;

public class LB_LevelEditor : MonoBehaviour
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
    private GameInstance gameInstance;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        gameInstance = FindObjectOfType<GameInstance>();
        // I'm removing this and just making the Widget be pre-instantiated since the LevelEditor widget has SO MANY DANG REFERENCES IN THE CURRENT SCENE (P.S Don't tell Kevin)
        // (Screw you past me!) ~Liz
        // Oh boo-hoo, it's four references you dope! ~Future Liz
        //gameInstance.UI_ShowLevelEditor();
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
