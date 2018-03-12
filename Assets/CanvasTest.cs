using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasTest : MonoBehaviour
{
	public Camera uiCamera;
	private Camera mainCamera;
	private Text label;
	private float deltaTime;
	int w = Screen.width, h = Screen.height;
	
	// Use this for initialization
	void Start ()
	{

		mainCamera = uiCamera;
		label = GetComponentInChildren<Text>();
		GetComponent<Canvas>().worldCamera = mainCamera;

	}

 
	void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
 
		float fps = 1.0f / deltaTime;
		int text = (int)fps;
		label.text = ((int)fps).ToString();
	}
 	
}
