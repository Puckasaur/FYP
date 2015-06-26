using UnityEngine;
using System.Collections;

public class testCamera : MonoBehaviour {

	public GameObject target = null;
	public float multiplier = 5.0f;
	public float distance = 4.0f;
	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {

		//transform.position = transform.position + Camera.main.transform.forward;

		if (Input.GetKey ("q")) {
			transform.RotateAround(target.transform.position, Vector3.up, Time.deltaTime * multiplier);
			
			//transform.RotateAround(target.transform.position, Vector3.up, Time.deltaTime * multiplier);
		}
		if (Input.GetKey ("e")) {
			transform.RotateAround(target.transform.position, Vector3.down, Time.deltaTime * multiplier);
		}

		
	}
}
