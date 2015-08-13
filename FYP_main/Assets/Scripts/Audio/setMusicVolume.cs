using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class setMusicVolume : MonoBehaviour {

	float musicVol;
	

	public AudioMixer bgmMixer;

	// Use this for initialization
	void Start () {
	


	}
	
	// Update is called once per frame
	void Update () {
	
		musicVol = PlayerPrefs.GetFloat ("Music Vol");


		bgmMixer.SetFloat ("BGM Master Vol", musicVol);

	}
}
