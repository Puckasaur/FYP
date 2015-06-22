using UnityEngine;
using System.Collections;

public class coneOfVision : MonoBehaviour 
{
    enemyPathfinding Script;
    void OnTriggerStay(Collider other)
    {
        //-----------------------------------------------------------------------//
        //if player crosses the cone, informs the parent(Enemy) of visible player//
        //-----------------------------------------------------------------------//
        if (other.gameObject.tag == "player")
        {
           RaycastHit hit;
           if (Physics.Linecast(transform.parent.position, other.transform.position, out hit))
               if (hit.collider == other)
               {
                   Script = this.transform.parent.GetComponent<enemyPathfinding>();
                   Script.escapeTimer = 0;
                   Script.stateManager(2);
                   Debug.Log(hit);
               }
        }     
    }

}
