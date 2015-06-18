using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class menuScript : MonoBehaviour {

	public Canvas quitWindow;
	public Button startText;
	public Button exitText;
	public Button optionText;

	// Use this for initialization
	void Start () 
	{
	
		quitWindow = quitWindow.GetComponent<Canvas> ();
		startText = startText.GetComponent<Button> ();
		exitText = exitText.GetComponent<Button> ();
		optionText = optionText.GetComponent<Button> ();
		quitWindow.enabled = false;


	}

	public void exitPress()
	{
		quitWindow.enabled = true;
		startText.enabled = false;
		exitText.enabled = false;
		optionText.enabled = false;

	}

	public void noPress ()
	{
		quitWindow.enabled = false;
		startText.enabled = true;
		exitText.enabled = true;
		optionText.enabled = true;
	}

	public void startLevel()
	{
		Application.LoadLevel (1);
	}

	public void exitGame()
	{
		Debug.Log ("Quitting");
		Application.Quit ();
		Debug.Log ("Quit");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
