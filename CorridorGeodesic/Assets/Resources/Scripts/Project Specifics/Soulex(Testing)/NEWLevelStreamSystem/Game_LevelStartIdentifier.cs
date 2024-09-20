//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_LevelStartIdentifier : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
	public static Game_LevelStartIdentifier Instance;
    public Transform startTransform;

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
        Instance = this;

        StartCoroutine(WaitForLocalPlayer());
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    IEnumerator WaitForLocalPlayer()
    {
        yield return new WaitUntil(()=> GameInstance.Instance.localPlayerCharacter != null);

        GameInstance.Instance.localPlayerCharacter.transform.position = startTransform.position + Vector3.up;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
