using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour 
{
	public string description;

	void OnMouseDown()
	{
		if (!DialogueBox.Instance.isOpen)
			DialogueBox.Instance.DisplayDialogue(name, description);
	}
}
