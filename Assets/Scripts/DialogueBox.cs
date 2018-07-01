using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueBox : MonoBehaviour 
{
	public static DialogueBox Instance;
	DialogueBox()
	{
		Instance = this;
	}

	public GameObject dialogueBox;
	public Text title;
	public Text dialogue;

	private bool isOpen = false;
	

	public void DisplayDialogue(string t, string d)
	{
		if( isOpen )
			return;

		dialogueBox.SetActive(true);
		dialogue.DOText(d, 3f).SetEase(Ease.Linear);
		title.text = t;
		isOpen = true;
	}

	void Update()
	{
		if (isOpen)
		{
			if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
			{
				isOpen = false;
				dialogue.text = "";
				title.text = "";
				dialogueBox.SetActive(false);
			}
		}
	}
}
