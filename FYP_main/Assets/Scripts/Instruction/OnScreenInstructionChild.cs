using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnScreenInstructionChild : MonoBehaviour {

	public GameObject area1;

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
		walkIns.SetActive(false);

		yield return new WaitForSeconds(0.2f);

		sprintIns.SetActive(true);

		//area1.SetActive(false);
		gameObject.GetComponentInParent<OnScreenInstructionParent>().enterArea1 = false;

	}
    IEnumerator sprintDelay()
    {
        yield return new WaitForSeconds(5.0f);
        sprintIns.SetActive(false);
        area1.SetActive(false);
    }

	void Update()
	{
		if (gameObject.GetComponentInParent<OnScreenInstructionParent>().enterArea1 == true)
		{		
			StartCoroutine(delay ());

		}
        else if(sprintIns.activeSelf == true)
        {
            StartCoroutine(sprintDelay());
        }
	}
}
