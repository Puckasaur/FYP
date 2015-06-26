using UnityEngine;
using System.Collections;

public class TemporaryMovement : MonoBehaviour 
{
	public float movementSpeed;
	public float turningSpeed;
    public float jumpHeight;

    public Camera mainCamera;
    /* Right position/rotation =
     * -2, 7.3, -4.4
     * 51, 53, 0
     * 
     * Left position/rotation =
     * -2, 7.3, 4.4
     * 51, 106, 0

     */
     
    private Vector3 offset;

    void Start()
    {
        //offset = this.transform.position - mainCamera.transform.position;
    }

	void FixedUpdate() 
    {
        /*
        float camAngleY = mainCamera.transform.rotation.y;
        AdaptMovementToCamera(camAngleY);
        */
        
        // Movement
        
        /*
		float horizontal = Input.GetAxis("Horizontal") * turningSpeed * Time.deltaTime;
        transform.Rotate(0, horizontal, 0);
        */

        float horizontal = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        transform.Translate(horizontal, 0, 0);

        float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
		transform.Translate(0, 0, vertical);

        /*
        if (horizontal != 0 || vertical != 0)
        {
            Vector3 position = Vector3.Lerp(mainCamera.transform.position, offset, Time.deltaTime * 1);
        }
        */
        
        // Jump
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space))
        {
            this.GetComponent<Rigidbody>().velocity += new Vector3(0.0f, jumpHeight, 0.0f);
        }

        // Anti-jump
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            this.GetComponent<Rigidbody>().velocity -= new Vector3(0.0f, jumpHeight, 0.0f);
        }

        if (Input.GetKeyDown(KeyCode.Q) && mainCamera.transform.rotation.y < 106)
        {
            mainCamera.transform.position = new Vector3(-2.0f, 7.3f, -4.4f);
            //mainCamera.transform.eulerAngles = new Vector3(51.0f, 106.0f, 0.0f);
            mainCamera.transform.Rotate(51.0f, 106.0f, 0.0f);
        }

        if (Input.GetKeyDown(KeyCode.Q) && mainCamera.transform.rotation.y > 53)
        {
            mainCamera.transform.position = new Vector3(0.0f, 0.0f, 4.4f);
            mainCamera.transform.Rotate(51.0f, 53.0f, 0.0f);
        }
	}

    /*
    private Quaternion Quaternion(float p, float horizontal, int p_2, int p_3)
    {
        throw new System.NotImplementedException();
    }

    void AdaptMovementToCamera(float camAngleY)
    {
        
    }
    */
}