using UnityEngine;
using System.Collections;

public class ConeOfVision : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {

	}
    void OnTriggerStay(Collider other)
    {
        //if player crosses the cone, informs the parent(Enemy) of visible player
        if (other.gameObject.tag == "Player")
        {
            this.gameObject.transform.parent.SendMessage("playerSpotted", SendMessageOptions.DontRequireReceiver);
        }        
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            this.gameObject.transform.parent.SendMessage("patrol", SendMessageOptions.DontRequireReceiver);
        }
    }
}
