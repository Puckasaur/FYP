using UnityEngine;
using System.Collections;

public class hunterDoorOpener : MonoBehaviour {
    hunterDoor script;
    public GameObject door;
	// Use this for initialization
	void Start () {
        script = door.GetComponent<hunterDoor>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerStay(Collider other)
    {
        if(script.doorOpened == false)
        {
            script.openDoor();
        }
    }
}
