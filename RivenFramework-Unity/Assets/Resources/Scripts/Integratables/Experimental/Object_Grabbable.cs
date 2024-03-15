//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Grabbable : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    public bool isHeld;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    public Pawn targetPawn;
    public Vector3 lastFaceDirection;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        if (!isHeld || !targetPawn)
        {
            GetComponent<Trigger2D_Interactable>().hideIndicator=false;
            return;
        }
        GetComponent<Trigger2D_Interactable>().hideIndicator=true;
        transform.parent.position = targetPawn.transform.position + GetTargetPawnOffset();
        transform.parent.GetComponent<Object_DepthAssigner>().depthLayer = targetPawn.GetComponent<Object_DepthAssigner>().depthLayer;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private Vector3 GetTargetPawnOffset()
    {
        var facingDirection = GetFaceDirectionFromQuaternion(targetPawn.faceDirection, -90);
        switch (facingDirection.y)
        {
            case (1):
                lastFaceDirection = new Vector3(0, 0.25f, 0);
                return lastFaceDirection;
            case (-1):
                lastFaceDirection = new Vector3(0, -0.3f, 0);
                return lastFaceDirection;
        }
        
        switch (facingDirection.x)
        {
            case (1):
                lastFaceDirection = new Vector3(0.5f, 0, 0);
                return lastFaceDirection;
            case (-1):
                lastFaceDirection = new Vector3(-0.5f, 0, 0);
                return lastFaceDirection;
        }
        return lastFaceDirection;
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        var interaction = _other.GetComponent<Trigger2D_Interaction>();
        if (interaction)
        {
            targetPawn = interaction.targetPawn;
            ToggleHeld();
        }
    }
    
    public Vector2 GetFaceDirectionFromQuaternion(Quaternion _rotation, int _zRotationOffset)
    {
        // Extract the z-axis rotation from the Quaternion
        float angle = _rotation.eulerAngles.z - _zRotationOffset;

        // Convert the angle back to radians since Unity's trig functions expect radians
        float angleInRadians = angle * Mathf.Deg2Rad;

        // Calculate the x and y components of the facing direction
        float x = Mathf.Cos(angleInRadians);
        float y = Mathf.Sin(angleInRadians);

        // Create and return the facing direction vector
        Vector2 facingDirection = new Vector2(x, y);

        return facingDirection;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    private void ToggleHeld()
    {
        isHeld = !isHeld;
    }
}
