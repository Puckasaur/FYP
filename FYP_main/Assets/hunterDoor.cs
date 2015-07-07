using UnityEngine;
using System.Collections;

public class hunterDoor : MonoBehaviour {
    public bool doorOpened = false;
    Quaternion startRotation;
    Quaternion endRotation;
	// Use this for initialization
	void Start () {
        startRotation = transform.rotation;
        endRotation = new Quaternion(0, 0, -90, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void openDoor()
    {
        this.transform.Rotate(0,0,90);
        doorOpened = true;
    }
}
