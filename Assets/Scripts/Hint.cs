using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour 
{
	public string description;

	void OnMouseDown()
	{
		if (!Inventory.Instance.gameIsPaused)
			DialogueBox.Instance.DisplayDialogue(name, description);
	}
}
