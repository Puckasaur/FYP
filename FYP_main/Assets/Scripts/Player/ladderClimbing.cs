using System;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ladderClimbing : MonoBehaviour
	{
		
		public Transform characterController;
		public bool inside = false;
		public float heightFactor = 3.2f;
		
		private ThirdPersonUserControl tpsInput;
		
		void Start()
		{
			tpsInput = GetComponent<ThirdPersonUserControl>();
		}
		
		void OnTriggerEnter(Collider ladder)
		{
			if (ladder.gameObject.tag == "ladder")
			{
				tpsInput.enabled = false;
				inside = !inside;
			}
		}
		
		void OnTriggerExit(Collider ladder)
		{
			if (ladder.gameObject.tag == "ladder")
			{
				tpsInput.enabled = true;
				inside = !inside;
			}
		}
		
		void Update()
		{
			if (inside == true && Input.GetKey("w"))
			{
				characterController.transform.position += Vector3.up / heightFactor;
			}
		}
	}
}
