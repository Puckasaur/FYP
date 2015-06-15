using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HideTP : MonoBehaviour {

    public Transform character;
    public Transform prevPosition;
    public Transform hidingPosition;

    public Text onScreenInstruction;
    public Text onScreenInstructionExit;

    private bool isHiding;

	// Use this for initialization
	void Start () {
        isHiding = false;

        onScreenInstruction.enabled = false;
        onScreenInstructionExit.enabled = false;
	}

    void OnTriggerStay()
    {
        onScreenInstruction.enabled = true;

        if (isHiding == false)
        {
            if (Input.GetKeyDown("e"))
            {
                character.transform.position = hidingPosition.transform.position;

				
				StartCoroutine(Wait());
            }
        }
    }

	// Update is called once per frame
	void Update () 
    {
        if (isHiding == true) {
			if (Input.GetKeyDown ("e"))
			{
				StartCoroutine (Delayed ());

				isHiding = false;
       
			}
		}
	}

	void OnTriggerExit()
	{

	}

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.1f);
        isHiding = true;
        onScreenInstruction.enabled = false;
        onScreenInstructionExit.enabled = true;
		
		GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity = Vector3.zero;
	}
	
	IEnumerator Delayed()
    {
        yield return new WaitForSeconds(0.1f);
        character.transform.position = prevPosition.transform.position;
        onScreenInstructionExit.enabled = false;

    }

}
