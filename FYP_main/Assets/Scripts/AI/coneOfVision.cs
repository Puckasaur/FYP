using UnityEngine;
using System.Collections;

public class coneOfVision : MonoBehaviour
{
    fatDogAi scriptFatDog;
    enemyPathfinding script;
    huntingDog scriptHuntingDog;
	chaseTransition chaseTransScript;
    RaycastHit hit;
    public bool playerSeen;
    float width;
    public float startWidth;
    float height;
    public float startHeight;
    float range;
    public float startRange;
    public float alarmBonus;
    public float detectionTimer = 60.0f;
    void Start()
    {
		chaseTransScript = GameObject.Find ("BGM").GetComponent<chaseTransition>();

        range = startRange;
        width = startWidth;
        height = startHeight;


        //if (transform.parent.tag == "enemy")
        //{
            if (this.transform.parent.GetComponent<enemyPathfinding>() != null)
            {
                script = this.transform.parent.GetComponent<enemyPathfinding>();
            }

            if (this.transform.parent.GetComponent<fatDogAi>() != null)
            {
                scriptFatDog = this.transform.parent.GetComponent<fatDogAi>();
            }
            if(transform.parent.GetComponent<huntingDog>() != null)
            {
                scriptHuntingDog = transform.parent.GetComponent<huntingDog>();
            }

            
        //}
        width = startWidth;
        height = startHeight;
        range = startRange;
    }
    void Update()
    {
            GetComponent<Rigidbody>().WakeUp();

            if (transform.localScale.x < width)
            {
                transform.localScale = new Vector3(width, height, range);
            }
            else if (transform.localScale.x > width)
            {
                transform.localScale = new Vector3(width, height, range);
            }
    }

    void OnTriggerStay(Collider other)
    {

        //-----------------------------------------------------------------------//
        //if player crosses the cone, informs the parent(Enemy) of visible player//
        //-----------------------------------------------------------------------//
        if (other.gameObject.tag == "player")
        {
            RaycastHit hit;
            Physics.Linecast(transform.parent.position, other.transform.position, out hit);
            if (hit.collider == other)
            {
				chaseTransScript.chaseTrans();
                if (script != null)
                {                   
                    script.stateManager(2);
                }
                else if (scriptFatDog != null)
                {
                    playerSeen = true;
                    scriptFatDog.stateManager(2);
                }
                else if (scriptHuntingDog != null)
                {
                    scriptHuntingDog.stateManager(2);
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            if (Physics.Linecast(transform.parent.position, other.transform.position, out hit))
            {
                if (hit.collider == other)
                {
					chaseTransScript.outChaseTrans();
                    if (transform.parent.tag == "patrolDog")
                    {
                        if (script.States != enumStates.chase)
                        {
                            script.areaCounter = 0;

                            script.stateManager(3);
                        }
                    }
                    else if (transform.parent.tag == "fatDog")
                    {
                        if (scriptFatDog.States != enumStatesFatDog.chase)
                        {                         
                            playerSeen = false;
                            scriptFatDog.stateManager(3);
                        }
                    }
                    else if (transform.parent.tag == "huntingDog")
                    {
                        if (scriptHuntingDog.statesHunter != enumStatesHunter.chase)
                        {
                            scriptHuntingDog.areaCounter = 0;
                            scriptHuntingDog.stateManager(3);
                        }
                    }
                }
            }
        }
    }
}
