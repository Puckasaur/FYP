using UnityEngine;
using System.Collections;

public class smellArea : MonoBehaviour {

	public GameObject ringOfSmell;
	private ringOfSmell checkInArea;
	private smelling smell;

	void Start()
	{
		checkInArea = GameObject.Find ("ring of Smell").GetComponent<ringOfSmell>();
		smell = GameObject.Find ("Smelling_Particle").GetComponent<smelling>();
	}

	void OnTriggerEnter(Collider enemyCheck)
	{
		if (enemyCheck.tag == "player")
		{
			ringOfSmell.SetActive(false);
			checkInArea.radius = 10.0f;
		}
	}

	void OnTriggerExit(Collider playerCheck)
	{
		if (playerCheck.tag == "player")
		{	
			ringOfSmell.SetActive(true);
		}
	}


}
