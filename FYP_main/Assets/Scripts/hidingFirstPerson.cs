using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class hidingFirstPerson : MonoBehaviour {

	public Camera mainCamera;
	public Camera hideCamera;
	public Transform character;
	public Transform prevPosition;

	public Text onScreenInstruction;
	public Text onScreenInstructionExit;

	private bool isHiding;

	void Start()
	{
		mainCamera.enabled = true;
		hideCamera.enabled = false;

		isHiding = false;
		
		onScreenInstruction.enabled = false;
		onScreenInstructionExit.enabled = false;
	}

	void OnTriggerStay()
	{
		onScreenInstruction.enabled = true;

		if (isHiding == false) 
		{
			if (Input.GetKeyDown ("e"))
			{
				character.transform.position = hideCamera.transform.position;

				mainCamera.enabled = false;
				hideCamera.enabled = true;

				Debug.Log ("DO IT NOW!");

				StartCoroutine(Wait());

			}
		}
	}

	void Update()
	{
		if (isHiding == true) 
        {
			if (Input.GetKeyDown ("e")) 
            {
				Debug.Log ("DO IT!");
				//hideCamera.enabled = false;
				//mainCamera.enabled = true;
				StartCoroutine (Delayed ());

				//character.transform.position = prevPosition.transform.position;

				isHiding = false;


			}
		}
	}


	void OnTriggerExit()
	{
		onScreenInstruction.enabled = false;
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(0.1f);
		isHiding = true;
		onScreenInstruction.enabled = false;
		onScreenInstructionExit.enabled = true;
	}

	IEnumerator Delayed()
	{
		yield return new WaitForSeconds(0.1f);
		hideCamera.enabled = false;
		mainCamera.enabled = true;
		character.transform.position = prevPosition.transform.position;
		onScreenInstructionExit.enabled = false;
		
	}

}
