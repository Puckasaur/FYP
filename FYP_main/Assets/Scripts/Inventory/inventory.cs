using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class inventory : MonoBehaviour
{

	public List<items> inventoryItem = new List<items> ();
	private itemDatabase database;

	public static int[] inventoryArray = {0, 0, 0, 0, 0};
	public Text inventoryText_BAG;
	public Text inventoryText_KEY;

	public bool inventoryActive = false;

	void Start()
	{
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

		if (Input.GetKey ("i")) 
		{
			inventoryActive = !inventoryActive;
		}

	}

	void OnGUI()
	{
		if (inventoryActive)
		{
			DrawInventory(); 
		}

		for (int i = 0; i < inventoryItem.Count; i++)
		{
			GUI.Label (new Rect (10, i, 200, 50), inventoryItem[i].itemName);
		}
	}
	
	void DrawInventory()
	{
		//amount of slot on x-axixs

	}
}
