using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pickUpCollectables : MonoBehaviour {

	public string collectableName;
	public itemDatabase database;
	public inventory inv;
	private static int i;
	private static int j;

	void Awake()
	{
		i = 0;
		j = 0;

		collectableName = gameObject.name;
		inv = GameObject.Find ("Char_Cat").GetComponent<inventory>();
		database = GameObject.Find ("Items_ItemDatabase").GetComponent<itemDatabase>();
		//trophy = GameObject.Find ("trophy");
	}

	void OnTriggerEnter (Collider pickUpCollectable)
	{
		Debug.Log ("i: " + i);
		if (pickUpCollectable.tag == "player")
		{	

			database.item.Add (new items (collectableName, 0 , items.ItemType.Collectables));
					
			inv.inventoryItem[i] = database.item[i];

			inv.inventoryItem.Add (database.item [i]);
		
			i++;

			Destroy (this.gameObject);
		}
	}
}
