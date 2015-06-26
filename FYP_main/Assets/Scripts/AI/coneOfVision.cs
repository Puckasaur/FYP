using UnityEngine;
using System.Collections;

public class coneOfVision : MonoBehaviour 
{

    enemyPathfinding script;
    RaycastHit hit;
    public float width;
    public float height;
    public float range;
    public float detectionTimer = 60.0f;
    void Start()
    {
        script = this.transform.parent.GetComponent<enemyPathfinding>();
    }
    void Update()
    {
        if (transform.localScale.x < width)
        {
            transform.localScale += new Vector3(width, height, range);
        }
    }

    void OnTriggerStay(Collider other)
    {
		//-----------------------------------------------------------------------//
		//if player crosses the cone, informs the parent(Enemy) of visible player//
		//-----------------------------------------------------------------------//
		if (other.gameObject.tag == "player") {

			RaycastHit hit;
            Physics.Linecast(transform.parent.position, other.transform.position, out hit);
			if (hit.collider == other.GetComponent<Collider>()) 
            {
				if (detectionTimer <= 0) 
                {
                    detectionTimer = 60;
				    script.stateManager (2);
				}
				detectionTimer--;

				
			}     
		}
	}
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "player")
        {
            detectionTimer = 60.0f;
            if (Physics.Linecast(transform.parent.position, other.transform.position, out hit))
            {
                if (hit.collider == other)
                    if (script.States != enumStates.chase && script.States != enumStates.alert)
                    {
                        script.alertTimer = 500;
                        script.currentTarget = script.alertArea[script.areaCounter];
                        if (script.areaCounter > 2)
                        {
                            script.areaCounter = 0;
                        }
                        script.stateManager(3);
                    }
            }
        }
    }


}
