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
        if (other.tag == "player")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                transform.parent.GetComponent<Rigidbody>().AddForce(Vector3.forward * 50, ForceMode.Force);
            }
        }
    }
}

