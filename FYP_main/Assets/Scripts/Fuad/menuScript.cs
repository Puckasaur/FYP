using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class menuScript : MonoBehaviour {

	public Canvas quitMenu;
	public Button startButton;
	public Button optionsButton;
	public Button exitButton;


	// Use this for initialization
	void Start () 
	{
	
		quitMenu = quitMenu.GetComponent<Canvas> ();
		startButton = startButton.GetComponent<Button> ();
		optionsButton = optionsButton.GetComponent<Button> ();
		exitButton = exitButton.GetComponent<Button> ();

		quitMenu.enabled = false;

	}

	public void exitPress()
	{
		quitMenu.enabled = true;
		startButton.enabled = false;
		optionsButton.enabled = false;
		exitButton.enabled = false;
	}

	public void noPress()
	{
		quitMenu.enabled = false;
		startButton.enabled = true;
		optionsButton.enabled = true;
		exitButton.enabled = true;
	}

	public void exitGame()
	{
		Application.Quit ();
	}

	public void startGame ()
	{
		Application.LoadLevel (1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
