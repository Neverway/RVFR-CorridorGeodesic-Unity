//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Simulate parallax scrolling on a tilemap for use with orthographic cameras.
// Notes: This script adjusts the scale and position of the tilemap based on the camera's movement.
//
//=============================================================================


using UnityEngine;

public class Tile_Paralax : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField] private Vector2 parallaxAmount;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Vector3 parallaxScale;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private GameInstance gameInstance;
    private Camera currentCamera;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        gameInstance = FindObjectOfType<GameInstance>();
        // Calculate the new local scale based on the parallax amount
        originalPosition = transform.position;
        originalScale = transform.localScale;
        parallaxScale = new Vector3(originalScale.x + (parallaxAmount.x/2), originalScale.y + (parallaxAmount.y/2), originalScale.z);
    }

    private void Update()
    {
        currentCamera = FindObjectOfType<Camera>(false);
        if (!currentCamera) return;
        
        // Only set the scale to the parallax scale if the correct gamemode is active
        transform.localScale = gameInstance.GetCurrentGamemode().Contains("Topdown2D") ? parallaxScale : originalScale;
        if (!gameInstance.GetCurrentGamemode().Contains("Topdown2D"))
        {
            transform.position = originalPosition;
            return;
        }
        
        // Calculate the distance to move based on the parallax amount
        Vector3 position = currentCamera.transform.position;
        Vector2 distance = new Vector2(position.x * -parallaxAmount.x, position.y * -parallaxAmount.y);
        Vector3 newPosition = new Vector3(distance.x, distance.y, originalPosition.z);

        // Set the new position
        transform.position = newPosition;
    }

    
    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}