using UnityEngine;
using System.Collections;

public class coneOfVision : MonoBehaviour
{

    enemyPathfinding script;
    //guardDog guard;
    RaycastHit hit;
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
        if (transform.parent.tag == "enemy")
        {
            script = this.transform.parent.GetComponent<enemyPathfinding>();
        }
        //else if (transform.parent.tag == "guard")
        //{
        //    guard = transform.parent.GetComponent<guardDog>();
        //}
    }
    void Update()
    {
        if (transform.parent.tag == "enemy")
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
                script.stateManager(2);
                Debug.Log(hit);

            }
            //print (hit.collider);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            if (transform.parent.tag == "patrolDog")
            {
                //detectionTimer = 60.0f;
                if (Physics.Linecast(transform.parent.position, other.transform.position, out hit))
                {
                    if (hit.collider == other)
                    {
                        if (script.States != enumStates.chase)
                        {
                            script.areaCounter = 0;

                            script.stateManager(3);
                        }
                    }
                }
            }
        }
    }
}



