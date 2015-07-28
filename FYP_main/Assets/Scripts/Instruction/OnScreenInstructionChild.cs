using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnScreenInstructionChild : MonoBehaviour {

	public GameObject walkIns;
	public GameObject sprintIns;

	void OnTriggerEnter(Collider tCheck)
	{
		if (tCheck.tag == "player")
		{
			gameObject.GetComponentInParent<OnScreenInstructionParent>().triggerCheck(tCheck);
			gameObject.GetComponentInParent<OnScreenInstructionParent>().enterArea1 = true;
		}
	}	

	IEnumerator delay()
	{

		yield return new WaitForSeconds(0.5f);

		sprintIns.SetActive(true);

		gameObject.GetComponentInParent<OnScreenInstructionParent>().enterArea1 = false;
	}

	void Update()
	{
		if (gameObject.GetComponentInParent<OnScreenInstructionParent>().enterArea1 == true)
		{		
			StartCoroutine(delay ());


		}
	}
}
