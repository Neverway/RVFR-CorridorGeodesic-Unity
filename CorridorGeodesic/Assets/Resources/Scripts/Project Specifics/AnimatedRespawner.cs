//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class AnimatedRespawner : Prop_Respawner
{
    //=-----------------=
    // Public Variables
    //=-----------------=



    //=-----------------=
    // Private Variables
    //=-----------------=

    [SerializeField] private Transform animStartPos;
    [SerializeField] private Transform animEndPos;
    [SerializeField] private float animDuration;
    [SerializeField] private Ease animEaseCurve = Ease.InQuad;
    [SerializeField] private float spawnVelocity;

    //This is the object that will be used for the tween animation, before the real object is spawned in.
    [SerializeField] private GameObject animatedObject;

    [SerializeField] Animator anim;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=

    //=-----------------=
    // Internal Functions
    //=-----------------=

    //=-----------------=
    // External Functions
    //=-----------------=

    protected override IEnumerator SpawnWorker ()
    {
        yield return new WaitForSeconds (spawnDelay);
        anim.SetBool ("Powered", true);
        GameObject animObject = Instantiate (animatedObject, animEndPos);
        animObject.transform.position = animStartPos.position;
        animObject.transform.DOLocalMove (Vector3.zero, animDuration).SetEase (animEaseCurve)
            .OnComplete (() => {
                Destroy (animObject);
                spawnedObject = Instantiate (prefabToSpawn, animEndPos.position, animEndPos.rotation);
                if (spawnedObject.TryGetComponent<Rigidbody> (out var rigidbody))
                {
                    rigidbody.velocity = animEndPos.forward * spawnVelocity;
                }
                if (autoRespawn == false)
                {
                    waitForRespawn = true;
                }
                anim.SetBool ("Powered", false);
            });
        yield return new WaitForSeconds (animDuration);
        spawnWorker = null;
    }
}