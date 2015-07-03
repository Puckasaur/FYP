using UnityEngine;
using System.Collections;

public class TemporaryMovement : MonoBehaviour 
{
	public float movementSpeed;
	public float jumpHeight;
	Rigidbody rb;
	CharacterController charControl;

	void Start()
	{
		rb = GetComponent<Rigidbody> ();
		//charControl = GetComponent<CharacterController> ();
	}

	void FixedUpdate() 
    {
		charControl = GetComponent<CharacterController> ();
		float horizontal = Input.GetAxis ("Horizontal"); //* movementSpeed * Time.deltaTime;
        //transform.Translate(horizontal, 0, 0);

		float vertical = Input.GetAxis("Vertical"); //* movementSpeed * Time.deltaTime;
		//transform.Translate(0, 0, vertical);

		Vector3 movement = new Vector3 (1, 0, 1) * vertical + new Vector3 (1, 0, -1) * horizontal;
		Vector3 look = new Vector3 (-1, 0, 1) * vertical + new Vector3 (1, 0, 1) * horizontal;

		rb.MovePosition (transform.position + movement.normalized * movementSpeed * Time.deltaTime);

		transform.LookAt (transform.position + look, Vector3.up);

		/*
		rb.MovePosition (new Vector3 (1, 0, 1) + new Vector3(hor) + vertical);
		this.transform.LookAt (this.transform.position + horizontal + vertical);
        */
		if (charControl.isGrounded)
		{
			Debug.Log("Grounded");
        // Jump
       		if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space))
        	{
            	this.GetComponent<Rigidbody>().velocity += new Vector3(0.0f, jumpHeight, 0.0f);
			}	
		}
	}
}