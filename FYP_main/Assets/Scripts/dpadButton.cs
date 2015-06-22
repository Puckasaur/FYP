using UnityEngine;
using System.Collections;

public class dpadButton : MonoBehaviour {

	public static bool up;
	public static bool down;
	public static bool left;
	public static bool right;

	float lastX;
	float lastY;

	public dpadButton()
	{
		up = down = left = right = false;
		lastX = Input.GetAxis("DPadX");
		lastY = Input.GetAxis("DPadY");
	}
 
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{

		if(Input.GetAxis ("DPadX") == 1 && lastX != 1) 
		{ 
			right = true; } else { right = false; 
		}

		if(Input.GetAxis ("DPadX") == -1 && lastX != -1) 
		{ 
			left = true; } else { left = false; 
		}

		if(Input.GetAxis ("DPadY") == 1 && lastY != 1) 
		{ 
			up = true; } else { up = false; 
		}

		if(Input.GetAxis ("DPadY") == -1 && lastY != -1) 
		{
			down = true; } else { down = false; 
		}
	
	}
}
