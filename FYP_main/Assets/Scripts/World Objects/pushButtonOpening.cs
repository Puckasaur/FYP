// This script goes on the door that has to be opened. Drag and drop the two buttons in the inspector.
// Not a very flexible script but I guess doors that will require more than two triggers will not exist...

using UnityEngine;
using System.Collections;

public class pushButtonOpening : MonoBehaviour
{
    public GameObject[] triggerButtons;

    public bool reguire_Both = false;

    private int count;

    private Animator m_Animator;

    void Start ()
    {
        m_Animator = GetComponent<Animator>();

        foreach (GameObject buttons in triggerButtons)
        {
            buttons.GetComponent<pushButton>();
        }
    }

	void Update () 
    {
        if(triggerButtons.Length == 1 && triggerButtons[0].GetComponent<pushButton>().buttonActivated == true)
        {
            m_Animator.SetBool("DoorOpen", true);
        }

        else if(triggerButtons.Length > 1)
        { 
            
            if(triggerButtons[1].GetComponent<pushButton>().buttonActivated == true && reguire_Both == false)
            {
                m_Animator.SetBool("DoorOpen", true);
            }

            else if(triggerButtons[0].GetComponent<pushButton>().buttonActivated == true && reguire_Both == false)
            {
                m_Animator.SetBool("DoorOpen", true);
            }

            else if (triggerButtons[0].GetComponent<pushButton>().buttonActivated == true && triggerButtons[1].GetComponent<pushButton>().buttonActivated == true && reguire_Both == true)
            {
                m_Animator.SetBool("DoorOpen", true);
            }

            else m_Animator.SetBool("DoorOpen", false);
        }

        else
        {
            m_Animator.SetBool("DoorOpen", false);
        }
	}
}