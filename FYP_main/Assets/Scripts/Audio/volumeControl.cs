using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class volumeControl : MonoBehaviour {

	float volMusicControl;
	float volSfxControl;

	public Slider volMusicSlider;
	public Slider volSfxSlider;

	// Use this for initialization
	void Start () {
	
		volMusicSlider.value = PlayerPrefs.GetFloat ("Music Vol");
		volSfxSlider.value = PlayerPrefs.GetFloat ("SFX Vol");
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void setMusicVolume()
	{
		volMusicControl = volMusicSlider.value;		
		PlayerPrefs.SetFloat ("Music Vol", volMusicControl);
		PlayerPrefs.Save ();
	}

	public void setSfxVolume()
	{
		volSfxControl = volSfxSlider.value;
		PlayerPrefs.SetFloat ("SFX Vol", volSfxControl);
		PlayerPrefs.Save ();
	}
}
