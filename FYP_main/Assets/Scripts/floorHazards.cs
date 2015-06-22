using UnityEngine;
using System.Collections;

public class floorHazards : MonoBehaviour {

	public AudioClip splash;
	new AudioSource audio;



	public floorHazards ()
	{
			
	}

	// Use this for initialization
	void Start () 
	{
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Q)) {
			playSound();
		}
	}

	public void playSound()
	{
		Debug.Log ("sound gonna play!");
		audio.PlayOneShot(splash);
		Debug.Log ("sound has played!");

	}
}
