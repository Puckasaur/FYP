using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class hidingThirdPerson : MonoBehaviour {

	private OnScreenInstructionChild onScreenChild;
	private TemporaryMovement tmpMovement;
	private ringOfSmell ros;

	private checkPoint cp;

    public Transform character;
    public Transform prevPosition;
    public Transform hidingPosition;

   	public GameObject checkToEnter;
	public GameObject checkToExit;

    public bool isHiding;
	public bool isPaused;

	void Start () 
	{
        //isHiding = false;
        //isPaused = false;

		tmpMovement = GameObject.Find ("Char_Cat").GetComponent<TemporaryMovement>();
		cp = GameObject.Find ("Char_Cat").GetComponent<checkPoint>();
		ros = GameObject.Find ("ring of Smell").GetComponent<ringOfSmell>();
		onScreenChild = GameObject.Find ("TriggerArea").GetComponent<OnScreenInstructionChild>();

	}

    void OnTriggerStay(Collider catType)
    {	
		if (catType.tag == "player") 
		{
			checkToEnter.SetActive(true);
			onScreenChild.sprintIns.SetActive(false);
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
		checkToEnter.SetActive(false);
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
				onScreenChild.sprintIns.SetActive(false);
				
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

		checkToEnter.SetActive(false);
		checkToExit.SetActive(true);
        
	}
	
	IEnumerator Delayed()
	{
        yield return new WaitForSeconds(0.1f);

        character.transform.position = prevPosition.transform.position;
		checkToExit.SetActive(false);
   
	}

}
