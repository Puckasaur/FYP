using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class patrolDogAudio : MonoBehaviour {

	public AudioSource patrolBarkSource;
	public AudioSource patrolWalkSource;


	// Use this for initialization
	void Start () {

		//patrolBarkSource = GetComponents<AudioSource>;
	
	}

	void playBark()
	{
		if(!patrolBarkSource.isPlaying)
		{
			patrolBarkSource.Play();
		}
	}

	void patrolWalk ()
	{
		if(!patrolWalkSource.isPlaying)
		{
			patrolWalkSource.Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
