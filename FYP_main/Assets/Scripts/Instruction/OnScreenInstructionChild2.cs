using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnScreenInstructionChild2 : MonoBehaviour {

	public GameObject area2;

	//public GameObject walkIns;
	public GameObject sprintIns;

	void OnTriggerEnter(Collider tCheck)
	{
		if (tCheck.tag == "player")
		{
			gameObject.GetComponentInParent<OnScreenInstructionParent>().triggerCheck(tCheck);
			gameObject.GetComponentInParent<OnScreenInstructionParent>().enterArea2 = true;
		}
	}	

	IEnumerator delay()
	{
		//walkIns.SetActive(false);

		sprintIns.SetActive(false);

		yield return new WaitForSeconds(0.2f);


		//area1.SetActive(false);
	
		gameObject.GetComponentInParent<OnScreenInstructionParent>().enterArea2 = false;
	}

	void Update()
	{
		if (gameObject.GetComponentInParent<OnScreenInstructionParent>().enterArea2 == true)
		{		
			StartCoroutine(delay ());

		}
	}
}
