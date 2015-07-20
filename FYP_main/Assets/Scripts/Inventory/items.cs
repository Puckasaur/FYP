using UnityEngine;
using System.Collections;

[System.Serializable]
public class items
{
	public string itemName;
	public int itemID;
	public Texture2D itemIcon;
	public ItemType itemType;
	
	public enum ItemType
	{
		Collectables
	}

	public items (string name, int id, ItemType type)
	{

		itemName = name;
		itemID = id;
		itemIcon = Resources.Load<Texture2D> (name);
		itemType = type;
	}

	public items()
	{
	}
}
