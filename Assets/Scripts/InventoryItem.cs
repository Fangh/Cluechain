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

	void ShowDetails()
	{
		if (Inventory.Instance.sidePanel.activeSelf
		&& Inventory.Instance.picture.sprite == myItem.pictureSprite)
		{
			Inventory.Instance.sidePanel.SetActive(false);
		}
		else
		{
			Inventory.Instance.sidePanel.SetActive(true);
			Inventory.Instance.descriptionText.text = myItem.description;
			Inventory.Instance.picture.sprite = myItem.pictureSprite;
		}
	}

	public void Initialize(Item i)
	{
		if ( i == null )
		{
			GetComponent<Button>().interactable = false;
			return;
		}
		GetComponent<Button>().interactable = true;
		myItem = i;
		icon.sprite = myItem.sprite;
		icon.gameObject.transform.localScale = Vector3.zero;
		icon.gameObject.transform.DOScale(Vector3.one, 0.7f).SetEase(Ease.OutElastic).SetDelay(1f);
	}

	public void Empty()
	{
		icon.sprite = null;
		myItem = null;
		GetComponent<Button>().interactable = false;
	}
}