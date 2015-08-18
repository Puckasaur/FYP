using UnityEngine;
using System.Collections;

public class pause : MonoBehaviour {

	void Update()
	{
		if (Input.GetButtonDown("quit"))
		{			
			Application.Quit();
		}
	}
}
