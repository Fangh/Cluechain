using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marketplace : MonoBehaviour 
{
	public static Marketplace Instance;

	Marketplace()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void Open()
	{
		gameObject.SetActive(true);
	}
	
	public void Close()
	{
		gameObject.SetActive(false);
	}
}