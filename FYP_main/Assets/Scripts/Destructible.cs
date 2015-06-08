using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {

	public GameObject debrisPrefab;

	void Update()
	{
		if(Input.GetKey(KeyCode.F12)){
			destroyMe ();
		}
	}

	void OnTriggerEnter() {
		destroyMe ();
	}

	void destroyMe()
	{
		if (debrisPrefab) {
			Instantiate (debrisPrefab, transform.position, transform.rotation);
		}
		
		Destroy (gameObject);
	}
}
