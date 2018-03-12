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
			loadingImage.gameObject.SetActive(true);
			StartCoroutine(RestartScene());
		});
		keywords.Add("Spawn", () =>
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

	IEnumerator RestartScene()
	{
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
