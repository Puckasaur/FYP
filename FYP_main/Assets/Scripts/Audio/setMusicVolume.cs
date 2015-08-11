using UnityEngine;
using System.Collections;

public class setMusicVolume : MonoBehaviour {

	float musicVol;
	
	public AudioSource menuMusic;
	// Use this for initialization
	void Start () {
	


	}
	
	// Update is called once per frame
	void Update () {
	
		musicVol = PlayerPrefs.GetFloat ("Music Vol");

		menuMusic.volume = musicVol;

		Debug.Log (musicVol);

	}
}
