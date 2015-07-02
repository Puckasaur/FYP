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
        transform.Translate(0, 0, -horizontal);

        float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
		transform.Translate(vertical, 0, 0);
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
    void OnCollisionEnter(Collision other)
    {
        if (Input.GetKey(KeyCode.Return))
        {
            Rigidbody body = other.collider.attachedRigidbody;
            if (body == null) { return; }

            if (other.gameObject.tag == "ball")
            {
                body.isKinematic = false;
                //pushForce = 2.0f;
                //Vector3 pushDir = new Vector3(1, 0, 1);
                //body.velocity = pushDir * pushForce;
                other.gameObject.SendMessage("ObjectFalling", SendMessageOptions.DontRequireReceiver);
            }
            if (other.gameObject.tag == "cube")
            {
                body.isKinematic = false;
                //pushForce = 50.0f;
                //Vector3 pushDir = new Vector3(1, 0, 1);
                //body.AddForce(pushDir * pushForce);
                other.gameObject.SendMessage("ObjectFalling", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}