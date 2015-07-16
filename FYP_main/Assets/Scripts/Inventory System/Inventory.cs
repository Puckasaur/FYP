using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Inventory : MonoBehaviour {

	public static int[] inventoryArray = {0, 0, 0, 0, 0};
	public Text inventoryText_BAG;
	public Text inventoryText_KEY;

	void Update()
	{
		inventoryText_BAG.text = "Bags " + "[" + inventoryArray [0] + "]";
		inventoryText_KEY.text = "Keys " + "[" + inventoryArray [1] + "]";
			
		//inventoryArray [0]++;

		if (Input.GetKeyDown ("y")) 
		{
			if (inventoryArray[0] > 0)
			{	
				inventoryArray[0]--;
			}
		}
	}

	
}
