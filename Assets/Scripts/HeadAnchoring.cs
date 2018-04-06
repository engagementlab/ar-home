/*==============================================================================
Copyright (c) 2018 Engagement Lab @ Emerson College. All Rights Reserved.
by Johnny Richardson
==============================================================================*/

using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class HeadAnchoring : MonoBehaviour
{
	public string EventName
	{
		get { return eventName; }
		set { eventName = value; }
	}

	private Camera mainCamera;
	private GestureRecognizer gestureRecognizer;
	private string eventName;

	// Use this for initialization
	void Start () {
		
		mainCamera = Camera.main;

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 playerPos = mainCamera.transform.position;
		Vector3 playerDirection = mainCamera.transform.forward;
        
		transform.rotation = mainCamera.transform.rotation;

		transform.position =
			Vector3.Slerp(transform.position, playerPos + (playerDirection * 1.5f), 1.5f * Time.deltaTime);
	}

	private void Awake()
	{
		if (gestureRecognizer != null) return;
	
		gestureRecognizer = new GestureRecognizer();
		gestureRecognizer.TappedEvent += OnAirTap;
		gestureRecognizer.StartCapturingGestures();
	}

	private void OnDisable()
	{
		
		if (gestureRecognizer == null) return;
		gestureRecognizer.TappedEvent -= OnAirTap;
		
	}

	void OnDestroy()
	{
	
		if (gestureRecognizer == null) return;
		gestureRecognizer.TappedEvent -= OnAirTap;
		gestureRecognizer.StopCapturingGestures();
		
	}

	private void OnAirTap(InteractionSourceKind src, int count, Ray headRay)
	{
		
		Events.instance.Raise(new GenericEvent(eventName));
		
	}
}
