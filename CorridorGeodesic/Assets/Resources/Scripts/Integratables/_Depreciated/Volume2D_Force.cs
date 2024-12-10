//======== Neverway 2023 Project Script | Written by Arthur Aka Liz ===========
// 
// Type: Utility
// Purpose: Remove health from an entity (or add health if the value on
//	the trigger is negative)
// Applied to: A 2D damage trigger
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
/*
public class Volume2D_Force : Volume
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Vector2 forceStrength;
    public float forceStrengthX;
    public float forceStrengthY;
    public float removeEffectDelay;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=

    private void Update()
    {
	    foreach (var entity in pawnsInTrigger)
	    {
		    if (entity) entity.GetComponent<Rigidbody2D>().AddForce(transform.right*forceStrengthX, ForceMode2D.Force);
		    if (entity) entity.GetComponent<Rigidbody2D>().AddForce(transform.up*forceStrengthY, ForceMode2D.Force);
	    }
	    foreach (var prop in propsInTrigger)
	    {
		    if (prop) prop.AssociatedGameObject.GetComponent<Rigidbody2D>().AddForce(transform.right*forceStrengthX, ForceMode2D.Force);
		    if (prop) prop.AssociatedGameObject.GetComponent<Rigidbody2D>().AddForce(transform.up*forceStrengthY, ForceMode2D.Force);
	    }
    }
    private new void OnTriggerExit2D(Collider2D _other)
    {
	    base.OnTriggerExit2D(_other); // Call the base class method
	    if (_other.CompareTag("Pawn"))
	    {
		    if (gameObject.activeInHierarchy) StartCoroutine(RemovePawn(targetEnt));
	    }
	    if(_other.CompareTag("PhysProp") && targetProp)
	    {
		    if (gameObject.activeInHierarchy) StartCoroutine(RemoveProp(targetProp));
	    }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private IEnumerator RemovePawn(Pawn _targetEnt)
    {
	    yield return new WaitForSeconds(removeEffectDelay);
	    pawnsInTrigger.Remove(_targetEnt);
    }
    private IEnumerator RemoveProp(Actor _targetProp)
    {
	    yield return new WaitForSeconds(removeEffectDelay);
	    propsInTrigger.Remove(_targetProp);
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}*/

