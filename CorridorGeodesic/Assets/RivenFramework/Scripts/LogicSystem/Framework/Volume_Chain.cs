//===================== (Neverway 2024) Written by Errynei ===================
//
// Purpose: Activate a prefab at the end of a chain. Used in the CorGeo project
//  to connect fluids and phys fields through pipes.
// Notes: Chain triggers should sit infront of one another and spawn something
//  at the end of a chain, if the chain is broken from the source, it will no
//  longer spawn something at the end
//
//=============================================================================

using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using Neverway.Framework.LogicSystem;
using UnityEngine.Rendering;

namespace Neverway.Framework.LogicSystem
{
    public class Volume_Chain : Volume
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        [Tooltip("Chains only spawn something at the end if they are connected to a source, set this to true if it's the first volume in the chain")]
        [SerializeField] private bool isSourceChain;
        [Tooltip("Set this to true if this chain is the last link and you don't want it to spawn anything when it's connected")]
        [SerializeField] private bool absorbsSource;
        [Tooltip("How far out to check for the next chain in the line")]
        [SerializeField] private float raycastDistance;
        [Tooltip("")]
        [SerializeField] private LayerMask layerMask;

        [SerializeField] private float sourceObjectForwardPositionOffset;
        [SerializeField] private Vector3 sourceObjectScale=Vector3.one;

        
        //=-----------------=
        // Private Variables
        //=-----------------=
        [ReadOnly] public bool connectedToSource;
        [ReadOnly] public GameObject instantiatedSourceObject;
        

        //=-----------------=
        // Reference Variables
        //=-----------------=
        [SerializeField] private Transform raycastOrigin;
        [Tooltip("The type of prefab to instantiate at the end of the source")]
        [SerializeField] public GameObject sourceObjectPrefab;


        //=-----------------=
        // Mono Functions
        //=-----------------=


        //=-----------------=
        // Internal Functions
        //=-----------------=
        private new void Awake()
        {
            base.Awake();
        }

        private void Update()
        {
            
            // If there is another link ahead...
            if (GetNextLinkInChain())
            {
                var nextLink = GetNextLinkInChain();
                
                // ...and we are the source or connected to the source
                if (isSourceChain || connectedToSource)
                {
                    // Flow the source object to the next link, and ensure the next link knows it's connected now
                    nextLink.connectedToSource = true;
                    nextLink.sourceObjectPrefab = sourceObjectPrefab;
                    nextLink.sourceObjectForwardPositionOffset = sourceObjectForwardPositionOffset;
                    nextLink.sourceObjectScale = sourceObjectScale;
                    ClearInstantiatedSourceObject();
                }
                // ... and we are not the source or connected to it
                else
                {
                    // and the next link is not another source
                    if (!nextLink.isSourceChain)
                    {
                        // clear the source object to the next link, and ensure the next link knows it's not connected now
                        nextLink.connectedToSource = false;
                        nextLink.sourceObjectPrefab = null;
                        nextLink.sourceObjectForwardPositionOffset = 0;
                        nextLink.sourceObjectScale = Vector3.one;
                    }
                    ClearInstantiatedSourceObject();
                }
            }
            // If there is no other link ahead...
            else
            {
                // ...and we are the source or connected to the source...
                if (isSourceChain || connectedToSource)
                {
                    // ...and we haven't created the source object
                    if (!instantiatedSourceObject && !absorbsSource)
                    {
                        // Spawn the source object at the end of our raycast
                        instantiatedSourceObject = Instantiate(sourceObjectPrefab, raycastOrigin.position+(raycastOrigin.forward*raycastDistance)+(raycastOrigin.forward*sourceObjectForwardPositionOffset), raycastOrigin.rotation);
                        instantiatedSourceObject.transform.localScale = sourceObjectScale;
                        instantiatedSourceObject.transform.parent = raycastOrigin;
                    }
                    
                }
            }

            // Clear source objects if link to source is lost
            if (!connectedToSource && !isSourceChain)
            {
                ClearInstantiatedSourceObject();
            }
            
            // Reset check value for if we are connected to the source
            connectedToSource = false;
        }

        private Volume_Chain GetNextLinkInChain()
        {
            Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, layerMask))
            {
                // If we are connected to the next link
                return hit.collider.GetComponent<Volume_Chain>();
            }

            return null;
        }

        private void ClearInstantiatedSourceObject()
        {
            if (instantiatedSourceObject)
            {
                Destroy(instantiatedSourceObject);
                instantiatedSourceObject = null;
            }
        }

        //=-----------------=
        // External Functions
        //=-----------------=
    }
}