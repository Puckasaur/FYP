using UnityEngine;
using System.Collections;

public class destructible : MonoBehaviour {
	
	public GameObject debrisPrefab;
	
	//floorHazards haz;
	
	void Start()
	{
		//haz = GameObject.Find ("floorHazard").GetComponent<floorHazards>();
	}
	void Update()
	{
		
	}
	
	void OnTriggerEnter(Collider Destructible) {
		
		
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
