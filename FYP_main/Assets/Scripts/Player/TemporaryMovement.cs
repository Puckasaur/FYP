using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TemporaryMovement : MonoBehaviour
{
	public int numberOfKeys;
	public int[] keyPossessed = new int[3];

    public float joystickPressure;
    public float magnMultiplier;

	public float movementSpeed;
	public float sprintModifier;
	private float sprintSpeed;
	public float origMovementSpeed;
	public float jumpHeight;
	float m_GroundCheckDistance;
	float m_OrigGroundCheckDistance;
	public Rigidbody rb;
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

	public float duration = 0.2f;

	public Image boneCoolDown;
	public Image bagCoolDown;

	public Image boneBackground;
	public Image bagBackground;

	private float durationOfSpriteAnimationBone; 
	public AnimationClip spriteAnimationBone;

	private float durationOfSpriteAnimationBag; 
	public AnimationClip spriteAnimationBag;

	IEnumerator spriteBoneTimer()
	{	
		boneCoolDown.GetComponent<Animator>().enabled = true;

		yield return new WaitForSeconds(durationOfSpriteAnimationBone);

		boneCoolDown.GetComponent<Animator>().enabled = false;	
	}

	IEnumerator spriteBagTimer()
	{
		bagCoolDown.GetComponent<Animator>().enabled = true;
		
		yield return new WaitForSeconds(durationOfSpriteAnimationBag);
		
		bagCoolDown.GetComponent<Animator>().enabled = false;	
	}

	void Start()
	{
		boneCoolDown.GetComponent<Animator>().enabled = false;
		bagCoolDown.GetComponent<Animator>().enabled = false;

		bagCoolDown.enabled = false;
		
		m_GroundCheckDistance = 0.6f;
		rb = GetComponent<Rigidbody> ();
		catAnim = GetComponent<Animator> ();
		origMovementSpeed = movementSpeed;
		sprintSpeed = movementSpeed * sprintModifier;
        ring = GetComponentInChildren<ringOfSmell>();

		durationOfSpriteAnimationBone = spriteAnimationBone.length;
		durationOfSpriteAnimationBag = spriteAnimationBag.length;
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
		
		if (Input.GetKeyDown(KeyCode.T) && bones > 0 || Input.GetButtonDown("Fire3"))
		{	
			boneCoolDown.enabled = true;
			bagCoolDown.enabled = false;

			boneBackground.enabled = true;
			bagBackground.enabled = false;

			boneBackground.CrossFadeAlpha(1.0f, duration, true);
			bagBackground.CrossFadeAlpha(0.0f, duration, true);

			StartCoroutine(spriteBoneTimer());
			bones--;
			newBone = (GameObject)Instantiate(bone, boneSpawner.transform.position, Quaternion.identity);
        }

		if (Input.GetKeyDown(KeyCode.Y) /*&& bags > 0*/)
		{
			boneCoolDown.enabled = false;
			bagCoolDown.enabled = true;

			boneBackground.enabled = false;
			bagBackground.enabled = true;

			boneBackground.CrossFadeAlpha(0.0f, duration, true);
			bagBackground.CrossFadeAlpha(1.0f, duration, true);

			StartCoroutine(spriteBagTimer());

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

        movementSpeed = (movement.magnitude * magnMultiplier) + origMovementSpeed;

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
            // WALK / SPRINT ACCORDING JOYSTICK PRESSURE
            /*
            if (movement.magnitude > 1.6)
            {
                catAnim.SetBool("isSprinting", true);
                movementSpeed = sprintSpeed;
            }

            else if (horizontal > joystickPressure || horizontal < -joystickPressure || vertical > joystickPressure || vertical < -joystickPressure || movementSpeed > 5)
            {
                catAnim.SetBool("isSprinting", true);
                movementSpeed = sprintSpeed;
            }

            else
            {
                catAnim.SetBool("isSprinting", false);
                movementSpeed = origMovementSpeed;
            }
            */
            // WALK / SPRINT WITH THE USE OF A BUTTON

            
			if (Input.GetButton ("Sprint") && movement.magnitude > 0.1) 
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

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "trap") {        
			breakableObject trap = col.collider.transform.GetComponent<breakableObject> ();
			trap.makeSound = true;
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
    public void resetKeys()
    {
        
        keyPossessed = new int[3];
    }
}