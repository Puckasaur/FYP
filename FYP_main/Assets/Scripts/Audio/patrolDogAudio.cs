using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class patrolDogAudio : MonoBehaviour {

	public AudioSource patrolSource;

	// Use this for initialization
	void Start () {

		patrolSource = GetComponent<AudioSource>();
	
	}

	void playBark()
	{
		if(!patrolSource.isPlaying)
		{
			patrolSource.Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
