using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hide : MonoBehaviour {
	
	public GameObject mainCamera;
	public GameObject hideCamera;

	private bool guiShow = false;
	private bool isHiding = false;

	void Start()
	{
		hideCamera.SetActive (false);
	}	
	
	void OnTriggerStay()
	{
		//GetComponent<Text>().enabled = true;

		if (Input.GetKeyDown ("e")) {
			hideCamera.SetActive (true);
			//mainCamera.SetActive (false);
		}

//		if (isHiding == true) 
//		{
//			hideCamera.SetActive(false);
//			mainCamera.SetActive(true);
//
//			isHiding = false;
//		}
	}

}
