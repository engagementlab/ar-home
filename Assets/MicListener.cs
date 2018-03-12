using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;

public class MicListener : MonoBehaviour
{

	private KeywordRecognizer keywordRecognizer;
	
	// Use this for initialization
	void Start () {

		// Setup a keyword recognizer to enable resetting the target location.
		List<string> keywords = new List<string>();
		keywords.Add("Reset World");
		keywordRecognizer = new KeywordRecognizer(keywords.ToArray());
		keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
		keywordRecognizer.Start();
	}

	/// <summary>
	/// When the keyword recognizer hears a command this will be called.  
	/// In this case we only have one keyword, which will re-enable moving the 
	/// target.
	/// </summary>
	/// <param name="args">information to help route the voice command.</param>
	private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
