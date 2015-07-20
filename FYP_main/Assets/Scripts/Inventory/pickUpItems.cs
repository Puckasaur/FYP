using UnityEngine;
using System.Collections;

public class pickUpItems : MonoBehaviour {

	void OnTriggerEnter (Collider pickUpObject)
	{
		if (pickUpObject.tag == "player")
		{
 			inventory.inventoryArray[0]++;

			Destroy (this.gameObject);
		}
	}
}
