using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Inventory : MonoBehaviour 
{
	public static Inventory Instance;

	Inventory()
	{
		Instance = this;
	}

	[Header("References")]
	public GameObject toggleButton;
	public Text descriptionText;
	public InventoryItem[] slots; 
	public string playerAdress;
	public string playerPrivateKey;

	[Header("Tweaking")]
	public float openingOffset;

	[Header("Private")]
	private bool isOpen = false;
	private bool isLocked = false;
	public List<Item> items = new List<Item>();
	public static string URL = "http://192.168.86.31:8085";

	// Use this for initialization
	void Start () 
	{
		slots = GetComponentsInChildren<InventoryItem>();
		Item i = Item.CreateInstance("adress", "moi", "book", "bonjour");
		AddItem(i);
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void AddItem(Item i)
	{
		Open();
		items.Add(i);
		int order = items.IndexOf(i);
		slots[order].Initialize(i);
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
			toggleButton.transform.DOScaleY(-1, 1f);
			transform.DOLocalMoveY(transform.localPosition.y - openingOffset, 1f).SetEase(Ease.InBack).OnComplete( () => {
				isOpen = false;
				isLocked = false;
			});
		}
		else
		{
			isLocked = true;
			toggleButton.transform.DOScaleY(1, 1f);
			transform.DOLocalMoveY(transform.localPosition.y + openingOffset, 1f).SetEase(Ease.OutBack).OnComplete( () => {
				isOpen = true;
				isLocked = false;
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