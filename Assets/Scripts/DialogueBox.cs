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

	public bool isOpen = false;
	private bool textIsDisplayed = false;

	void Start()
	{
		if (isOpen)
			textIsDisplayed = true;
	}
	

	public void DisplayDialogue(string t, string d)
	{
		if( isOpen )
			return;

		dialogueBox.SetActive(true);
		dialogue.DOText(d, 3f).SetEase(Ease.Linear).OnComplete( () => { 
			textIsDisplayed = true;
		});
		isOpen = true;
		Inventory.Instance.TogglePause(true);

		title.text = t;
	}

	void Update()
	{
		if (isOpen && textIsDisplayed)
		{
			if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
			{
				Inventory.Instance.TogglePause(false);
				isOpen = false;
				textIsDisplayed = false;
				dialogue.text = "";
				title.text = "";
				dialogueBox.SetActive(false);
			}
		}
	}
}
