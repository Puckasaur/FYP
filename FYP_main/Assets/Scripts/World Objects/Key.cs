/*  numberOfKeys (int) has to be added to Temporary Movement
 *  keyPossessed[] (array) has to be added to Temporary movement
 * 
 */

using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour
{
    public int keyNumber; // the number of the key (will open the door with the same number)
	sfxPlayer SFX;

    private int i;  // used to add a key in the good array

	void Start()
	{
		SFX = GameObject.Find("SFX").GetComponent<sfxPlayer>();
	}
    
 
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
			inventory.inventoryArray[1]++;

            i = other.GetComponent<TemporaryMovement>().numberOfKeys;
            other.GetComponent<TemporaryMovement>().keyPossessed[i] = keyNumber;
            other.GetComponent<TemporaryMovement>().numberOfKeys += 1;
			SFX.playKey();
			Destroy(this.gameObject, 0.1f);
        }
    }
}