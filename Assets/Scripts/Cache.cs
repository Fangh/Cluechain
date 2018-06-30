using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache : MonoBehaviour 
{
	public Item generatedItem;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnMouseUp()
	{
		Inventory.Instance.AddItem(generatedItem);
		GetComponent<BoxCollider2D>().enabled = false;
	}
}