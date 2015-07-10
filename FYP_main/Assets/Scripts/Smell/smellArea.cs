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
		if (enemyCheck.tag == "enemy")
		{
			ringOfSmell.SetActive(false);
		}

		if (enemyCheck.tag == "player")
		{
			ringOfSmell.SetActive(false);
		}
	}

	void OnTriggerExit(Collider playerCheck)
	{
		if (playerCheck.tag == "player")
		{
			ringOfSmell.SetActive(true);
			checkInArea.radius = 15.0f;
		}
	}
}
