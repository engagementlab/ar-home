/*==============================================================================
Copyright (c) 2018 Engagement Lab @ Emerson College. All Rights Reserved.
by Johnny Richardson
==============================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positioning : MonoBehaviour
{
	private Vector3 initalVector3;

	private Transform target;
	// Use this for initialization
	void Start ()
	{

		initalVector3 = transform.position;
		target = Camera.main.transform;
//
//		// Calculate the current rotation angles
//		float wantedRotationAngle = target.eulerAngles.y;
//		float wantedHeight = target.position.y + 1;
//
//		float currentRotationAngle = transform.eulerAngles.y;
//		float currentHeight = transform.position.y;
//
//		// Damp the rotation around the y-axis
//		currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, 3 * Time.deltaTime);
//	
//		// Damp the height
//		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, 2 * Time.deltaTime);
//
//		// Convert the angle into a rotation
//		var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
//
//
//		transform.position = target.position;
//		transform.position -= currentRotation * Camera.main.transform.position - (Vector3.forward * 2);
//		//transform.LookAt(target);
	}
	
	// Update is called once per frame
	void Update () {
	
		
		// Do a raycast into the world that will only hit the Spatial Mapping mesh.
		var headPosition = Camera.main.transform.position;
		var gazeDirection = Camera.main.transform.forward;

		RaycastHit hitInfo;
		// Move this object's parent object to
		// where the raycast hit the Spatial Mapping mesh.
		this.transform.position  = headPosition + (gazeDirection * 2);

		// Rotate this object's parent object to face the user.
		Quaternion toQuat = Camera.main.transform.localRotation;
		toQuat.y = 0;
		toQuat.z = 0;
	
	}
}
