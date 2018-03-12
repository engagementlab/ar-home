using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class PhysicsTest : MonoBehaviour, IInputHandler {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnInputClicked(InputClickedEventData eventData)
	{
        return;
	}

	public void OnInputDown(InputEventData eventData)
	{
		// On each tap gesture, toggle whether the user is in placing mode.
		this.GetComponent<Rigidbody>().isKinematic = false;
	}

	public void OnInputUp(InputEventData eventData)
	{
        return;
	}
}
