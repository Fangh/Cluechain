using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour 
{
	public string description;

	void OnMouseUp()
	{
		DialogueBox.Instance.DisplayDialogue(name, description);
	}
}
