using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class setMenuVolume : MonoBehaviour {

	float musicVol;

	public AudioMixer menuMusic;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	

		musicVol = PlayerPrefs.GetFloat ("Music Vol");

		menuMusic.SetFloat("Main Menu Master Vol", musicVol);
	}
}
