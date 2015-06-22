using UnityEngine;
using System.Collections;

public class visible : MonoBehaviour {


	public bool checkHiding;

	void Start () {
		checkHiding = false;
	}

	void OnTriggerEnter()
	{
			
		Debug.Log("Inivisble");
		checkHiding = true;
	
	}

	void OnTriggerExit()
	{
		Debug.Log ("Visble");
		checkHiding = false;
	}
}
