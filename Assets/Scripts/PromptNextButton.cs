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

	public void AdvancePrompt()
	{
		promptText[promptIndex].gameObject.SetActive(false);
		promptIndex++;

		if (promptIndex == promptText.Length)
		{
			transform.parent.gameObject.SetActive(false);
			return;
		}
		
		promptText[promptIndex].gameObject.SetActive(true);
	}
	
}
