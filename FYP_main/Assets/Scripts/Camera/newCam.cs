using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class newCam : MonoBehaviour {

	public Text countText;

	bool isSecondary;

	public Transform target;

	public GameObject mainCamera;
	public GameObject secondaryCamera;

	// Use this for initialization
	void Start () {
		isSecondary = false;

	
	}	
	
	// Update is called once per frame
	void Update () {


		if (Input.GetKeyUp ("v")) 
		{
			isSecondary = !isSecondary;

		}

		if (isSecondary == false) {
			secondaryCamera.SetActive(false);
			mainCamera.SetActive(true);

//			countText.text = "Main Camera";
			//Debug.Log ("Main Camera");

			//secondaryCamera.transform.position = target.position;
			secondaryCamera.transform.position = new Vector3(target.position.x, target.position.y, target.position.z);
		
		} else if (isSecondary == true)
		{
			secondaryCamera.SetActive(true);
			mainCamera.SetActive(false);

			//countText.text = "Secondary Camera";
			//Debug.Log ("Secondary Camera");

		}

	}
}