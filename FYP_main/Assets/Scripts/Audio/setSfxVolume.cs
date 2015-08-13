using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class setSfxVolume : MonoBehaviour {

	float sfxVol;

	public AudioMixer sfxMixer;
	public AudioMixer dogMixer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		sfxVol = PlayerPrefs.GetFloat("SFX Vol");

		sfxMixer.SetFloat ("SFX Master Vol", sfxVol);
		dogMixer.SetFloat ("Dog Master Vol", sfxVol);
	}
}
