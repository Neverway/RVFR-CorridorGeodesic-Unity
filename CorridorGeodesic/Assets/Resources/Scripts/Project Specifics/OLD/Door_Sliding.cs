//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;

public class Door_Sliding : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=

    [SerializeField] Transform doorLeft;
    [SerializeField] Transform doorRight;
    [SerializeField] private bool open;

    private float openTimer;
    [SerializeField] private float timeToOpenSeconds = 0.6f;

    [SerializeField] private float xDistance = 1f;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=

    private void Start ()
    {

    }

    private void Update ()
    {
        if (open)
        {
            openTimer = Mathf.Clamp (openTimer + Time.deltaTime, 0, timeToOpenSeconds);
        }
        else
        {
            openTimer = Mathf.Clamp (openTimer - Time.deltaTime, 0, timeToOpenSeconds);
        }

        float p = openTimer / timeToOpenSeconds;

        doorLeft.transform.localPosition = Vector3.Lerp (Vector3.zero, Vector3.left * xDistance, p);
        doorRight.transform.localPosition = Vector3.Lerp (Vector3.zero, Vector3.right * xDistance, p);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}