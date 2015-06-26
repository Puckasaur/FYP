using UnityEngine;
using System.Collections;

public class ringOfSmell : MonoBehaviour {
    enemyPathfinding script;

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
        GetComponent<Rigidbody>().WakeUp();
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
                if (script.States != enumStates.alert)
                { transform.parent.LookAt(player.transform); }
                if(script.States == enumStates.alert)
                {
                    playerSeen = false;
                }

            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            player = other.gameObject;

                playerSeen = true;

        }
    }

    void OnTriggerStay(Collider other)
    {
        
        //-----------------------------------------------------------------------//
        //if player crosses the cone, informs the parent(Enemy) of visible player//
        //-----------------------------------------------------------------------//
        if (other.gameObject.tag == "player")
        {
			print("hello");
            //print("hello");
            //script.stateManager(2);

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
            
            if (script.States != enumStates.chase && script.States != enumStates.alert)
            {
				detectionTimer = 60.0f;
                script.alertTimer = 500;
				if(script.alertArea[script.areaCounter] != null)
				{
				script.currentTarget = script.alertArea[script.areaCounter];
				}
               
				print( script.currentTarget + " << ringOfSmell Target");
                if(script.areaCounter >2)
                {
                    script.areaCounter = 0;
                }
                script.stateManager(3);
            }

        }
    }
}
