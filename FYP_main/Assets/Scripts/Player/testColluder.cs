using UnityEngine;
using System.Collections;

public class testColluder : MonoBehaviour {

	Wall wall1;

	void OnTriggerEnter()
	{
		wall1.SetTransparent ();
	}

	void OnTriggerExit()
	{
		wall1.SetToNormal ();
	}
}
