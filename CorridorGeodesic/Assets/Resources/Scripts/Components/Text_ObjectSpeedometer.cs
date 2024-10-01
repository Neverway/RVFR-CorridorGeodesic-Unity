//===================== (Neverway 2024) Written by Liz M. =====================
// 
// Purpose: Show the velocity of a target object in a text element
// Notes: 
//
//=============================================================================

using TMPro;
using UnityEngine;

/// <summary>
/// Show the velocity of a target object in a text element
/// </summary>
public class Text_ObjectSpeedometer : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField] private bool targetLocalPlayer;


    //=-----------------=
    // Private Variables
    //=-----------------=
    
    
    //=-----------------=
    // Reference Variables
    //=-----------------=
    private TMP_Text velocityText;
    [SerializeField] private Rigidbody entityRigidbody;
    private GameInstance gameInstance;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        velocityText = GetComponent<TMP_Text>();
        gameInstance = FindObjectOfType<GameInstance>();
    }

    private void Update()
    {
        if (targetLocalPlayer && !entityRigidbody)
        {
            gameInstance = FindObjectOfType<GameInstance>();
            if (gameInstance.localPlayerCharacter.GetComponent<Rigidbody>()) entityRigidbody = gameInstance.localPlayerCharacter.GetComponent<Rigidbody>();
        }
        if (!entityRigidbody) return;
        velocityText.text = "Velocity: " + entityRigidbody.velocity.magnitude.ToString("F2") + " m/s";
    }
    
    
    //=-----------------=
    // Internal Functions
    //=-----------------=
    
    
    //=-----------------=
    // External Functions
    //=-----------------=
}

