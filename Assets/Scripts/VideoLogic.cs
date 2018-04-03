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

	public GameObject sampleModel;

	[HideInInspector]
	public bool videoPlayed;

	public float timeToShowModel;
	
	private AudioSource audioSource;
	private VideoPlayer videoSource;
	private Camera mainCamera;
	private Billboard videoTextureBillboard;

	private float videoCurrentTime;
	private bool videoIsPlaying;

	// Use this for initialization
	void Start ()
	{
		
		mainCamera = Camera.main;
		promptText.SetActive(false);	
		
		if(sampleModel != null)
			sampleModel.SetActive(false);
		
		// Create video texture (testing)
	/*	videoTexture = Instantiate(Resources.Load<GameObject>("VideoTexture"), Vector3.zero, Quaternion.identity);
		videoTexture.transform.parent = transform;
		
		videoTexture.transform.localRotation = Quaternion.identity;*/
		videoTexture.SetActive(false);
		audioSource = videoTexture.GetComponentInChildren<AudioSource>();

	}

	private void Update()
	{
		if(videoIsPlaying)
			videoCurrentTime += Time.deltaTime;

		if (sampleModel != null && !sampleModel.active)
		{
			if (videoCurrentTime > timeToShowModel)
				sampleModel.SetActive(true);
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

	// Called via "Ok" command
	public void StartVideo()
	{
		
		videoSource = GetComponent<VideoPlayer>();
		videoSource.audioOutputMode = VideoAudioOutputMode.AudioSource;
		videoSource.EnableAudioTrack(0, true);
		videoSource.SetTargetAudioSource(0, audioSource);
		
		videoSource.started += VideoStarted;
		videoSource.loopPointReached += VideoEnded;

		// Play vid and show rendering
		videoSource.Play();
		videoTexture.SetActive(true);

	}
}
