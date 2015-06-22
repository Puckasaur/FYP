using UnityEngine;
using System.Collections;

public class hazardTrigger : MonoBehaviour {

	floorHazards Haz;
	

	// Use this for initialization
	void Start () 
	{
		Haz = GameObject.Find ("floorHazard").GetComponent<floorHazards>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider MainCamera)
	{
		Haz.playSound ();
	}
}
