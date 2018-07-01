using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marketplace : MonoBehaviour 
{
	public static Marketplace Instance;

	public string myItemName = "Rayman";
	public string theirItemName = "Lapin Licorne";

	public Image icon1;
	public Image icon2;

	public InputField adress1;
	public InputField adress2;

	public Text owner1;
	public Text owner2;

	private Item item1;
	private Item item2;

	public bool isOpen = false;


	Marketplace()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		
	}

	void OnEnable()
	{
		item1 = ItemGeneratorContract.Instance.GetItemByName(myItemName);
		item2 = ItemGeneratorContract.Instance.GetItemByName(theirItemName);

		icon1.sprite = item1.sprite;
		icon2.sprite = item2.sprite;

		adress1.text = item1.adress;
		adress2.text = item2.adress;

		owner1.text = "Owner : "+item1.owner;
		owner2.text = "Owner : "+item2.owner;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void Open()
	{
		gameObject.SetActive(true);
		isOpen = true;
		Inventory.Instance.TogglePause(true);
	}
	
	public void Close()
	{
		gameObject.SetActive(false);
		isOpen = false;
		Inventory.Instance.TogglePause(false);
	}

	public void Approve()
	{
		ItemGeneratorContract.Instance.ApproveExchange( item1, item2 );
		Inventory.Instance.RemoveItem(item1);
		Inventory.Instance.AddItem(item2);
		Close();
	}
}