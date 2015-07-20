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

	void Start()
	{
		for (int i = 0; i < (slotsX * slotsY); i++) 
		{
			slots.Add(new items());
		}

		database = GameObject.FindGameObjectWithTag ("itemDatabase").GetComponent<itemDatabase> ();
		Debug.Log ("Number of Item: " + inventoryItem.Count);
		inventoryItem.Add (database.item [0]);
		Debug.Log ("Number of Item: " + inventoryItem.Count);
		
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

//		for (int i = 0; i < inventoryItem.Count; i++)
//		{
//			GUI.Label (new Rect (10, i, 200, 50), inventoryItem[i].itemName);
//		}
	}
	
	void DrawInventory()
	{
		//amount of slot on x-axixs
		for (int x = 0; x < slotsX; x++) 
		{
			for (int y = 0; y < slotsY; y++)
			{
				GUI.Box (new Rect(x * 60, y * 60, 50, 50), y.ToString(), skin.GetStyle("slot"));
			}
		}
	}
}
