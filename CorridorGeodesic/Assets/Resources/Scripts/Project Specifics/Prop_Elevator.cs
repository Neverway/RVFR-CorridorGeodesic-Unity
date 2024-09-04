//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(NEW_LogicProcessor))]
public class Prop_Elevator : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public NEW_LogicProcessor doorSignal;
    public NEW_LogicProcessor liftSignal;


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private Transform elevatorTransform;
    private bool elevatorActivated;
    

    //=-----------------=
    // Reference Variables
    //=-----------------=
    private NEW_LogicProcessor logicProcessor;
    [SerializeField] private Animator animator;


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
        if (!elevatorActivated && doorSignal)
        {
            logicProcessor.isPowered = doorSignal.isPowered;
        
            if (doorSignal.hasPowerStateChanged)
            {
                animator.SetBool("Powered", logicProcessor.isPowered);
            }
        }

        if (liftSignal && liftSignal.isPowered && !elevatorActivated)
        {
            animator.SetBool("Powered", false);
            StartCoroutine(DescendElevator());
            elevatorActivated = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (doorSignal) Debug.DrawLine(gameObject.transform.position, doorSignal.transform.position, Color.blue);
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    IEnumerator DescendElevator()
    {
        float timer = 10;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            elevatorTransform.position -= Vector3.up * Time.deltaTime * 3;
            yield return null;
        }
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}
