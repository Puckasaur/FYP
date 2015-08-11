using UnityEngine;
using System.Collections;

public class setSfxVolume : MonoBehaviour {

	float sfxVol;

	public AudioSource keySource;
	public AudioSource doorSource;
	public AudioSource destructibleSource;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		sfxVol = PlayerPrefs.GetFloat("SFX Vol");

		keySource.volume = doorSource.volume = destructibleSource.volume = sfxVol;

	}
}
