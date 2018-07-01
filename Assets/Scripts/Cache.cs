using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache : MonoBehaviour 
{
	public string adressOfObjectWanted = "";
	public Item generatedItem;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnMouseDown()
	{
		if (!Inventory.Instance.gameIsPaused)
		{
			generatedItem = ItemGeneratorContract.Instance.GetItemByAdress(adressOfObjectWanted);
			Inventory.Instance.AddItem(generatedItem);
			GetComponent<BoxCollider2D>().enabled = false;			
		}
	}
}