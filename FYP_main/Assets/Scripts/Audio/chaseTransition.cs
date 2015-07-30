using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class chaseTransition : MonoBehaviour {

	public bool chase = false;

	public AudioMixerSnapshot inChase;
	public AudioMixerSnapshot outOfChase;
	public AudioClip[] stings;
	public AudioSource stingSource;
	public float bpm = 160;

	private float inTransition;
	private float outTransition;
	private float quarterNote;

	// Use this for initialization
	void Start () 
	{
		quarterNote = 60 / bpm;
		inTransition = quarterNote;
		outTransition = quarterNote * 32;
	}
	
	// Update is called once per frame
	public void chaseTrans () 
	{
		inChase.TransitionTo(inTransition);
	}

	public void outChaseTrans()
	{
		outOfChase.TransitionTo(outTransition);
	}

	public void resetChaseTrans()
	{
		outOfChase.TransitionTo(0.1f);
	}

	public void playSting()
	{
		int randClip = Random.Range (0, stings.Length);
		stingSource.clip = stings [randClip];
		stingSource.Play ();
	}
}
