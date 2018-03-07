using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.Windows.Speech;

public class VideoLogic : MonoBehaviour
{
	public VideoPlayer videoPlayer;
	public GameObject videoPlayerTexture;
	public GameObject playButton;
	public GameObject restartButton;
	public GameObject stopButton;
	public GameObject promptText;

	public bool videoPlayed;
	
	private VideoPlayer _video;

	// Use this for initialization
	void Start ()
	{

		videoPlayer.loopPointReached += ShowPrompt;

	}

	void ShowPrompt(VideoPlayer source)
	{
		restartButton.SetActive(true);
		promptText.SetActive(true);
		
		videoPlayerTexture.SetActive(false);
		stopButton.SetActive(false);

		videoPlayed = true;

	}
	
	public void StopVideo()
	{
		videoPlayerTexture.SetActive(false);
		stopButton.SetActive(false);
		
		if (videoPlayed)
		{
			restartButton.SetActive(true);
			promptText.SetActive(true);
			GetComponent<VideoTagalong>().SphereRadius = 4;
		}
		else
		{
			playButton.SetActive(true);
			GetComponent<VideoTagalong>().VideoStop();
		}
	}
}
