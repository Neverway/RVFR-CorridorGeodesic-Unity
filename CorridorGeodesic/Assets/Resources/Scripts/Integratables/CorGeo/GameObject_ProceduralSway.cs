//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Add some procedural bobbing and swaying to things like weapons for when a pawn looks around
// Notes: This is based off the tutorial by BuffaTwo
// Source: https://youtu.be/DR4fTllQnXg
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neverway.Framework.PawnManagement;

namespace Neverway.Framework
{
    public class GameObject_ProceduralSway : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        [SerializeField] private bool sway = true;
        [SerializeField] private bool bob = true;
        [SerializeField] private float smoothingPosition = 10f;
        [SerializeField] private float smoothingRotation = 12f;
        [Header("Sway")]
        [SerializeField] private float swayPositionStep = 0.01f;
        [SerializeField] private float swayRotationStep = 1f;
        [SerializeField] private float maxSwayDistance = 0.06f;
        [SerializeField] private float maxSwayRotation = 5f;


        //=-----------------=
        // Private Variables
        //=-----------------=
        private Vector2 moveInput;
        private Vector2 lastLook;
        private Vector2 currentLook;
        private Vector2 lookInput;
        private Vector3 swayPosition;
        private Vector3 swayRotation;
        private bool hasInitialized; // A delay value to make sure that things like ropes that may be attached have time to get their position references


        //=-----------------=
        // Reference Variables
        //=-----------------=
        //[SerializeField] private Rigidbody positionReference;
        [SerializeField] private Pawn pawn;
        [SerializeField] private PlayerController_FirstPersonShooter pawnController;


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void Start()
        {
        }

        private void Update()
        {
            GetInput();
            Sway();
            Bob();
        }

        //=-----------------=
        // Internal Functions
        //=-----------------=
        
        private void GetInput()
        {
            if (pawn.currentController as PlayerController_FirstPersonShooter)
            {
                pawnController = pawn.currentController as PlayerController_FirstPersonShooter;
                currentLook = new Vector2(pawnController.yRotation, pawnController.xRotation); // Don't ask me why we have to swap these axis, but if we don't, the rotations don't match ~Liz
                lookInput = currentLook - lastLook;
                lastLook = currentLook;
            }
        }
        
        private void Sway()
        {
            if (!sway)
            {
                swayPosition = Vector3.zero;
                return;
            }
            // Sway position
            Vector3 invertLook = lookInput * -swayPositionStep;
            invertLook.x = Mathf.Clamp(invertLook.x, -maxSwayDistance, maxSwayDistance);
            invertLook.y = Mathf.Clamp(invertLook.y, -maxSwayDistance, maxSwayDistance);
            swayPosition = invertLook;
            // Sway Rotation
            invertLook = lookInput * -swayRotationStep;
            invertLook.x = Mathf.Clamp(invertLook.x, -maxSwayRotation, maxSwayRotation);
            invertLook.y = Mathf.Clamp(invertLook.y, -maxSwayRotation, maxSwayRotation);
            swayRotation = new Vector3(invertLook.y, invertLook.x, invertLook.x);
            // Apply movement & rotation
            transform.localPosition = 
                Vector3.Lerp(transform.localPosition, swayPosition, Time.deltaTime * smoothingPosition);
            transform.localRotation = 
                Quaternion.Slerp(transform.localRotation, Quaternion.Euler(swayRotation), Time.deltaTime*smoothingRotation);
        }

        private void Bob()
        {
            if (!bob)
            {
                swayPosition = Vector3.zero;
                return;
            }
        }


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}
