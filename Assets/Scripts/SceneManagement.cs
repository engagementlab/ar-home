/*==============================================================================
Copyright (c) 2018 Engagement Lab @ Emerson College. All Rights Reserved.
by Johnny Richardson
==============================================================================*/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HoloToolkit.Sharing.Tests;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManagement : MonoBehaviour
{

	public Image loadingImage;
	public SyncObjectSpawner syncObjectSpawner;
	
	KeywordRecognizer keywordRecognizer = null;
	Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

	// Use this for initialization
	void Start () {
		keywords.Add("Start Over", () =>
		{
			Vuforia.CameraDevice.Instance.Deinit();
			Vuforia.CameraDevice.Instance.Stop();
			Vuforia.TrackerManager.Instance.GetTracker<Vuforia.ObjectTracker>().Stop();
			Vuforia.DigitalEyewearARController.Instance.SetViewerActive(true, true);
			
//			loadingImage.gameObject.SetActive(true);
			StartCoroutine(RestartScene());
		});
		
		keywords.Add("Ok", () =>
		{
			Debug.Log("Place head");
			Events.instance.Raise(new GenericEvent("PlaceHead"));
		});
		
		// Hologram sharing tests
		keywords.Add("Dog bottom", () =>
		{
			syncObjectSpawner.SpawnBasicSyncObject();
			Debug.Log("Spawn object");
		});

		// Tell the KeywordRecognizer about our keywords.
		keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

		// Register a callback for the KeywordRecognizer and start recognizing!
		keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
		keywordRecognizer.Start();

	}

	public void Restart()
	{
		loadingImage.gameObject.SetActive(true);
		StartCoroutine(RestartScene());
	}

	IEnumerator RestartScene()
	{
		Debug.Log(SceneManager.GetActiveScene().name);
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

		//Wait until the last operation fully loads to return anything
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}
	
	private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
	{
		System.Action keywordAction;
		if (keywords.TryGetValue(args.text, out keywordAction))
		{
			keywordAction.Invoke();
		}
	}

}
