using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class volumeControl : MonoBehaviour {

	float volControl;

	public Slider volSlider;

	// Use this for initialization
	void Start () {
	
		volSlider.value = PlayerPrefs.GetFloat ("Music Vol");

	}
	
	// Update is called once per frame
	void Update () {
	


	}

	public void setVolume()
	{
		volControl = volSlider.value;
		
		PlayerPrefs.SetFloat ("Music Vol", volControl);
		
		PlayerPrefs.Save ();
	}
}
