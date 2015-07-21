using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TemporaryUI : MonoBehaviour 
{
    public Text boneVent;
	
	void Start () 
    {
        boneVent.enabled = false;
	}

    void onTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            boneVent.enabled = true;
        }
    }

    void onTriggerExit()
    {
        boneVent.enabled = false;
    }
}
