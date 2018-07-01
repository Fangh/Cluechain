using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class Inventory : MonoBehaviour 
{
	public static Inventory Instance;

	Inventory()
	{
		Instance = this;
	}

	[Header("References")]
	public Text descriptionText;
	public InventoryItem[] slots; 
	public string playerPrivateKey = "1";

	[Header("Tweaking")]
	public float openingOffset = 480;

	[Header("Private")]
	public bool isOpen = false;
	private bool isLocked = false;
	public List<Item> items = new List<Item>();
	public static string URL = "http://192.168.203.101:8085";
	public string playerAdress;
	public bool gameIsPaused = false;

	// Use this for initialization
	void Start () 
	{
		importAccountFromPrivateKey();


		slots = GetComponentsInChildren<InventoryItem>();
		// Item i = Item.CreateInstance("adress", "moi", "book", "bonjour");
		// AddItem(i);
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void TogglePause( bool toggle )
	{
		gameIsPaused = toggle;
	}

	public void importAccountFromPrivateKey () 
	{
		// Here we try to get the public address from the secretKey we defined
		try 
		{
			var address = Nethereum.Signer.EthECKey.GetPublicAddress (playerPrivateKey);
			// Then we define the accountAdress private variable with the public key
			playerAdress = address;
			Debug.Log("you are playing with this adress: " + playerAdress);
		} 
		catch (Exception e) 
		{
			// If we catch some error when getting the public address we just display the exception in the console
			Debug.Log("Error importing account from PrivateKey: " + e);
		}
	}

	public void AddItem(Item i)
	{
		Open();
		items.Add(i);
		int order = items.IndexOf(i);
		slots[order].Initialize(i);
	}
	
	public void RemoveItem(Item i)
	{
		int order = items.IndexOf(i);
		Debug.Log("items" + i.name + " is in order "+order+ " in you inventory");
		slots[order].Empty();
		items.Remove(i);

		for (int j = order; j < slots.Length-1; j++)
		{
			slots[j].myItem = slots[j+1].myItem;
			slots[j].Initialize(slots[j].myItem);
		}
	}

	public void SetDescription(string des)
	{
		descriptionText.text = des;
	}

	public void Toggle()
	{
		if ( isLocked )
			return;

		if ( isOpen )
		{
			isLocked = true;
			transform.DOLocalMoveX(transform.localPosition.x - openingOffset, 1f).SetEase(Ease.InBack).OnComplete( () => {
				isOpen = false;
				isLocked = false;
				Inventory.Instance.TogglePause(false);
			});
		}
		else
		{
			isLocked = true;
			transform.DOLocalMoveX(transform.localPosition.x + openingOffset, 1f).SetEase(Ease.OutBack).OnComplete( () => {
				isOpen = true;
				isLocked = false;
				Inventory.Instance.TogglePause(true);
			});
		}		
	}

	public void Open()
	{
		if (!isOpen)
		{
			Toggle();
		}
	}

	public void Close()
	{
		if (isOpen)
			Toggle();
	}
}