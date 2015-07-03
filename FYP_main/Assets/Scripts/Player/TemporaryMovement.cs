using UnityEngine;
using System.Collections;

public class TemporaryMovement : MonoBehaviour
{
	public float movementSpeed;
	public float sprintModifier;
	private float sprintSpeed;
	private float origMovementSpeed;
	public float jumpHeight;
	float m_GroundCheckDistance;
	float m_OrigGroundCheckDistance;
	Rigidbody rb;
	Animator catAnim;
	 public GameObject bone;
    GameObject boneSpawner;
    GameObject newBone;
    public float throwForce = 00.00010f;
    public int bones = 2;
	private bool isGrounded;

	void Start()
	{
		m_GroundCheckDistance = 0.6f;
		rb = GetComponent<Rigidbody> ();
		catAnim = GetComponent<Animator> ();
		origMovementSpeed = movementSpeed;
		sprintSpeed = movementSpeed * sprintModifier;
	}


	void FixedUpdate() 
    {

		sprint ();
		updateAnimator();
		boneSpawner = GameObject.FindGameObjectWithTag("boneSpawner");
		checkGroundStatus ();
		float horizontal = Input.GetAxis ("Horizontal"); //* movementSpeed * Time.deltaTime;
        //transform.Translate(horizontal, 0, 0);

        float vertical = Input.GetAxis("Vertical"); //* movementSpeed * Time.deltaTime;
        //transform.Translate(0, 0, vertical);

        Vector3 movement = new Vector3(1, 0, 1) * vertical + new Vector3(1, 0, -1) * horizontal;
        Vector3 look = new Vector3(-1, 0, 1) * vertical + new Vector3(1, 0, 1) * horizontal;

        rb.MovePosition(transform.position + movement.normalized * movementSpeed * Time.deltaTime);

        transform.LookAt(transform.position + look, Vector3.up);
		
		if (Input.GetKeyDown(KeyCode.T) && bones > 0)
        {
            print(boneSpawner.transform.parent);
			bones--;
			newBone = (GameObject)Instantiate(bone, boneSpawner.transform.position, Quaternion.identity);
			// newBone.GetComponent<Rigidbody>().AddForce(this.transform.right * throwForce + this.transform.up * (throwForce / 2));
        }
        

        /*
        rb.MovePosition (new Vector3 (1, 0, 1) + new Vector3(hor) + vertical);
        this.transform.LookAt (this.transform.position + horizontal + vertical);
        */

	}

	void Update()
	{
		//checks if character is grounded
		if (isGrounded) 
		{
			// Jump
			if (Input.GetButtonDown ("Jump")) 
			{
				rb.velocity += new Vector3 (0.0f, jumpHeight, 0.0f);
				Debug.Log(rb.velocity);
			}	
		}
	}

	void sprint()
	{
		if (isGrounded)
		{
			if (Input.GetButton ("Sprint")) 
			{
				catAnim.SetBool("isSprinting", true);
				movementSpeed = sprintSpeed;
			}
			else 
			{
				catAnim.SetBool("isSprinting", false);
				movementSpeed = origMovementSpeed;
			}
		}
	}

	void updateAnimator()
	{
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		
		catAnim.SetFloat ("hSpeed", horizontal);
		catAnim.SetFloat ("vSpeed", vertical);
	}

	//uses raycast to check if player is grounded
	void checkGroundStatus()
	{
		RaycastHit hitInfo;
		#if UNITY_EDITOR
		// helper to visualise the ground check ray in the scene view
		Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
		#endif
		// 0.1f is a small offset to start the ray from inside the character
		// it is also good to note that the transform position in the sample assets is at the base of the character
		if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
		{
			isGrounded = true;
		}
		else
		{
			isGrounded = false;
		}
	}
    void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Return) && other.transform.parent.tag == "ball" || other.transform.parent.tag == "cube")
        {
            other.transform.parent.GetComponent<Rigidbody>().AddForce(Vector3.forward * throwForce, ForceMode.Force);
        }
    }
}