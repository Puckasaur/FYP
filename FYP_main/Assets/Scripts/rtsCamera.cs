using UnityEngine;
using System.Collections;

public class rtsCamera : MonoBehaviour {

	public GameObject target = null;
	public bool orbitY = false;

	private Vector3 positionOffSet = Vector3.zero;

	void Start()
	{
		positionOffSet = transform.position - target.transform.position;
	}

	// Update is called once per frame
	void Update () 
	{
		float mousePosX = Input.mousePosition.x;
		float mousePosY = Input.mousePosition.y;

		int scrollDistance = 100;
		float scrollSpeed = 3 * Camera.main.orthographicSize + 2;

		Vector3 aPosition = new Vector3 (0, 0, 0);

		float scrollAmount = scrollSpeed * Time.deltaTime;

		const float orthographicSizeMin = 15f;
		const float orthographicSizeMax = 256f;
/*
		if (target != null) 
		{
			transform.LookAt(target.transform); 

			if (orbitY)
			{
				transform.RotateAround (target.transform.position, Vector3.up, Time.deltaTime * 15);		
			}

			transform.position = target.transform.position + positionOffSet;
		}
*/

		/////////////////////////////////////////////////////////////////////////////////////
										     //MOUSE//
		/////////////////////////////////////////////////////////////////////////////////////

		// Mouse Left
		if ((mousePosX < scrollDistance) && (transform.position.x > -480))
		{ 
			transform.Translate (-scrollAmount,0,0, Space.World); 
		} 
		//	Mouse Right
		if ((mousePosX >= Screen.width - scrollDistance) && (transform.position.x < 480))
		{ 
			transform.Translate (scrollAmount,0,0, Space.World);  
		}
		// Mouse Down
		if ((mousePosY < scrollDistance) && (transform.position.z > -240))
		{ 
			transform.Translate (0,0,-scrollAmount, Space.World); 
		} 
		// Mouse Up
		if ((mousePosY >= Screen.height - scrollDistance) && (transform.position.z < 240))
		{ 
			transform.Translate (0,0,scrollAmount, Space.World); ; 
		}

		/////////////////////////////////////////////////////////////////////////////////////
											//KEYBOARD//
		//////////////////////////////////////////////////////////////////////////////////// 
	
		//Keyboard controls 
		if ((Input.GetKey(KeyCode.UpArrow)) && (transform.position.z < 240))
		{ 
			transform.Translate (0,0,scrollAmount, Space.World); ; 
		} 
		if ((Input.GetKey(KeyCode.DownArrow)) && (transform.position.z > -240))
		{ 
			transform.Translate (0,0,-scrollAmount, Space.World);
		}
		if ((Input.GetKey(KeyCode.LeftArrow)) && (transform.position.x > -240))
		{ 
			transform.Translate (-scrollAmount,0,0, Space.World);  
		} 
		if ((Input.GetKey(KeyCode.RightArrow)) && (transform.position.x < 240))
		{ 
			transform.Translate (scrollAmount,0,0, Space.World); 
		}

		/////////////////////////////////////////////////////////////////////////////////////
											//SCROLLING//
		//////////////////////////////////////////////////////////////////////////////////// 
	
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, orthographicSizeMin, orthographicSizeMax );
	

	}
}
