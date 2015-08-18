using UnityEngine;
using System.Collections;

public class DisplayInstruction : MonoBehaviour
{
	private SpriteRenderer instruction;
	private SpriteRenderer doorInstruction;

	private Animator animate;
	private Animator doorAnimate;

	public Sprite keyboard;
	public Sprite controller;

	public RuntimeAnimatorController keyboard1;
	public RuntimeAnimatorController controller1;

	private float maximum = 1.0f;
	private float minimum = 0.0f;
	private float duration = 5.0f;
	private float startTime;

	void Start () 
    {
		startTime = Time.time;

		doorAnimate = GameObject.Find("OnScreenInstruction").GetComponent<Animator>();
		doorInstruction = GameObject.Find("OnScreenInstruction").GetComponent<SpriteRenderer>();

		animate = GetComponentInChildren<Animator>();
		instruction = GetComponentInChildren<SpriteRenderer>();
	
		if (instruction.enabled == true)
			instruction.enabled = false;
		
	}

	void OnTriggerEnter(Collider fadingText)
	{
		if (fadingText.gameObject.tag == "player") 
		{
			StartCoroutine(FadeIn());

			instruction.enabled = true;
		}
	}

	void OnTriggerExit(Collider fadingText)
    {
		if (fadingText.gameObject.tag == "player") 
		{
			StartCoroutine(FadeOut());
		}
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
			instruction.sprite = controller; 
			animate.runtimeAnimatorController = controller1;
		}
		
		// joystick axis
		if(Input.GetAxis("horizontalCheck") != 0.0f || Input.GetAxis("verticalCheck") != 0.0f)
		{
			instruction.sprite = controller; 
			animate.runtimeAnimatorController = controller1;

		}
	}

	IEnumerator FadeOut()
	{	
		instruction.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.8f);
		
		yield return new WaitForSeconds(0.06f);
		
		instruction.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.6f);
		
		yield return new WaitForSeconds(0.06f);
		
		instruction.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
		
		yield return new WaitForSeconds(0.06f);
		
		instruction.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
		
		yield return new WaitForSeconds(0.06f);
			
		
		instruction.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.0f);

		instruction.enabled = false;
	}
	
	IEnumerator FadeIn()
	{
		//yield return new WaitForSeconds(0.06f);
		
		instruction.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
		
		yield return new WaitForSeconds(0.06f);
		
		instruction.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
		
		yield return new WaitForSeconds(0.06f);
		
		instruction.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.6f);
		
		yield return new WaitForSeconds(0.06f);
		
		instruction.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.8f);

		yield return new WaitForSeconds(0.06f);
		
		instruction.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1.0f);
	}
}