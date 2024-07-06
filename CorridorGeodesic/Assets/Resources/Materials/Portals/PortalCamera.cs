//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
	//=-----------------=
	// Public Variables
	//=-----------------=


	//=-----------------=
	// Private Variables
	//=-----------------=


	//=-----------------=
	// Reference Variables
	//=-----------------=
	private CameraManager cameraManager;
	private Transform playerCamera;
	[SerializeField] private Transform portal;
	[SerializeField] private Transform otherPortal;


	//=-----------------=
	// Mono Functions
	//=-----------------=
	private void Start()
	{
		cameraManager = FindObjectOfType<CameraManager>();
	}

	private void Update()
	{
		if (!playerCamera)
		{
			playerCamera = cameraManager.GetActiveRenderingCamera().gameObject.transform;
			return;
		}
		
		Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;
		transform.position = portal.position + playerOffsetFromPortal;

		float angularDifferenceBetweenPortalRotations = Quaternion.Angle(portal.rotation, otherPortal.rotation);

		Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
		Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
		transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
	}

	
	//=-----------------=
	// Internal Functions
	//=-----------------=


	//=-----------------=
	// External Functions
	//=-----------------=
}

