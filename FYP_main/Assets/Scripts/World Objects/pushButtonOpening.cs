// This script goes on the door that has to be opened. Drag and drop the two buttons in the inspector.
// Not a very flexible script but I guess doors that will require more than two triggers will not exist...

using UnityEngine;
using System.Collections;

public class pushButtonOpening : MonoBehaviour
{
    public GameObject[] triggerButtons;

    private Renderer rend;
    private Collider coll;

    void Start ()
    {
        rend = this.GetComponent<Renderer>();
        coll = this.GetComponent<Collider>();

        foreach (GameObject buttons in triggerButtons)
        {
            buttons.GetComponent<pushButton>();
        }
    }

	void Update () 
    {
        if (triggerButtons[0].GetComponent<pushButton>().buttonActivated == true)
        {
            if (triggerButtons.Length > 1)
            {
                if (triggerButtons[1].GetComponent<pushButton>().buttonActivated == true)
                {
                    this.rend.enabled = false;
                    this.coll.enabled = false;
                }
            }

            else
            {
                this.rend.enabled = false;
                this.coll.enabled = false;
            }
        }

        else
        {
            this.rend.enabled = true;
            this.coll.enabled = true;
        }
	}
}
