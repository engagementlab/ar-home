using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogic : MonoBehaviour
{

	public AudioClip pingSound;
	private AudioSource audioSource;

	float audioStartTime;
	float audioDelayTime = 2;
	
	// Use this for initialization
	void Start () {
		
		// If this object does not already have an AudioSource component, then add one.
		audioSource = gameObject.GetComponent<AudioSource>();
		if (audioSource == null)
		{
			// Add an AudioSource and spatialize it.
			audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = true;
			audioSource.spatialize = true;
			audioSource.spatialBlend = 1.0f;
			audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
		}

		audioSource.loop = false;
		audioSource.clip = pingSound;
		audioSource.Play();

		audioStartTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		
		if ((Time.time - audioStartTime) >= audioDelayTime)
		{
			audioSource.Play();
			audioStartTime = Time.time;
		}
		
	}
}
