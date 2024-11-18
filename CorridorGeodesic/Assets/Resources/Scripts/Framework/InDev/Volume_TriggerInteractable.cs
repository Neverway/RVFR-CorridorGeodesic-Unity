//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Neverway.Framework
{
    public class Volume_TriggerInteractable : Volume
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        [Tooltip("If this is false, this trigger can only be activated once")]
        public bool resetsAutomatically = true;

        [Tooltip(
            "If this is false, a little indicator will appear above this volume to show the player it can be interacted with")]
        public bool hideIndicator;

        [Tooltip("If enabled, the indicator will show a speech bubble instead of the interact indicator")]
        public bool useTalkIndicator;

        [Tooltip("If enabled, then the actor who created the interaction volume must also be inside this trigger")]
        public bool requireActivatingActorInside = true;

        [Tooltip("How many seconds to remain powered when pressing interact")]
        public float secondsToStayPowered = 0.2f;

        [Header("Signal Events")]
        //[Tooltip("When this trigger is powered, this event will be fired (this is used to trigger things that don't use our signal system)")]
        public UnityEvent onInteract;


        //=-----------------=
        // Private Variables
        //=-----------------=
        //[SerializeField] private LogicComponent resetSignal;

        [Tooltip("A variable to keep track of if this volume has already been trigger")] [HideInInspector]
        public bool hasBeenTriggered;

        private bool
            previousIsPoweredState; // Used to check for any overrides to the initial isPowered state in the level editor


        //=-----------------=
        // Reference Variables
        //=-----------------=
        [Tooltip("This is the object that displays the sprite showing this object can be interacted with")]
        [SerializeField]
        private GameObject interactionIndicator;


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private new void OnDrawGizmos()
        {
            //if (resetSignal) Debug.DrawLine(transform.position, resetSignal.transform.position, new Color(1,0.5f,0,1));
        }

        private new void OnTriggerEnter2D(Collider2D _other)
        {
            base.OnTriggerEnter2D(_other); // Call the base class method
            // Check for interaction
            var interaction = _other.GetComponent<Volume_TriggerInteraction>();
            if (interaction) Interact(interaction);
            SetInteractionIndicatorState();
        }

        private new void OnTriggerExit2D(Collider2D _other)
        {
            base.OnTriggerExit2D(_other); // Call the base class method
            if (_other.CompareTag("Pawn"))
                if (targetEntity.isPossessed)
                    targetEntity.isNearInteractable = false;
            SetInteractionIndicatorState();
        }

        private new void OnTriggerEnter(Collider _other)
        {
            base.OnTriggerEnter(_other); // Call the base class method
            SetInteractionIndicatorState();

            // Check for interaction
            var interaction = _other.GetComponent<Volume_TriggerInteraction>();
            if (interaction)
            {
                Interact(interaction);
            }
        }

        private new void OnTriggerExit(Collider _other)
        {
            base.OnTriggerExit(_other); // Call the base class method
            SetInteractionIndicatorState();

            // Disable the indicator if the player left
            // (Shouldn't `SetInteractionIndicatorState();` already take care of this?? ~Liz)
            if (_other.CompareTag("Pawn") && targetEntity.isPossessed)
            {
                targetEntity.isNearInteractable = false;
            }
        }

        //=-----------------=
        // Internal Functions
        //=-----------------=
        private void SetInteractionIndicatorState()
        {
            if (interactionIndicator.activeInHierarchy)
                interactionIndicator.GetComponent<Animator>().Play(useTalkIndicator ? "talk" : "use");

            if (GetPlayerInTrigger() && !hideIndicator)
            {
                interactionIndicator.SetActive(true);
            }
            else
            {
                interactionIndicator.SetActive(false);
            }
        }

        private void Interact(Volume_TriggerInteraction _interaction)
        {
            if (hasBeenTriggered && !resetsAutomatically ||
                requireActivatingActorInside && !pawnsInTrigger.Contains(_interaction.targetPawn))
            {
                return;
            }

            onInteract.Invoke();
            // Dear future me, please keep in mind that this will not be called unless the onInteractSignal is set. I don't know if I intended for it to work that way. (P.S. I am using "-" for empty activations) ~Past Liz M.
            // Dear past me, you are a fool and a coward. I fixed it. ~Future Liz M.

            // Flip the current activation state
            StartCoroutine(WaitUnpower());
            //previousIsPoweredState = isPowered;

            // Update connected devices
            hasBeenTriggered = true;
        }

        IEnumerator WaitUnpower()
        {
            isPowered = true;
            yield return null;
            yield return new WaitForSeconds(secondsToStayPowered);
            isPowered = false;
        }


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}