using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Loom.Unity3d;

[CreateAssetMenu(fileName = "Item", menuName = "Create Item", order = 2)]
public class Item : ScriptableObject 
{
	public string description;
	public Address owner;
	public string iconName;
	public byte[] adress; //uniqueID
	public SpriteMapList spriteMap;
	public Sprite sprite;

	// Use this for initialization
	void Awake () 
	{
		//maybe never happens
		sprite = spriteMap.GetSpriteByName(iconName);
	}

	public void SetAdress(byte[] byte32)
	{
		adress = byte32;
	}
}

public enum ItemMode
{
	ingame,
	inventory
}