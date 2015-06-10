using UnityEngine;
using System.Collections;

public class floorHazards : MonoBehaviour {

	public AudioClip splash;
	AudioSource audio;

	// Use this for initialization
	void Start () 
	{
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
	{
			
	}

	public void playSound()
	{
	audio.PlayOneShot (splash);
	}
}
