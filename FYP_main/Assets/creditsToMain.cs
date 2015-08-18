using UnityEngine;
using System.Collections;

public class creditsToMain : MonoBehaviour {
	
	returnToMain Main;

	// Use this for initialization
	void Start () {
		Main = GameObject.Find ("Jump to Main").GetComponent<returnToMain> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetButtonDown ("Jump")) 
		{
			Main.backToMain();
		}

	}
}
