using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SpriteMap", menuName = "SpriteMap", order = 1)]
public class SpriteMapList : ScriptableObject 
{
	public List<SpriteMap> list;

	public static SpriteMapList Instance;

	SpriteMapList()
	{
		Instance = this;
	}

	public Sprite GetSpriteByName(string name)
	{
		foreach (SpriteMap s in list)
		{
			if ( s.name == name)
			{
				return s.sprite;
			}
		}
		return null;
	}
}