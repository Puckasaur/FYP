using UnityEngine;
using System.Collections;

public class destroyableTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerStay(Collider other)
    {
        print("collision");
        print(other.tag);
        if (other.tag == "player")
        {
            print("Player in trigger");
            if (Input.GetKeyDown(KeyCode.Return))
            {
                print("Enter pressed");
                transform.parent.GetComponent<Rigidbody>().AddForce(Vector3.forward * 50, ForceMode.Force);
            }
        }
    }
}

