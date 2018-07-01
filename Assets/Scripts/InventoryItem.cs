using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryItem : MonoBehaviour 
{
	[Header("References")]
	public Item myItem;
	public Image icon;

	void Start()
	{
		GetComponent<Button>().onClick.AddListener(SetDescription);
	}

	void OnDestroy()
	{
		GetComponent<Button>().onClick.RemoveListener(SetDescription);
	}

	void SetDescription()
	{
		Inventory.Instance.SetDescription(myItem.description);
	}

	public void Initialize(Item i)
	{
		myItem = i;
		icon.sprite = myItem.sprite;
		icon.color = Color.black;
		icon.gameObject.transform.localScale = Vector3.zero;
		icon.gameObject.transform.DOScale(Vector3.one, 0.7f).SetEase(Ease.OutElastic).SetDelay(1f);
	}

	void OnMouseOver()
	{
		Inventory.Instance.descriptionText.text = myItem.description;
	}

	void OnMouseExit()
	{
		Inventory.Instance.descriptionText.text = "";
	}
}