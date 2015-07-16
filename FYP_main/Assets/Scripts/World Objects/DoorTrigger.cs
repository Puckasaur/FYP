using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour
{
	sfxPlayer SFX;

    public int doorNumber; // number of the door (opened by a key with the same number)
    public float zRotation; // Vector 3 rotation 
    public float angle; // angle, well, tbh I'm not sure of the difference between "zRotation" and "angle" but...


    private bool opening = false; // checks if the door is opened or not

	void Start ()
	{
		SFX = GameObject.Find("SFX").GetComponent<sfxPlayer>();
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {	
			Inventory.inventoryArray[1]--;

            for (var j = 0; j < other.GetComponent<TemporaryMovement>().numberOfKeys; j++) // checks all the keys possessed by the player and if one corresponds with the door he wants to open
            {
                if (other.GetComponent<TemporaryMovement>().keyPossessed[j] == doorNumber && opening == false)
                {
                    opening = true;
					SFX.playUnlock();
                    Destroy(this.gameObject, 0.1f);
                    //this.transform.Rotate(new Vector3(0.0f, 0.0f, zRotation), angle, Space.Self);
                }
            }
        }
    }
}