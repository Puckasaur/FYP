using UnityEngine;
using System.Collections;

public class pause : MonoBehaviour {

	void Update()
	{
		if (Input.GetButtonDown("quit"))
		{
			Debug.Log ("sakdjaoksdja");
			Application.Quit();
		}
	}
}
