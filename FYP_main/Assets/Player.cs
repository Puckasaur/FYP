using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public float movementSpeed;
	public float turningSpeed;
    public float jumpHeight;

    private Vector3 offset;

	void FixedUpdate() 
    {        
        // Movement
		float horizontal = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        transform.Translate(horizontal, 0, 0);

        float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
		transform.Translate(0, 0, vertical);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.GetComponent<Rigidbody>().velocity += new Vector3(0.0f, jumpHeight, 0.0f);
        }
	}
}