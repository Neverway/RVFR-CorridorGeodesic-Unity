//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;

public class Laser_Detector : MonoBehaviour
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

    private Renderer laserRenderer;
    private MeshSlicer meshSlicer;


    //=-----------------=
    // Mono Functions
    //=-----------------=

    private void Start ()
    {
        laserRenderer = GetComponent<Renderer> ();
        meshSlicer = GetComponent<MeshSlicer> ();
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
                laserRenderer.material = offMaterial;
                laserLight.SetActive (false);
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
            isActive = true;
            laserRenderer.material = onMaterial;
            laserLight.SetActive(true);
        }
    }

}