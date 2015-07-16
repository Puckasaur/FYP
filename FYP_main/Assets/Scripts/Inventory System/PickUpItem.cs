using UnityEngine;
using System.Collections;

public class PickUpItem : MonoBehaviour {

	void Update()
	{

	}

	void OnTriggerEnter (Collider pickUpObject)
	{
		if (pickUpObject.tag == "player")
		{
			Inventory.inventoryArray[0]++;

			Destroy (this.gameObject);
		}
	}
}
