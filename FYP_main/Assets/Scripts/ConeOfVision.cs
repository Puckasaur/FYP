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
    void OnTriggerEnter(Collider other)
    {
        //if player crosses the cone, informs the parent(Enemy) of visible player
        if (other.gameObject.tag == "Player")
        {
            this.gameObject.transform.parent.SendMessage("m_chasePlayer", SendMessageOptions.DontRequireReceiver);
        }        
    }

}
