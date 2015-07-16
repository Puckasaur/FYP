using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class sfxPlayer : MonoBehaviour {

	public AudioSource keySource;
	public AudioSource unlockSource;

	// Use this for initialization
	void Start () 
	{

	}
	
	public void playKey()
	{
		keySource.Play ();
	}

	public void playUnlock()
	{
		unlockSource.Play ();
	}
}
