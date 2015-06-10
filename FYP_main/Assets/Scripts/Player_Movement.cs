using UnityEngine;
using System.Collections;

public class Player_Movement : MonoBehaviour 
{
    // just initializing variables
    public float PLAYER_VELOCITY = 0f;
    static float PLAYER_ACCELERATION = 0.7f;
    static float PLAYER_DECCELERATION = 0.5f;
    public float X_Velocity = 0;
    public float Y_Velocity = 0;
    public float Z_Velocity = 0;
    public float gravity = 10.0f;
    public float jump_Time = 0.0f;
    public float max_Jump_Time = 0.5f;
    public float jump_cooldown = 1;
    public float time_not_jumping = 0;
    float pushForce = 0.0f;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        // creating the velocities to the player
        CharacterController controller = GetComponent<CharacterController>();
        if (Input.GetKey(KeyCode.W) && Z_Velocity < 1.0f)
            Z_Velocity += PLAYER_ACCELERATION * Time.deltaTime;
        else if (Z_Velocity > 0.0f)
            Z_Velocity -= PLAYER_DECCELERATION * Time.deltaTime;
        if (Input.GetKey(KeyCode.S) && Z_Velocity > -1.0f)
            Z_Velocity -= PLAYER_ACCELERATION * Time.deltaTime;
        else if (Z_Velocity < -0.0f)
            Z_Velocity += PLAYER_DECCELERATION * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) && X_Velocity > -1.0f)
            X_Velocity -= PLAYER_ACCELERATION * Time.deltaTime;
        else if (X_Velocity < -0.0f)
            X_Velocity += PLAYER_DECCELERATION * Time.deltaTime;
        if (Input.GetKey(KeyCode.D) && X_Velocity < 1.0f)
            X_Velocity += PLAYER_ACCELERATION * Time.deltaTime;
        else if (X_Velocity > 0.0f)
            X_Velocity -= PLAYER_DECCELERATION * Time.deltaTime;
        // jumping with space
        if (Input.GetKey(KeyCode.Space)&& jump_Time < max_Jump_Time)
        {
            jump_Time += Time.deltaTime;   
            Y_Velocity = 0.1f;
        }
        // and gravity
        else 
        {
            if (Y_Velocity > -2.0f) 
                Y_Velocity -= gravity * Time.deltaTime;
            time_not_jumping += Time.deltaTime;
            if (time_not_jumping >= jump_cooldown)
            {
                jump_Time = 0;
                time_not_jumping = 0; 
            }
                
        }
        // cuts the small details of movement, to exclude idle movement
        if (Z_Velocity < 0.01f && Z_Velocity > -0.01f) { Z_Velocity = 0.0f; }
        if (X_Velocity < 0.01f && X_Velocity > -0.01f) { X_Velocity = 0.0f; }


        // and finally moving according to velocity
        Vector3 movement;
        movement.x = X_Velocity;
        movement.y = Y_Velocity;
        movement.z = Z_Velocity;
        controller.Move(movement);

	}
    // Pushing objects
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(Input.GetKey(KeyCode.Return))
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            if (body == null) { return; }
                    
            if (hit.gameObject.tag == "ball")
            {   
                body.isKinematic = false;
                Debug.Log("Toimii");
                pushForce = 2.0f;
                Vector3 pushDir = new Vector3(1, 0, 1);
                body.velocity = pushDir * pushForce;
                hit.gameObject.SendMessage("ObjectFalling", SendMessageOptions.DontRequireReceiver);
            }
            if(hit.gameObject.tag == "cube")
            {
                body.isKinematic = false;
                Debug.Log("Toimii");
                pushForce = 50.0f;
                Vector3 pushDir = new Vector3(1, 0, 1);
                body.AddForce(pushDir * pushForce);
                hit.gameObject.SendMessage("ObjectFalling", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
