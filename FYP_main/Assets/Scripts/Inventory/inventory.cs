using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class inventory : MonoBehaviour
{
	public int slotsX, slotsY;
	public GUISkin skin;
	public List<items> inventoryItem = new List<items> ();
	public List<items> slots = new List<items> ();
	private itemDatabase database;

	public static int[] inventoryArray = {0, 0, 0, 0, 0};
	public Text inventoryText_BAG;
	public Text inventoryText_KEY;

	public bool inventoryActive = false;

	void Awake()
	{
		for (int i = 0; i < (slotsX * slotsY); i++) 
		{
			slots.Add(new items());
			inventoryItem.Add (new items());
		}

		database = GameObject.FindGameObjectWithTag ("itemDatabase").GetComponent<itemDatabase> ();

		//inventoryItem[0] = database.item[0];
		//inventoryItem[1] = database.item[1];

		//Debug.Log ("Number of Item: " + inventoryItem.Count);
		//inventoryItem.Add (database.item [0]);
		//Debug.Log ("Number of Item: " + inventoryItem.Count);
	}
	void Update()
	{
		inventoryText_BAG.text = "Bags " + "[" + inventoryArray [0] + "]";
		inventoryText_KEY.text = "Keys " + "[" + inventoryArray [1] + "]";

		if (Input.GetKeyDown ("y")) 
		{
			if (inventoryArray[0] > 0)
			{	
				inventoryArray[0]--;
			}
		}

		if (Input.GetKeyUp ("i")) 
		{
			inventoryActive = !inventoryActive;
		}

	}

	void OnGUI()
	{
		GUI.skin = skin;

		if (inventoryActive)
		{
			DrawInventory(); 
		}
	}
	
	void DrawInventory()
	{
		int i = 0;
		//amount of slot on x-axixs
		for (int y = 0; y < slotsY; y++)
		{
			for (int x = 0; x < slotsX; x++)
			{
				Rect slotRect = new Rect(x * 70, y * 70, 60, 50);
				GUI.Box (slotRect, y.ToString(), skin.GetStyle("slot"));

				slots[i] = inventoryItem[i];

				if (slots[i].itemName != null)
				{
					GUI.DrawTexture(slotRect, slots[i].itemIcon);
				}

				i++;
			}
		}
	}
	
}
