using UnityEngine;
using System.Collections;

public class ringOfSmell : MonoBehaviour {
    enemyPathfinding script;

    void OnTriggerStay(Collider other)
    {
        //-----------------------------------------------------------------------//
        //if player crosses the cone, informs the parent(Enemy) of visible player//
        //-----------------------------------------------------------------------//
        if (other.gameObject.tag == "player")
        {
            script = this.transform.parent.GetComponent<enemyPathfinding>();
            script.stateManager(2);
        }
    }
}
