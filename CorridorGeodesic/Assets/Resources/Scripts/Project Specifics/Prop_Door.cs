//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NEW_LogicProcessor))]
public class Prop_Door : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public NEW_LogicProcessor inputSignal;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------='
    private NEW_LogicProcessor logicProcessor;
    [Header("References")]
    [SerializeField] private Animator animator;
    [HideInInspector] [SerializeField] private UnityEvent onPowered;
    [HideInInspector] [SerializeField] private UnityEvent onUnpowered;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        logicProcessor = GetComponent<NEW_LogicProcessor>();
        //animator.keepAnimatorStateOnDisable = true;
    }

    private void Update()
    {
        if (!inputSignal) return;
        logicProcessor.isPowered = inputSignal.isPowered;
        
        if (inputSignal.hasPowerStateChanged)
        {
            if (logicProcessor.isPowered)
            {
                onPowered.Invoke();
            }
            else
            {
                onUnpowered.Invoke();
            }
            animator.SetBool("Powered", logicProcessor.isPowered);
        }
    }

    private void OnDrawGizmos()
    {
        if (inputSignal) Debug.DrawLine(gameObject.transform.position, inputSignal.transform.position, Color.blue);
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
