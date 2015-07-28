using UnityEngine;
using System.Collections;

public class smellArea : MonoBehaviour {

	public GameObject ringOfSmell;
	private ringOfSmell checkInArea;

	void Start()
	{
		checkInArea = GameObject.Find ("ring of Smell").GetComponent<ringOfSmell>();
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
