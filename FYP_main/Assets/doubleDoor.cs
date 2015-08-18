using UnityEngine;
using System.Collections;

public class doubleDoor : MonoBehaviour {

    public float doorSpeed;
    private Transform leftDoor;
    private Transform rightDoor;
    private float leftClosedPositionX;
    private float rightClosedPositionX;
    private float leftClosedRotation;
    private float rightClosedRotation;
    Quaternion leftDoorRotation;
    Quaternion rightDoorRotation;
    public float doorNumber;
    bool opening = false;

	// Use this for initialization
	void Start () 
    {
        leftDoor = GameObject.Find("leftDoor").transform;
        rightDoor = GameObject.Find("rightDoor").transform;
        leftClosedPositionX = leftDoor.position.x;
        rightClosedPositionX = rightDoor.position.x;
        leftDoorRotation = Quaternion.Euler(leftDoor.rotation.x, leftDoor.rotation.y - 90, leftDoor.rotation.z);
        rightDoorRotation = Quaternion.Euler(rightDoor.rotation.x, rightDoor.rotation.y - 90, rightDoor.rotation.z);
	}
	
	// Update is called once per frame
	void Update () 
    {
	
        if(opening)
        {
            // Move the left door
            leftDoor.rotation = Quaternion.Slerp(leftDoor.rotation, leftDoorRotation, Time.deltaTime);
            // Move the right door
            rightDoor.rotation = Quaternion.Slerp(rightDoor.rotation, rightDoorRotation, Time.deltaTime);
        }
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            inventory.inventoryArray[1]--;

            for (int j = 0; j < other.GetComponent<TemporaryMovement>().numberOfKeys; j++) // checks all the keys possessed by the player and if one corresponds with the door he wants to open
            {

                if (other.GetComponent<TemporaryMovement>().keyPossessed[j] == doorNumber && opening == false)
                {

                    opening = true;
                }
            }
        }
    }
}
