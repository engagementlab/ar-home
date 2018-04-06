/*==============================================================================
Copyright (c) 2018 Engagement Lab @ Emerson College. All Rights Reserved.
by Johnny Richardson
==============================================================================*/

using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class VideoLogic : MonoBehaviour
{
	public GameObject promptText;
	public GameObject videoTexture;

	public bool HasPlayed
	{
		get { return videoPlayed; }
	}

	[HideInInspector]
	public bool videoPlayed;

	public float timeToShowModel;
	
	private AudioSource audioSource;
	private VideoPlayer videoSource;
	private Billboard videoTextureBillboard;
	private GameObject storyObject;

	private float videoCurrentTime;
	private bool videoIsPlaying;

	// Use this for initialization
	void Start ()
	{
		
		promptText.SetActive(false);	
		videoTexture.SetActive(false);
		
		audioSource = videoTexture.GetComponentInChildren<AudioSource>();

	}

	private void Update()
	{
		if(videoIsPlaying)
			videoCurrentTime += Time.deltaTime;

		if (storyObject != null && !storyObject.active)
		{
			if (videoCurrentTime > timeToShowModel)
				storyObject.SetActive(true);
		}
	}

	private void OnDestroy()
	{

		if (videoSource == null) return;
		videoSource.started -= VideoStarted;
		videoSource.loopPointReached -= VideoEnded;
		
	}

	private void VideoStarted(VideoPlayer source)
	{
		videoIsPlaying = true;
	}

	void VideoEnded(VideoPlayer source)
	{
		
		videoTexture.SetActive(false);
		promptText.SetActive(true);
		
		videoPlayed = true;
		videoIsPlaying = false;

	}

	// Called via "Place" command
	public void StartVideo(GameObject storyObjectInstance)
	{
		
		videoSource = GetComponent<VideoPlayer>();
		videoSource.audioOutputMode = VideoAudioOutputMode.AudioSource;
		videoSource.EnableAudioTrack(0, true);
		videoSource.SetTargetAudioSource(0, audioSource);
		
		videoSource.started += VideoStarted;
		videoSource.loopPointReached += VideoEnded;

		storyObject = storyObjectInstance;
		// Hide story object for now
		if(storyObject != null)
			storyObject.SetActive(false);

		// Play vid and show rendering
		videoSource.Play();
		videoTexture.SetActive(true);

	}
}
