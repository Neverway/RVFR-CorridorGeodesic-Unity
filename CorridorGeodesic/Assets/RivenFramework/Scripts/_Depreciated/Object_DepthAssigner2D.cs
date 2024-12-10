//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Neverway.Framework.PawnManagement;

public class Object_DepthAssigner2D : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public int depthLayer; // What layer this object is set to
    public bool assignZDepthToRoot;
    public List<Collider2D> targetedColliders; // The objects to assign layers to
    public List<SpriteRenderer> targetedSpriteRenderers; // The objects to assign layers to


    //=-----------------=
    // Private Variables
    //=-----------------=
    [HideInInspector] public float fallTime;
    [HideInInspector] public bool isFalling;


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        AssignColliders();
        AssignSpriteRenderers();
        if (assignZDepthToRoot) AssignZDepth();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    // Assign 2D Colliders
    private void AssignColliders()
    {
        switch (depthLayer)
        {
            case 0:
            {
                foreach (var collider in targetedColliders) { collider.gameObject.layer = 6; }
                break;
            }
            case 1:
            {
                foreach (var collider in targetedColliders) { collider.gameObject.layer = 7; }
                break;
            }
            case 2:
            {
                foreach (var collider in targetedColliders) { collider.gameObject.layer = 8; }
                break;
            }
        }
    }
    // Assign sprite renderers
    private void AssignSpriteRenderers()
    {
        switch (depthLayer)
        {
            case 0:
            {
                foreach (var spriteRender in targetedSpriteRenderers) { spriteRender.sortingLayerName = "Depth Layer 1"; }
                break;
            }
            case 1:
            {
                foreach (var spriteRender in targetedSpriteRenderers) { spriteRender.sortingLayerName = "Depth Layer 2"; }
                break;
            }
            case 2:
            {
                foreach (var spriteRender in targetedSpriteRenderers) { spriteRender.sortingLayerName = "Depth Layer 3"; }
                break;
            }
        }
    }
    private void AssignZDepth()
    {
        var currentPosition = gameObject.transform.position;
        switch (depthLayer)
        { 
            case 0:
            {
                gameObject.transform.position = new Vector3(currentPosition.x, currentPosition.y, 0);
                break;
            }
            case 1:
            {
                gameObject.transform.position = new Vector3(currentPosition.x, currentPosition.y, -1);
                break;
            }
            case 2:
            {
                gameObject.transform.position = new Vector3(currentPosition.x, currentPosition.y, -2);
                break;
            }
        }
    }
    private IEnumerator FallForSeconds()
    {
        // Exit if already falling
        if (isFalling) yield break;
        isFalling = true;
        
        // Freeze entity movement while falling
        if (gameObject.GetComponent<Pawn>()) { gameObject.GetComponent<Pawn>().isPaused = true; }
        
        // Set temporary mass for consistent falls
        var previousMass = gameObject.GetComponent<Rigidbody2D>().mass;
        gameObject.GetComponent<Rigidbody2D>().mass = 1;
        
        // Set gravity scale
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 10;
        
        // Disable colliders on falling object
        foreach (var collider in targetedColliders) { collider.enabled = false; }
        
        // Wait until finished falling
        yield return new WaitForSeconds(fallTime);
        
        // Reset mass for consistent falls
        gameObject.GetComponent<Rigidbody2D>().mass = previousMass;
        
        // Reset gravity scale
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        
        // Reset colliders
        foreach (var collider in targetedColliders) { collider.enabled = true; }
        
        // Unfreeze entity movement
        if (gameObject.GetComponent<Pawn>()) { gameObject.GetComponent<Pawn>().isPaused = false; }
        
        isFalling = false;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void Fall()
    {
        StartCoroutine(FallForSeconds());
    }
}
