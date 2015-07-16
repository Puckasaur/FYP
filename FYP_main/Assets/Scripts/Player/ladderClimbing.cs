using System;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class ladderClimbing : MonoBehaviour
{
	private TemporaryMovement climbMovement;

	public Transform characterController;
	public bool inside = false;
	public float heightFactor = 3.2f;
		
	void Start()
	{
		climbMovement = GameObject.Find ("Char_Cat").GetComponent<TemporaryMovement>();
	}
		
	void OnTriggerEnter(Collider ladder)
	{
		if (ladder.gameObject.tag == "player")
		{
			climbMovement.enabled = false;
			//inside = !inside;
            inside = true;
		}
	}
		
	void OnTriggerExit(Collider ladder)
	{
		if (ladder.gameObject.tag == "player")
		{
			climbMovement.enabled = true;
			//inside = !inside;
            inside = false;
		}
	}
		
	void Update()
	{
		if (inside == true && 
            characterController.GetComponent<TemporaryMovement>().movement.magnitude > 0.01f /*||
            characterController.GetComponent<TemporaryMovement>().movement.magnitude < 0.1f*/)
		{
			characterController.transform.position += Vector3.up / heightFactor;
		}

        print("MAGNITUDE: " + characterController.GetComponent<TemporaryMovement>().movement.magnitude);
	}
}
