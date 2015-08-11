using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class levelSelectScript : MonoBehaviour {

	private fade fading;

	public Canvas notAvailable;
	public Button levelOne;
	public Button levelTwo;
	public Button levelThree;
	public Button backToMain;


	// Use this for initialization
	void Start () 
	{

		fading = GameObject.Find ("Fading").GetComponent<fade>();
		

		notAvailable = notAvailable.GetComponent<Canvas> ();
		levelOne = levelOne.GetComponent<Button> ();
		levelTwo = levelTwo.GetComponent<Button> ();
		levelThree = levelThree.GetComponent<Button> ();
		backToMain = backToMain.GetComponent<Button> ();

		
		notAvailable.enabled = false;

	
	}

	public void onePress()
	{
		//Application.LoadLevel (2);
        PlayerPrefs.SetString("Movie", "Intro");
        PlayerPrefs.SetInt("Scene", 3);
        PlayerPrefs.Save();
		StartCoroutine(fadeChange());

	}

	public void twoPress()
	{
		//Uncomment when level is ready
        //PlayerPrefs.SetString("Movie", "Level_2_Intro");
        //PlayerPrefs.SetInt("Scene", 4);
        //PlayerPrefs.Save();
		//Application.LoadLevel (2);

		//comment out when level is ready
		notAvailable.enabled = true;
		levelOne.enabled = false;
		levelTwo.enabled = false;
		levelThree.enabled = false;
		backToMain.enabled = false;
	}

	public void threePress()
	{
		//Uncomment when level is ready
        //PlayerPrefs.SetString("Movie", "Level_3_Intro");
        //PlayerPrefs.SetInt("Scene", 5);
        //PlayerPrefs.Save();
		//Application.LoadLevel (2);

		//comment out when level is ready
		notAvailable.enabled = true;
		levelOne.enabled = false;
		levelTwo.enabled = false;
		levelThree.enabled = false;
		backToMain.enabled = false;
	}


	public void backNoAvail()
	{
		notAvailable.enabled = false;
		levelOne.enabled = true;
		levelTwo.enabled = true;
		levelThree.enabled = true;
		backToMain.enabled = true;
	}

	public void bactToMain()
	{
		//Application.LoadLevel (0);
		StartCoroutine(fadeBackChange());
	}

	IEnumerator fadeChange()
	{
		float fadeTime = fading.BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		Application.LoadLevel(Application.loadedLevel + 1);
	}

	IEnumerator fadeBackChange()
	{
		float fadeTime = fading.BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		Application.LoadLevel(Application.loadedLevel - 1);
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log(ao.isDone);
	}
}
