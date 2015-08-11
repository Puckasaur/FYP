﻿using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class setMusicVolume : MonoBehaviour {

	float musicVol;
	
	public AudioSource menuMusic;
	public AudioSource quietSource;
	public AudioSource chaseSource;

	// Use this for initialization
	void Start () {
	


	}
	
	// Update is called once per frame
	void Update () {
	
		musicVol = PlayerPrefs.GetFloat ("Music Vol");

		menuMusic.volume = musicVol;
		quietSource.volume = chaseSource.volume = musicVol;

		Debug.Log (musicVol);

	}
}
