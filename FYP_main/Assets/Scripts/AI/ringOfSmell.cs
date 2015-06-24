using UnityEngine;
using System.Collections;

public class ringOfSmell : MonoBehaviour {
    enemyPathfinding script;
<<<<<<< HEAD

=======
    public float radius;
    bool playerSeen = false;
    Vector3 scalingRate = new Vector3(1.0f, 0.0f, 1.0f);
    public float detectionTimer = 60.0f;
    GameObject player;
    RaycastHit hit;
    
    
    void Start()
    {
        script = this.transform.parent.GetComponent<enemyPathfinding>();
    }
    void Update()
    {
        if (this.transform.localScale.x < radius)
        {
            this.transform.localScale += scalingRate;
        }
        if (playerSeen)
        {

            Physics.Linecast(transform.parent.position, player.transform.position, out hit);
            print(hit.collider);
            if (hit.collider == player.GetComponent<Collider>())
            {

                transform.parent.LookAt(player.transform);
                detectionTimer--;

            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            player = other.gameObject;
            script.escapeTimer = 0;
            if(script.States != enumStates.chase)
            {
                playerSeen = true;
                transform.parent.LookAt(other.transform);
            }
        }
    }
>>>>>>> origin/Toni_Sound&Vision
    void OnTriggerStay(Collider other)
    {
        //-----------------------------------------------------------------------//
        //if player crosses the cone, informs the parent(Enemy) of visible player//
        //-----------------------------------------------------------------------//
        if (other.gameObject.tag == "player")
        {
<<<<<<< HEAD
            script = this.transform.parent.GetComponent<enemyPathfinding>();
            script.stateManager(2);
=======
            detectionTimer--;

            if (detectionTimer <= 0)
            {
                detectionTimer = 60;
                script.stateManager(2);
                //script.stateManager(2);
            }

        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "player")
        {
            detectionTimer = 60.0f;
            if (script.States != enumStates.chase)
            {
                script.alertTimer = 500;
                script.currentTarget = script.alertArea[1];
                script.stateManager(3);
            }
>>>>>>> origin/Toni_Sound&Vision
        }
    }
}
