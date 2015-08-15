using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class sfxPlayer : MonoBehaviour {

	public AudioSource keySource;
	public AudioSource unlockSource;
	public AudioClip[] glassBreak;
	public AudioSource glassSource;

	public AudioSource buttonSource;

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

	public void playGlassBreak()
	{
		int randClip = Random.Range (0, glassBreak.Length);
		glassSource.clip = glassBreak [randClip];
		glassSource.Play ();
	}

	public void playButtonPress()
	{
		buttonSource.Play ();
	}
}
