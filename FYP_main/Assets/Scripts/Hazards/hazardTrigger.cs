using UnityEngine;
using System.Collections;

public class hazardTrigger : MonoBehaviour {

	floorHazards haz;
	

	// Use this for initialization
	void Start () 
	{
		haz = GameObject.Find ("floorHazard").GetComponent<floorHazards>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider MainCamera)
	{
		haz.playSound ();
	}
}
