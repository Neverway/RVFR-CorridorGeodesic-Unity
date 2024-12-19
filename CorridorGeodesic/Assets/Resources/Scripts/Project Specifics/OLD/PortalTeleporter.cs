//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes: Created following the portal rendering tutorial by Brackeys
// Source: https://www.youtube.com/watch?v=cuQao3hEKfs
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
	//=-----------------=
	// Public Variables
	//=-----------------=

	
	//=-----------------=
	// Private Variables
	//=-----------------=
	private bool isOverlapping = false;


	//=-----------------=
	// Reference Variables
	//=-----------------=
	public Transform targetObject;
	public Transform reciever;


	//=-----------------=
	// Mono Functions
	//=-----------------=
	void Update () {
		if (isOverlapping)
		{
			Vector3 portalToPlayer = targetObject.position - transform.position;
			float dotProduct = Vector3.Dot(transform.up, portalToPlayer);
			// If this is true: The player has moved across the portal
			if (dotProduct < 0f)
			{
				// Teleport him!
				float rotationDiff = -Quaternion.Angle(transform.rotation, reciever.rotation);
				rotationDiff += 180;
				targetObject.Rotate(Vector3.up, rotationDiff);

				Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
				targetObject.position = reciever.position + positionOffset;

				isOverlapping = false;
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		//if (other.tag == "Player")
		{
			targetObject = other.gameObject.transform;
			isOverlapping = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		//if (other.tag == "Player")
		{
			targetObject = null;
			isOverlapping = false;
		}
	}
	

	//=-----------------=
	// Internal Functions
	//=-----------------=


	//=-----------------=
	// External Functions
	//=-----------------=
}

