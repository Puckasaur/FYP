using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class hidingThirdPerson : MonoBehaviour {

    public Transform character;
    public Transform prevPosition;
    public Transform hidingPosition;

    public Text onScreenInstruction;
    public Text onScreenInstructionExit;

    private bool isHiding;
	private bool isPaused;

	// Use this for initialization
	void Start () 
	{
        isHiding = false;
        isPaused = false;

        onScreenInstruction.enabled = false;
        onScreenInstructionExit.enabled = false;
	}

    void OnTriggerStay()
    {
	    onScreenInstruction.enabled = true;
	
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
		onScreenInstruction.enabled = false;
	}

	void Update () 
    {
        if (isHiding == true) 
		{
			if (Input.GetButtonDown ("Interact"))
			{
				StartCoroutine (Delayed ());

				isHiding = false;
				isPaused = false;;
				
			}
		}

		if (isPaused == true) 
		{
			//pause
			character.GetComponent<Rigidbody> ().isKinematic = true;

		} 
		else if (isPaused == false)
		{
			//unpause
			character.GetComponent<Rigidbody>().isKinematic = false;

		}

	}


    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.1f);

		isPaused = true;
        isHiding = true;

      	onScreenInstruction.enabled = false;
        onScreenInstructionExit.enabled = true;
	}
	
	IEnumerator Delayed()
	{
        yield return new WaitForSeconds(0.1f);

        character.transform.position = prevPosition.transform.position;
        onScreenInstructionExit.enabled = false;
   
	}

}
