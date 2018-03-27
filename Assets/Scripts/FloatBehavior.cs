/*==============================================================================
Copyright (c) 2018 Engagement Lab @ Emerson College. All Rights Reserved.
by Johnny Richardson
==============================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatBehavior : MonoBehaviour {

	private Vector3 velocity = Vector3.zero;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{

		Vector3 toPosRandom = Random.insideUnitSphere * Random.Range(-3, 3);
		transform.position = Vector3.SmoothDamp(transform.position, transform.position + toPosRandom, ref velocity, 5);
		
	}
}
