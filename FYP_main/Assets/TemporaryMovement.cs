using UnityEngine;
using System.Collections;

public class TemporaryMovement : MonoBehaviour 
{
	public float movementSpeed;
	public float jumpHeight;
	Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate() 
    {
        float horizontal = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        transform.Translate(horizontal, 0, 0);

        float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
		transform.Translate(0, 0, vertical);
		/*
		rb.MovePosition (new Vector3 (1, 0, 1) + new Vector3(hor) + vertical);
		this.transform.LookAt (this.transform.position + horizontal + vertical);
        */
        // Jump
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space))
        {
            this.GetComponent<Rigidbody>().velocity += new Vector3(0.0f, jumpHeight, 0.0f);
        }
	}
}