using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class hidingThirdPerson : MonoBehaviour {

	private ringOfSmell ros;

	private checkPoint cp;

    public Transform character;
    public Transform prevPosition;
    public Transform hidingPosition;

   	public GameObject keyboardCheckToEnter;
	public GameObject keyboardCheckToExit;

	public GameObject controllerCheckToEnter;
	public GameObject controllerCheckToExit;

    public bool isHiding;
	public bool isPaused;

	void Start () 
	{
        //isHiding = false;
        //isPaused = false;
		
		cp = GameObject.Find ("Char_Cat").GetComponent<checkPoint>();
		ros = GameObject.Find ("ring of Smell").GetComponent<ringOfSmell>();

	}

    void OnTriggerStay(Collider catType)
    {	
		if (catType.tag == "player") 
		{
			keyboardCheckToEnter.SetActive(true);
			controllerCheckToEnter.SetActive(true);
			
			if (isHiding == false)
			{
				if (Input.GetButtonDown("Interact") || Input.GetKeyDown (KeyCode.E))
				{
					character.transform.position = hidingPosition.transform.position;
					
					StartCoroutine(Wait());
				}
			}
		}
    }

	void OnTriggerExit()
	{
		keyboardCheckToEnter.SetActive(false);
		controllerCheckToEnter.SetActive(false);
	}

	void Update (){ 

        if (isHiding == true) 
		{
			if (Input.GetButtonDown("Interact") || Input.GetKeyDown (KeyCode.E))
			{
				StartCoroutine (Delayed ());

				isHiding = false;
				isPaused = false;            
                if(ros.disguised == true)
            	ros.isNotDisguised("htp");
				
			}
		} 

		if (cp.sendBack == true) {
			isHiding = false;
			isPaused = false;    
			
			//checkToEnter.enabled = false;
			//checkToExit.enabled = false;

			cp.sendBack = false;
		}
	}

    IEnumerator Wait()
	{

        yield return new WaitForSeconds(0.1f);

		isPaused = true;
        isHiding = true;

        if(ros.disguised == false)
        	ros.isDisguised("htp");

		keyboardCheckToEnter.SetActive(false);
		keyboardCheckToExit.SetActive(true);

		controllerCheckToEnter.SetActive(false);
		controllerCheckToExit.SetActive(true);
        
	}
	
	IEnumerator Delayed()
	{
        yield return new WaitForSeconds(0.1f);

        character.transform.position = prevPosition.transform.position;
		controllerCheckToExit.SetActive(false);
		keyboardCheckToExit.SetActive(false);
		
   
	}

}
