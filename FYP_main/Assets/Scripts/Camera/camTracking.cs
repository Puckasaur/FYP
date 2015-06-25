using UnityEngine;
using System.Collections;

public class camTracking : MonoBehaviour {

	public Transform target;
    public float value;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.x >= target.position.x + value || transform.position.x <= target.position.x - value || transform.position.z >= target.position.z + value || transform.position.z <= target.position.z - value)
		transform.position = target.position;
	}
}
