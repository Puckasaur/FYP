using System;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class ladderClimbing : MonoBehaviour
{
	private TemporaryMovement climbMovement;

	public Transform characterController;
	public bool inside = false;
	public float heightFactor;
		
	void Start()
	{
		climbMovement = GameObject.Find("Char_Cat").GetComponent<TemporaryMovement>();
	}
		
	void OnTriggerStay(Collider ladder)
	{
		if (ladder.gameObject.tag == "player")
		{
            ladder.GetComponent<TemporaryMovement>().onLadder = true;

            if (climbMovement.movement.magnitude > 1)
            {    
                climbMovement.rb.useGravity = false;
                climbMovement.enabled = false;
            }
            
            else
            {
                climbMovement.rb.useGravity = true;
                climbMovement.enabled = true;
            }
			//inside = !inside;
            inside = true;
		}
	}
		
	void OnTriggerExit(Collider ladder)
	{
       // ladder.GetComponent<TemporaryMovement>().onLadder = false;
		if (ladder.gameObject.tag == "player")
		{
			climbMovement.enabled = true;
            climbMovement.rb.useGravity = true;
			//inside = !inside;
            inside = false;
		}
	}
		
	void Update()
	{
        if (inside == true && climbMovement.movement.magnitude > 0.01f)
		{
            climbMovement.transform.position += Vector3.up / heightFactor * climbMovement.movement.magnitude;
		}

        //print("MAGNITUDE: " + characterController.GetComponent<TemporaryMovement>().movement.magnitude);
	}
}
