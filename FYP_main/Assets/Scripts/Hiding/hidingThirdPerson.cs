using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class hidingThirdPerson : MonoBehaviour {

	private TemporaryMovement tmpMovement;
	private ringOfSmell ros;

	private checkPoint cp;

    public Transform character;
    public Transform prevPosition;
    public Transform hidingPosition;

   	public Text checkToEnter;
    public Text checkToExit;

    public bool isHiding;
	public bool isPaused;

	void Start () 
	{
        //isHiding = false;
        //isPaused = false;

		tmpMovement = GameObject.Find ("Char_Cat").GetComponent<TemporaryMovement>();
		cp = GameObject.Find ("Char_Cat").GetComponent<checkPoint>();
		ros = GameObject.Find ("ring of Smell").GetComponent<ringOfSmell>();
	}

    void OnTriggerStay(Collider catType)
    {	
		if (catType.tag == "player") 
		{
			checkToEnter.enabled = true;
			
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
		checkToEnter.enabled = false;
	}

	void Update () 
    {

		//Debug.Log ("checkToExit: " + checkToEnter.enabled);


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

		checkToEnter.enabled = false;
		checkToExit.enabled = true;
        
	}
	
	IEnumerator Delayed()
	{
        yield return new WaitForSeconds(0.1f);

        character.transform.position = prevPosition.transform.position;
		checkToExit.enabled = false;
   
	}

}
