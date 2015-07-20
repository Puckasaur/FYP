using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TemporaryMovement : MonoBehaviour
{
	public int numberOfKeys;
	public int[] keyPossessed = new int[3];

	public float movementSpeed;
	public float sprintModifier;
	private float sprintSpeed;
	public float origMovementSpeed;
	public float jumpHeight;
	float m_GroundCheckDistance;
	float m_OrigGroundCheckDistance;
	Rigidbody rb;
	Animator catAnim;
	public GameObject bone;
	public GameObject bagOfAir;

    GameObject boneSpawner;
    GameObject newBone;
    GameObject newBagOfAir;

    public float throwForce = 00.00010f;
    public int bones = 2;
    public int bags;

	private bool isGrounded;
    public List<GameObject> enemies = new List<GameObject>();
    ringOfSmell ring;
    bool smellHidden;
    bool disguisedAsDog;

    public float horizontal;
    public float vertical;

    public Vector3 movement;


	void Start()
	{
		m_GroundCheckDistance = 0.6f;
		rb = GetComponent<Rigidbody> ();
		catAnim = GetComponent<Animator> ();
		origMovementSpeed = movementSpeed;
		sprintSpeed = movementSpeed * sprintModifier;
        ring = GetComponentInChildren<ringOfSmell>();
	}

	void FixedUpdate() 
    {
		bags = inventory.inventoryArray [0];
		sprint ();
		updateAnimator();
		boneSpawner = GameObject.FindGameObjectWithTag("boneSpawner");
		checkGroundStatus ();

		horizontal = Input.GetAxis ("Horizontal"); 
        vertical = Input.GetAxis("Vertical"); 

        movement = new Vector3(1, 0, 1) * vertical + new Vector3(1, 0, -1) * horizontal;
        Vector3 look = new Vector3(-1, 0, 1) * vertical + new Vector3(1, 0, 1) * horizontal;

        rb.MovePosition(transform.position + movement.normalized * movementSpeed * Time.deltaTime);

        transform.LookAt(transform.position + look, Vector3.up);
		
		if (Input.GetKeyDown(KeyCode.T) && bones > 0)
        {
            print(boneSpawner.transform.parent);
			bones--;
			newBone = (GameObject)Instantiate(bone, boneSpawner.transform.position, Quaternion.identity);
        }

		if (Input.GetKeyDown(KeyCode.Y) && bags > 0)
		{
			print(boneSpawner.transform.parent);
			bags--;
			newBagOfAir = (GameObject)Instantiate(bagOfAir, boneSpawner.transform.position, Quaternion.identity);
		}

        if(Input.GetKeyDown(KeyCode.G))
        {
            if(!disguisedAsDog)
            {
                disguisedAsDog = true;
                disGuiseAsDog();
            }
            else if(disguisedAsDog)
            {
                disguisedAsDog = false;
                disGuiseAsDog();
            }
        }

        if(Input.GetKeyDown(KeyCode.H))
        {
            if (smellHidden)
            {
                smellHidden = false;
                ring.isNotDisguised("tempMove");
            }
            else if(!smellHidden)
            {
                smellHidden = true;
                ring.isDisguised("tempMove");
            }
        }
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
				//Debug.Log(rb.velocity);
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

            if (movement.magnitude < 0.25f)
            {
                catAnim.SetBool("isSprinting", false);
                movementSpeed = origMovementSpeed;
            }
		}
	}

	void updateAnimator()
	{
		horizontal = Input.GetAxis ("Horizontal");
		vertical = Input.GetAxis("Vertical");
		
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
        if (other.transform.parent != null)
        {
            if ((Input.GetKeyDown(KeyCode.Return) && other.transform.parent.tag == "ball" || other.transform.parent.tag == "cube"))
            {
                other.transform.parent.GetComponent<Rigidbody>().AddForce(Vector3.forward * throwForce, ForceMode.Force);
                other.transform.GetComponentInParent<breakableObject>().ObjectFalling();
            }

            if (other.transform.parent.tag == "trap")
            {

            }
        }
    }

    void OnTriggerEnter(Collider other) // BREAKABLE OBJECT
    {
        if (other.gameObject.tag == "breakableObject")
        {
            print("Je test un truc en français, mouahahaha.");
            other.GetComponent<breakableObject>().objectBreaking();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag == "trap")
        {        
            print(col.collider);
            breakableObject trap = col.collider.transform.GetComponent<breakableObject>();
            trap.makeSound = true;
            print(trap.makeSound);
        }
    }

    void disGuiseAsDog()
    {
        GameObject[] patrolEnemy;
        GameObject[] hunterEnemy;
        enemies.Clear();
        patrolEnemy = GameObject.FindGameObjectsWithTag("enemy");
        hunterEnemy = GameObject.FindGameObjectsWithTag("huntingDog");

        foreach (GameObject enemy in patrolEnemy)
        {
            enemies.Add(enemy);
        }

        foreach (GameObject enemy in hunterEnemy)
        {
            enemies.Add(enemy);
        }

        if (disguisedAsDog)
        {
            foreach (GameObject enemy in enemies)
            {
                coneOfVision cone = enemy.GetComponentInChildren<coneOfVision>();
                cone.isDisguised();
            }
        }

        else if (!disguisedAsDog)
        {
            foreach (GameObject enemy in enemies)
            {
                coneOfVision cone = enemy.GetComponentInChildren<coneOfVision>();
                cone.isNotDisguised();
            }
        }
    }
}