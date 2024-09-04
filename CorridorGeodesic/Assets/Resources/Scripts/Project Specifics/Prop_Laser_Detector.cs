//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NEW_LogicProcessor))]
public class Prop_Laser_Detector : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=

    //=-----------------=
    // Private Variables
    //=-----------------=

    private float timeSinceHit = 0f;
    private float timeToCancelHit = 0.1f;
    [SerializeField] private Material offMaterial;
    [SerializeField] private Material onMaterial;
    [SerializeField] private GameObject laserLight;
    private bool isActive = false;

    //=-----------------=
    // Reference Variables
    //=-----------------=
    private NEW_LogicProcessor logicProcessor;
    [SerializeField] private Renderer laserRenderer;
    public UnityEvent OnPowered;
    public UnityEvent OnNotPowered;


    //=-----------------=
    // Mono Functions
    //=-----------------=

    private void Start ()
    {
        logicProcessor = GetComponent<NEW_LogicProcessor>();
        laserRenderer.material = offMaterial;
        laserLight.SetActive(false);
    }

    private void Update ()
    {
        timeSinceHit += Time.deltaTime;
        if (timeSinceHit > timeToCancelHit)//if (timeSinceHit > timeToCancelHit || meshSlicer.isCut)
        {
            if (isActive)
            {
                isActive = false;
                logicProcessor.isPowered = false;
                laserRenderer.material = offMaterial;
                laserLight.SetActive (false);
                OnNotPowered.Invoke();
            }
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=

    public void OnHit ()
    {
        timeSinceHit = 0f;
        if (!isActive)
        {
            OnPowered.Invoke();
            isActive = true;
            logicProcessor.isPowered = true;
            laserRenderer.material = onMaterial;
            laserLight.SetActive(true);
        }
    }

}