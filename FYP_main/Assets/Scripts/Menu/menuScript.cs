using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class menuScript : MonoBehaviour {

	private fade fading;

	public Canvas quitMenu;
	public Button startButton;
	public Button optionsButton;
	public Button exitButton;

	sfxPlayer SFX;


	// Use this for initialization
	void Start () 
	{
		fading = GameObject.Find ("Fading").GetComponent<fade>();
	
		quitMenu = quitMenu.GetComponent<Canvas> ();
		startButton = startButton.GetComponent<Button> ();
		optionsButton = optionsButton.GetComponent<Button> ();
		exitButton = exitButton.GetComponent<Button> ();
		quitMenu.enabled = false;

		SFX = GameObject.Find ("SFX").GetComponent<sfxPlayer>();

	}

	public void exitPress()
	{

		SFX.playButtonPress ();
		quitMenu.enabled = true;
		startButton.enabled = false;
		optionsButton.enabled = false;
		exitButton.enabled = false;

	}

	public void noPress()
	{
		SFX.playButtonPress ();
		quitMenu.enabled = false;
		startButton.enabled = true;
		optionsButton.enabled = true;
		exitButton.enabled = true;
	}

	public void exitGame()
	{
		SFX.playButtonPress ();
		Application.Quit ();
	}

	public void startGame ()
	{
		SFX.playButtonPress ();
     	PlayerPrefs.SetString("Movie", "Intro");
        PlayerPrefs.SetInt("Scene", 2);
        PlayerPrefs.Save();
		Application.LoadLevel (1);
	}

	public void optionsPage()
	{
		SFX.playButtonPress ();
		Application.LoadLevel(5);
	}

	IEnumerator fadeChange()
	{
		float fadeTime = fading.BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		Application.LoadLevel(Application.loadedLevel + 1);
	}

}
