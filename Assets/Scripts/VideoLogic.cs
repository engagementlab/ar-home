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

	public bool videoPlayed;
	
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
		
		// Create video texture
/*		videoTexture = Instantiate(Resources.Load<GameObject>("VideoTexture"), Vector3.zero, Quaternion.identity);
		videoTexture.transform.parent = transform;
//		videoTexture.transform.position = new Vector3(0, 0, .2f);
		videoTexture.transform.localRotation = Quaternion.identity;
		videoTexture.SetActive(false);*/
		audioSource = videoTexture.GetComponentInChildren<AudioSource>();

	}

	private void Update()
	{
		if(videoIsPlaying)
			videoCurrentTime += Time.deltaTime;

		if (sampleModel != null)
		{
			if (videoCurrentTime > 5 && !sampleModel.active)
				sampleModel.SetActive(true);
		}
	}

	private void OnDestroy()
	{

		if (videoSource == null) return;
		videoSource.started -= VideoStarted;
		videoSource.loopPointReached -= ShowPrompt;
		
	}

	private void VideoStarted(VideoPlayer source)
	{
		// Disable billboard when watching
//		GetComponent<Billboard>().enabled = false;
	
		videoIsPlaying = true;
	}

	void ShowPrompt(VideoPlayer source)
	{
		
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
		videoSource.loopPointReached += ShowPrompt;

		videoSource.Play();
		videoTexture.SetActive(true);
		/*videoTextureBillboard = videoTexture.AddComponent<Billboard>();
        videoTextureBillboard.PivotAxis = PivotAxis.Y;
        videoTextureBillboard.enabled = false;*/
		
//		GetComponent<VideoTagalong>().VideoStart();

	}
	
	public void StopVideo()
	{
		Destroy(videoTexture);
		
		if (videoPlayed)
		{
			promptText.SetActive(true);
//			GetComponent<VideoTagalong>().SphereRadius = 4;
		}
//		else
//			GetComponent<VideoTagalong>().VideoStop();
	
	}
}
