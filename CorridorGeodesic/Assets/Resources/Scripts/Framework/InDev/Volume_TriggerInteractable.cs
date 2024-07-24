//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Logic_Processor))]
public class Volume_TriggerInteractable : Volume
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("Transmits a power signal to all devices with the same string when this volume is interacted with")]
    public string onInteractSignal;
    [Tooltip("A readable value to tell if this trigger is currently transmitting a power signal")]
    public bool isPowered;
    public string resetSignal;
    [Tooltip("If this is false, this trigger can only be activated once")]
    public bool resetsAutomatically = true;
    [Tooltip("If this is false, a little indicator will appear above this volume to show the player it can be interacted with")]
    public bool hideIndicator;
    [Tooltip("If this is false, a little indicator will appear above this volume to show the player it can be interacted with")]
    public bool useTalkIndicator;
    [Tooltip("When this trigger is powered, this event will be fired (this is used to trigger things that don't use our signal system)")]
    public UnityEvent onInteract;


    //=-----------------=
    // Private Variables
    //=-----------------=
    [Tooltip("A variable to keep track of if this volume has already been trigger")]
    [HideInInspector] public bool hasBeenTriggered;
    private bool previousIsPoweredState; // Used to check for any overrides to the initial isPowered state in the level editor


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Logic_Processor logicProcessor;
    [Tooltip("This is the object that displays the sprite showing this object can be interacted with")]
    [SerializeField] private GameObject interactionIndicator;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        logicProcessor = GetComponent<Logic_Processor>();
    }
    
    private new void OnTriggerEnter2D(Collider2D _other)
    {
        base.OnTriggerEnter2D(_other); // Call the base class method
        // Check for interaction
        var interaction = _other.GetComponent<Trigger2D_Interaction>();
        if (interaction) Interact();
        SetInteractionIndicatorState();
    }

    private new void OnTriggerExit2D(Collider2D _other)
    {
        base.OnTriggerExit2D(_other); // Call the base class method
        if (_other.CompareTag("Pawn")) if (targetEnt.isPossessed) targetEnt.isNearInteractable = false;
        SetInteractionIndicatorState();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void SetInteractionIndicatorState()
    {
        if (interactionIndicator.activeInHierarchy) interactionIndicator.GetComponent<Animator>().Play(useTalkIndicator ? "talk" : "use");
        
        if (GetPlayerInTrigger() && !hideIndicator)
        {
            interactionIndicator.SetActive(true);
        }
        else
        {
            interactionIndicator.SetActive(false);
        }
    }
    
    private void Interact()
    {
        if (onInteractSignal == "" || hasBeenTriggered && !resetsAutomatically){ return;}
        onInteract.Invoke(); // Dear future me, please keep in mind that this will not be called unless the onInteractSignal is set. I don't know if I intended for it to work that way. (P.S. I am using "-" for empty activations) ~Past Liz M.
        
        // Flip the current activation state
        isPowered = !isPowered;
        previousIsPoweredState = isPowered;
        
        // Update connected devices
        logicProcessor.UpdateState(onInteractSignal, isPowered);
        hasBeenTriggered = true;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
