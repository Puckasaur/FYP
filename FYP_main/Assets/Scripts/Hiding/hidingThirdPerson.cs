using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class hidingThirdPerson : MonoBehaviour {

	private TemporaryMovement tmpMovement;

    public Transform character;
    public Transform prevPosition;
    public Transform hidingPosition;

   	public Text checkToEnter;
    public Text checkToExit;

    private bool isHiding;
	private bool isPaused;

	//public Text hidingText;

	 //Use this for initialization
	void Start () 
	{
        isHiding = false;
        isPaused = false;

		tmpMovement = GameObject.Find ("Char_Cat").GetComponent<TemporaryMovement>();
	}

    void OnTriggerStay()
    {	
		checkToEnter.enabled = true;

        if (isHiding == false)
        {
			if (Input.GetButtonDown("Interact"))
			{
				character.transform.position = hidingPosition.transform.position;

				StartCoroutine(Wait());
            }
        }
    }

	void OnTriggerExit()
	{
		checkToEnter.enabled = false;
	}

	void Update () 
    {
        if (isHiding == true) 
		{
			if (Input.GetButtonDown ("Interact"))
			{
				StartCoroutine (Delayed ());

				isHiding = false;
				isPaused = false;
			}
		}

		if (isPaused == true) 
		{
			//pause
			character.GetComponent<Rigidbody>().isKinematic = true;
			tmpMovement.movementSpeed = 0;

		} 
		else if (isPaused == false)
		{
			//unpause
			character.GetComponent<Rigidbody>().isKinematic = false;
			tmpMovement.movementSpeed = tmpMovement.origMovementSpeed;
		}
	}
    IEnumerator Wait()
	{

        yield return new WaitForSeconds(0.1f);

		isPaused = true;
        isHiding = true;

		//checkToEnter.enabled = false;
		checkToExit.enabled = true;
	}
	
	IEnumerator Delayed()
	{
        yield return new WaitForSeconds(0.1f);

        character.transform.position = prevPosition.transform.position;
		checkToExit.enabled = false;
   
	}

}
