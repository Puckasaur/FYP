using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class menuScript : MonoBehaviour {

	private fade fading;

	public Canvas quitMenu;
	public Button startButton;
	public Button optionsButton;
	public Button exitButton;


	// Use this for initialization
	void Start () 
	{
		fading = GameObject.Find ("Fading").GetComponent<fade>();
	
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
     	PlayerPrefs.SetString("Movie", "Intro");
        PlayerPrefs.SetInt("Scene", 2);
        PlayerPrefs.Save();
		Application.LoadLevel (1);
	}

	public void optionsPage()
	{
		Application.LoadLevel(5);
	}

	IEnumerator fadeChange()
	{
		float fadeTime = fading.BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		Application.LoadLevel(Application.loadedLevel + 1);
	}

}
