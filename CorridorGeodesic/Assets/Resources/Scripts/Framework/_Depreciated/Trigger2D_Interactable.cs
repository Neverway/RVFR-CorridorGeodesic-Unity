//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Logic_Processor))]
public class Trigger2D_Interactable : Volume
{/*
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string onInteractSignal;
    public bool isPowered;
    public string resetSignal;
    public bool resetsAutomatically = true;
    public bool hideIndicator;
    public bool useTalkIndicator;
    public UnityEvent onInteract;


    //=-----------------=
    // Private Variables
    //=-----------------=
    [HideInInspector] public bool hasBeenTriggered;
    private bool previousIsPoweredState; // Used to check for any overrides to the initial isPowered state in the level editor
    
    
    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Logic_Processor logicProcessor;
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
        var interaction = _other.GetComponent<Volume_TriggerInteraction>();
        if (interaction) Interact();
        SetInteractionState();
    }

    private new void OnTriggerExit2D(Collider2D _other)
    {
        base.OnTriggerExit2D(_other); // Call the base class method
        if (_other.CompareTag("Pawn")) if (targetEnt.isPossessed) targetEnt.isNearInteractable = false;
        SetInteractionState();
    }

    private void Update()
    {
        //CheckForPoweredStateOverrides();
        if (GetPlayerInTrigger()) GetPlayerInTrigger().isNearInteractable = true;
        SetInteractionState();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void CheckForPoweredStateOverrides()
    {
        // Check for any overrides that have modified the switch state
        if (previousIsPoweredState != isPowered) logicProcessor.UpdateState(onInteractSignal, isPowered);
        previousIsPoweredState = isPowered;
    }

    private void SetInteractionState()
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


    //=-----------------=
    // External Functions
    //=-----------------=
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
    }*/
}
