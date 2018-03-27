/*==============================================================================
Copyright (c) 2018 Engagement Lab @ Emerson College. All Rights Reserved.
by Johnny Richardson
==============================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.Buttons;

public class PromptNextButton : MonoBehaviour
{

	public TextMesh[] promptText;
	private CompoundButton _compoundButton;
	private int promptIndex;

	// Use this for initialization
	void Start ()
	{

	}

	public void AdvancePrompt()
	{
		promptText[promptIndex].gameObject.SetActive(false);
		promptIndex++;

		if (promptIndex == promptText.Length - 1)
		{
			transform.parent.gameObject.SetActive(false);
			return;
		}
		
		promptText[promptIndex].gameObject.SetActive(true);
	}
	
}
