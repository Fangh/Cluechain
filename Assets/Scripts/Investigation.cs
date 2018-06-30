using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Investigation : MonoBehaviour 
{
	public Item[] itemsNeeded;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public bool Resolve( Item[] itemsToTry )
	{
		if (itemsToTry.Length != itemsNeeded.Length)
		{
			Debug.Log("Please try with " + itemsNeeded.Length + " items");
			return false;
		}
		for (int i = 0; i < itemsToTry.Length; i++)
		{
			if ( itemsToTry[i].adress != itemsNeeded[i].adress )
			{
				Debug.Log("on of the items is not correct");
				return false;
			}
		}
		return true;
	}
}
