//======== Neverway 2023 Project Script | Written by Arthur Aka Liz ===========
// 
// Type: 
// Purpose: 
// Applied to: 
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neverway.Framework;
/*
public class Object_LiveThrowable : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    private bool explodesWhenTouchingEntity;
    private bool alreadyExploded; // This is a failsafe to keep the object from exploding twice (which seems to happen if an entity contact-explodes the object before it's lifetime expires)
    
    
    //=-----------------=
    // Reference Variables
    //=-----------------=
    public Item_Utility_Throwable throwable;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        StartCoroutine(ActivateEntityTriggeredExplosions());
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private IEnumerator ActivateEntityTriggeredExplosions()
    {
        yield return new WaitForSeconds(0.05f);
        explodesWhenTouchingEntity = true;
    }

    private void Explode()
    {
        if (alreadyExploded) return;
        alreadyExploded = true;
        if (!throwable.hitObject)
        {
            Destroy(gameObject,0.1f); // wait a small fraction of a second so that the damage trigger hits the target before being destroyed
        }
        else
        {
            var liveItem = Instantiate(throwable.hitObject, transform.position, new Quaternion());
            liveItem.GetComponent<Object_DepthAssigner2D>().depthLayer = GetComponent<Object_DepthAssigner2D>().depthLayer;
            Destroy(liveItem, 1f);
            Destroy(gameObject);
        }
    }

    
    
    //=-----------------=
    // External Functions
    //=-----------------=
    public IEnumerator ActivateSubAction(float time)
    {
        yield return new WaitForSeconds(time);
        Explode(); // This is called, but the object is destroyed
    }

    public void EntityContactExplode()
    {
        if (!explodesWhenTouchingEntity) return;
        StopAllCoroutines();
        Explode();
    }
}
*/
