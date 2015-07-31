using UnityEngine;
using System.Collections;

public class DisplayInstruction : MonoBehaviour
{
	private SpriteRenderer instruction;
	private Animator animate;
	///public GameObject controller;

	public Sprite keyboard;
	public Sprite controller;

	public RuntimeAnimatorController keyboard1;
	public RuntimeAnimatorController controller1;


	void Start () 
    {
		animate = GameObject.Find("OnScreenInstruction_instruction").GetComponent<Animator>();
		instruction = GameObject.Find("OnScreenInstruction_instruction").GetComponent<SpriteRenderer>();
	
		instruction.enabled = false;
		//controller.SetActive (false);
	}
	
	void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "player")
			instruction.enabled = true;
	}

    void OnTriggerExit()
    {
		instruction.enabled = false;
		//controller.SetActive (false	);
	}
	
	void OnGUI()
	{
		isMouseKeyboard ();
		isControllerInput ();
	}	

	void isMouseKeyboard()
	{
		// mouse & keyboard buttons
		if (Event.current.isKey ||
		    Event.current.isMouse)
		{
			instruction.sprite = keyboard; 
			animate.runtimeAnimatorController = keyboard1;
		}
		// mouse movement
//		if( Input.GetAxis("Mouse X") != 0.0f ||
//		   Input.GetAxis("Mouse Y") != 0.0f )
//		{
//
//		}
	}
	
	void isControllerInput()
	{
		// joystick buttons
		if(Input.GetKey(KeyCode.Joystick1Button0)  ||
		   Input.GetKey(KeyCode.Joystick1Button1)  ||
		   Input.GetKey(KeyCode.Joystick1Button2)  ||
		   Input.GetKey(KeyCode.Joystick1Button3)  ||
		   Input.GetKey(KeyCode.Joystick1Button4)  ||
		   Input.GetKey(KeyCode.Joystick1Button5)  ||
		   Input.GetKey(KeyCode.Joystick1Button6)  ||
		   Input.GetKey(KeyCode.Joystick1Button7)  ||
		   Input.GetKey(KeyCode.Joystick1Button8)  ||
		   Input.GetKey(KeyCode.Joystick1Button9)  ||
		   Input.GetKey(KeyCode.Joystick1Button10) ||
		   Input.GetKey(KeyCode.Joystick1Button11) ||
		   Input.GetKey(KeyCode.Joystick1Button12) ||
		   Input.GetKey(KeyCode.Joystick1Button13) ||
		   Input.GetKey(KeyCode.Joystick1Button14) ||
		   Input.GetKey(KeyCode.Joystick1Button15) ||
		   Input.GetKey(KeyCode.Joystick1Button16) ||
		   Input.GetKey(KeyCode.Joystick1Button17) ||
		   Input.GetKey(KeyCode.Joystick1Button18) ||
		   Input.GetKey(KeyCode.Joystick1Button19) )
		{

		}
		
		// joystick axis
		if(Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f)
		{
			instruction.sprite = controller; 
			animate.runtimeAnimatorController = controller1;

		}
	}
}