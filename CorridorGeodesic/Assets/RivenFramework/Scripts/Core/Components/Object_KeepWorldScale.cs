//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: If an object is the child of another object that changes scale
//  this component will make sure the child keeps their original scale
// Notes:
//  Fixed this function using Seneral's post here: https://discussions.unity.com/t/reading-and-setting-an-objects-global-scale-with-transform-functions/143857
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neverway.Framework
{
    public class Object_KeepWorldScale : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        public bool overrideInitialScale;


        //=-----------------=
        // Private Variables
        //=-----------------=
        public Vector3 initialScale;
        private Vector3 lastScale;


        //=-----------------=
        // Reference Variables
        //=-----------------=


        //=-----------------=
        // Mono Functions
        //=-----------------=
        void Start()
        {
            // Record the initial scale of the sprite
            if (!overrideInitialScale) initialScale = transform.localScale;
        }

        void LateUpdate()
        {
            if (transform.localScale != lastScale)
            {
                lastScale = transform.localScale;
                SetGlobalScale(initialScale);
            }
            /*
            // Get the parent's scale
            Vector3 parentScale = transform.parent.localScale;

            // Calculate the inverse scale factor
            Vector3 inverseScale = new Vector3(
                1f / parentScale.x,
                1f / parentScale.y,
                1f / parentScale.z
            );

            // Apply the inverse scale to the sprite
            transform.localScale = Vector3.Scale(initialScale, inverseScale);*/
        }

        //=-----------------=
        // Internal Functions
        //=-----------------=
        public void SetGlobalScale (Vector3 globalScale)
        {
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3 (globalScale.x/transform.lossyScale.x, globalScale.y/transform.lossyScale.y, globalScale.z/transform.lossyScale.z);
        }


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}