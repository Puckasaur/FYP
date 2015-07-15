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
			inside = !inside;
		}
	}
		
	void OnTriggerExit(Collider ladder)
	{
		if (ladder.gameObject.tag == "player")
		{
			climbMovement.enabled = true;
			inside = !inside;
		}
	}
		
	void Update()
	{
		if (inside == true && Input.GetKey(KeyCode.W))
		{
			characterController.transform.position += Vector3.up / heightFactor;
		}
	}
}
