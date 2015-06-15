using UnityEngine;
using System.Collections;

public class ConeOfVision : MonoBehaviour 
{

    void OnTriggerStay(Collider other)
    {
        //if player crosses the cone, informs the parent(Enemy) of visible player
        if (other.gameObject.tag == "Player")
        {
           RaycastHit hit;
            if (Physics.Linecast(transform.parent.position, other.transform.position,out hit))
                if(hit.collider == other)
                    this.gameObject.transform.parent.SendMessage("stateManager", 2, SendMessageOptions.DontRequireReceiver);
            Debug.Log(hit.collider);
        }     
    }

}
