using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;


public class Hide : MonoBehaviour {

	public Camera mainCamera;
	public Camera hideCamera;
	public Transform character;
	public Transform prevPosition;
	public GameObject onScreenCharacter;

	public Text onScreenInstruction;
	public Text onScreenInstructionExit;

	private bool isHiding = false;

	void Start()
	{
		//hideCamera.SetActive (false);
		onScreenInstruction.enabled = false;
		onScreenInstructionExit.enabled = false;
		hideCamera.enabled = false;
		
	}

	void OnTriggerStay()
	{	
		Debug.Log ("Enter Test2");
		onScreenInstruction.enabled = true;

		if (Input.GetKeyDown ("e"))
		{

			character.transform.position = hideCamera.transform.position;

			mainCamera.enabled = false;
			hideCamera.enabled = true;

			//onScreenCharacter.GetComponent<Renderer>().enabled = false;
			Debug.Log ("Test1");
			Wait();
			Debug.Log ("Test2");
			isHiding = true;
			Debug.Log ("Test3");
			
			onScreenInstruction.enabled = false;
			onScreenInstructionExit.enabled = true;
		}
	}

	void OnTriggerExit()
	{
		onScreenInstruction.enabled = false;
	}

	void Update() 	
	{
		if (isHiding == true) 
		{	
			onScreenInstructionExit.enabled = true;
			if (Input.GetKeyDown ("e"))
			{
				hideCamera.enabled = false;
				mainCamera.enabled = true;

				onScreenCharacter.transform.position = prevPosition.transform.position;
				isHiding = false;
			}
		}

		if (isHiding == false)
		{
			onScreenInstructionExit.enabled = false;
		}
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds (2.0f);
		//Debug.Log ("asdad");
	}
}
