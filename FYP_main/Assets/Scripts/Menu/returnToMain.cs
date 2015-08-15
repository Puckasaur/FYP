using UnityEngine;
using System.Collections;

public class returnToMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void backToMain ()
	{
		Application.LoadLevel (0);
	}
}
