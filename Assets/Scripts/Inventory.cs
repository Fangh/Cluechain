using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Loom.Unity3d;
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
	public InventoryItem[] slots; 
	public Text descriptionText;

	[Header("Tweaking")]
	public float openingOffset;
	public Address owner;

	[Header("Private")]
	private bool isOpen = false;
	private bool isLocked = false;
	private List<Item> items = new List<Item>();

	// Use this for initialization
	void Start () 
	{
		slots = GetComponentsInChildren<InventoryItem>();
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