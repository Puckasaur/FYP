using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour
{
	sfxPlayer SFX;

	public GameObject showLock;

    public int doorNumber; // number of the door (opened by a key with the same number)

    private bool opening = false; // checks if the door is opened or not

    private Animator m_Animator;

    
	void Start ()
	{
        m_Animator = GetComponent<Animator>();
		//showLock = GetComponentInChildren<SpriteRenderer>();
		showLock.SetActive(false);

		SFX = GameObject.Find("SFX").GetComponent<sfxPlayer>();

		//showLock.GetComponent<SpriteRenderer>().enabled = false;
	}
    
     
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {	
			inventory.inventoryArray[1]--;

			showLock.SetActive(true);

			//showLock.GetComponent<SpriteRenderer>().enabled = true;
			

            for (int j = 0; j < other.GetComponent<TemporaryMovement>().numberOfKeys; j++) // checks all the keys possessed by the player and if one corresponds with the door he wants to open
            {

                if (other.GetComponent<TemporaryMovement>().keyPossessed[j] == doorNumber && opening == false)
                {
					showLock.SetActive(false);
					//showLock.enabled = false;
                    opening = true;
                    m_Animator.SetBool("DoorOpen", true);

					

					SFX.playUnlock();
                    //Destroy(this.gameObject, 0.1f);
                    //this.transform.Rotate(new Vector3(0.0f, 0.0f, zRotation), angle, Space.Self);
                }

				if (other.GetComponent<TemporaryMovement>().numberOfKeys <= 0)
				{
					//showLock.SetActive(true);
					//showLock.GetComponent<SpriteRenderer>().enabled = true;
				}
            }
        	
		}
    }
    public void checkpoint()
    {
        m_Animator.SetBool("DoorOpen", false);
        opening = false;
    }
}