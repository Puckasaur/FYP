using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour 
{
    //-----------------------//
    // initializing variables//
    //-----------------------//
    public GameObject bone;
    GameObject boneSpawner;
    GameObject newBone;
    public float playerVelocity = 0f;
    static float playerAcceleration = 0.7f;
    static float playerDecceleration = 0.5f;
    public float xVelocity = 0;
    public float yVelocity = 0;
    public float zVelocity = 0;
    public float Gravity = 10.0f;
    public float jumpTime = 0.0f;
    public float maxJumpTime = 0.5f;
    public float jumpCooldown = 1;
    public float timeNotJumping = 0;
    float pushForce = 0.0f;
    float throwForce = 00.00010f;
    int bones = 2;
	// Use this for initialization
	void Start () 
    {
        boneSpawner = GameObject.FindGameObjectWithTag("boneSpawner");
	}
	// Update is called once per frame
	void Update () 
    {
        //--------------------------------------//
        // creating the velocities to the player//
        //--------------------------------------//
        CharacterController controller = GetComponent<CharacterController>();
        if (Input.GetKey(KeyCode.W) && zVelocity < 1.0f)
            zVelocity += playerAcceleration * Time.deltaTime;
        else if (zVelocity > 0.0f)
            zVelocity -= playerDecceleration * Time.deltaTime;
        if (Input.GetKey(KeyCode.S) && zVelocity > -1.0f)
            zVelocity -= playerAcceleration * Time.deltaTime;
        else if (zVelocity < -0.0f)
            zVelocity += playerDecceleration * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) && xVelocity > -1.0f)
            xVelocity -= playerAcceleration * Time.deltaTime;
        else if (xVelocity < -0.0f)
            xVelocity += playerDecceleration * Time.deltaTime;
        if (Input.GetKey(KeyCode.D) && xVelocity < 1.0f)
            xVelocity += playerAcceleration * Time.deltaTime;
        else if (xVelocity > 0.0f)
            xVelocity -= playerDecceleration * Time.deltaTime;
        //-------------------//
        // jumping with space//
        //-------------------//
        if (Input.GetKey(KeyCode.Space)&& jumpTime < maxJumpTime)
        {
            jumpTime += Time.deltaTime;   
            yVelocity = 0.1f;
        }
        //-------------------//
        // simulating Gravity//
        //-------------------//
        else 
        {
            if (yVelocity > -2.0f) 
                yVelocity -= Gravity * Time.deltaTime;
            timeNotJumping += Time.deltaTime;
            if (timeNotJumping >= jumpCooldown)
            {
                jumpTime = 0;
                timeNotJumping = 0; 
            }
                
        }
        if(Input.GetKeyDown(KeyCode.T)&& bones > 0)
        {

            newBone = (GameObject)Instantiate(bone, boneSpawner.transform.position, Quaternion.identity);
            newBone.GetComponent<Rigidbody>().AddForce(this.transform.forward * throwForce + this.transform.up * (throwForce / 2));
        }
        //------------------------------------------------------------//
        // cuts the small details of Movement to exclude idle Movement//
        //------------------------------------------------------------//
        if (zVelocity < 0.01f && zVelocity > -0.01f) { zVelocity = 0.0f; }
        if (xVelocity < 0.01f && xVelocity > -0.01f) { xVelocity = 0.0f; }

        //-----------------------------------------//
        // and finally moving according to velocity//
        //-----------------------------------------//
        Vector3 Movement;
        Movement.x = xVelocity;
        Movement.y = yVelocity;
        Movement.z = zVelocity;
        controller.Move(Movement);

	}
    //----------------//
    // Pushing objects//
    //----------------//
    void OnControllerColliderHit(ControllerColliderHit Hit)
    {
        if(Input.GetKey(KeyCode.Return))
        {
            Rigidbody body = Hit.collider.attachedRigidbody;
            if (body == null) { return; }
                    
            if (Hit.gameObject.tag == "ball")
            {   
                body.isKinematic = false;
                pushForce = 2.0f;
                Vector3 pushDir = new Vector3(1, 0, 1);
                body.velocity = pushDir * pushForce;
                Hit.gameObject.SendMessage("ObjectFalling", SendMessageOptions.DontRequireReceiver);
            }
            if(Hit.gameObject.tag == "cube")
            {
                body.isKinematic = false;
                pushForce = 50.0f;
                Vector3 pushDir = new Vector3(1, 0, 1);
                body.AddForce(pushDir * pushForce);
                Hit.gameObject.SendMessage("ObjectFalling", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
