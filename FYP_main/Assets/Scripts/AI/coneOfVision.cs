using UnityEngine;
using System.Collections;

public class coneOfVision : MonoBehaviour
{
    fatDogAi scriptFatDog;
    enemyPathfinding script;
    huntingDog scriptHuntingDog;
    chaseTransition chaseTransScript;
    RaycastHit hit;
    Transform parent;
    public bool disguised;
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
        chaseTransScript = GameObject.Find("BGM").GetComponent<chaseTransition>();

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
        if (transform.parent.GetComponent<huntingDog>() != null)
        {
            scriptHuntingDog = transform.parent.GetComponent<huntingDog>();
        }
        parent = this.transform.parent;

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

	void OnTriggerEnter (Collider other)
	{
        if (other.gameObject.tag == "player")
        {
            Physics.Linecast(transform.position, other.transform.position, out hit);
            Debug.DrawLine(transform.position, other.transform.position, Color.green);
            if (hit.collider.tag == "player")//other.tag)
            {
                chaseTransScript.playSting();
            }
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
            if(parent.position.y <= 1)
            {
                //parent.transform.;
            }
            Physics.Linecast(transform.position, other.transform.position, out hit);
            Debug.DrawLine(transform.position, other.transform.position, Color.green);
            if (hit.collider.tag == "player")//other.tag)
            {
                chaseTransScript.chaseTrans();

                if (script != null)
                {
                    playerSeen = true;
                    script.stateManager(2);
                }
                else if (scriptFatDog != null)
                {
                    playerSeen = true;
                    scriptFatDog.stateManager(2);
                }
                else if (scriptHuntingDog != null)
                {
                    playerSeen = true;
                    scriptHuntingDog.stateManager(2);
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (playerSeen)
        {
            if (other.gameObject.tag == "player")
            {
                chaseTransScript.outChaseTrans();

                if (Physics.Linecast(transform.parent.position, other.transform.position, out hit))
                {
                    if (hit.collider.tag == "player")//other)
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
                playerSeen = false;

            }
        }
    }
    public void isDisguised()
    {
        width = 0;
        height = 0;
        range = 0;
    }
    public void isNotDisguised()
    {
        width = startWidth;
        height = startHeight;
        range = startRange;
    }
}