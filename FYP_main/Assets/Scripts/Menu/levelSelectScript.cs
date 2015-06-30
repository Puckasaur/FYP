using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class levelSelectScript : MonoBehaviour {

	public Canvas notAvailable;
	public Button levelOne;
	public Button levelTwo;
	public Button levelThree;
	public Button levelFour;
	public Button levelFive;
	public Button backToMain;


	// Use this for initialization
	void Start () 
	{

		notAvailable = notAvailable.GetComponent<Canvas> ();
		levelOne = levelOne.GetComponent<Button> ();
		levelTwo = levelTwo.GetComponent<Button> ();
		levelThree = levelThree.GetComponent<Button> ();
		levelFour = levelFour.GetComponent<Button> ();
		levelFive = levelFive.GetComponent<Button> ();
		backToMain = backToMain.GetComponent<Button> ();

		
		notAvailable.enabled = false;

	
	}

	public void onePress()
	{
		Application.LoadLevel (2);
	}

	public void twoPress()
	{
		//Uncomment when level is ready
		//Application.LoadLevel (3);

		//comment out when level is ready
		notAvailable.enabled = true;
		levelOne.enabled = false;
		levelTwo.enabled = false;
		levelThree.enabled = false;
		levelFour.enabled = false;
		levelFive.enabled = false;
		backToMain.enabled = false;
	}

	public void threePress()
	{
		//Uncomment when level is ready
		//Application.LoadLevel (4);

		//comment out when level is ready
		notAvailable.enabled = true;
		levelOne.enabled = false;
		levelTwo.enabled = false;
		levelThree.enabled = false;
		levelFour.enabled = false;
		levelFive.enabled = false;
		backToMain.enabled = false;
	}

	public void fourPress()
	{
		//Uncomment when level is ready
		//Application.LoadLevel (5);

		//comment out when level is ready
		notAvailable.enabled = true;
		levelOne.enabled = false;
		levelTwo.enabled = false;
		levelThree.enabled = false;
		levelFour.enabled = false;
		levelFive.enabled = false;
		backToMain.enabled = false;
	}

	public void fivePress()
	{
		//Uncomment when level is ready
		//Application.LoadLevel (6);

		//comment out when level is ready
		notAvailable.enabled = true;
		levelOne.enabled = false;
		levelTwo.enabled = false;
		levelThree.enabled = false;
		levelFour.enabled = false;
		levelFive.enabled = false;
		backToMain.enabled = false;
	}

	public void backNoAvail()
	{
		notAvailable.enabled = false;
		levelOne.enabled = true;
		levelTwo.enabled = true;
		levelThree.enabled = true;
		levelFour.enabled = true;
		levelFive.enabled = true;
		backToMain.enabled = true;
	}

	public void bactToMain()
	{
		Application.LoadLevel (0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
