using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum enumStates
{
    patrol = 0,
    idle = 1,
    chase = 2,
    alert = 3,
    idleSuspicious = 4,
    distracted = 5,
    detectSound = 6,
    eatbone = 7
}

public class enemyPathfinding : MonoBehaviour {

	public Transform target1;
	public Transform target2;
	public Transform target3;
	public Transform currentTarget;
	public Transform lastTarget;
    public int lastState;

    
    public enumStates States;
    GameObject vision;
    GameObject smell;
    GameObject bone;
    GameObject player; 

	List<Transform> targets = new List<Transform>();
	public bool loopWaypoints;
    public bool patrol = true;
    public bool lookForSound = false;
    public bool chasePlayer = false;
    public bool distracted = false;
    public bool eatbone = false;

    public float turnSpeed = 2.0f;
    public float escapeTimer = 0;
	float waypointOffsetMin = -1.0f;
	float waypointOffsetMax = 1.0f;

	public float speed = 10;
	Vector3[] path = new Vector3[0];
	int targetIndex;
	int targetCounter = 0;
	bool hasWaypointsLeft;
	Vector3 currentWaypoint;
	public int timer = 400;

	float vectorTransformPositionx = 0;
	float vectorTransformPositionz = 0;

	float vectorCurrentTargetx = 0;
	float vectorCurrentTargetz = 0;

	float vectorx;
	float vectorz;

    float maxspeed = 20;
	void Start()
	{

		targets.Add (target1);
		targets.Add (target2);
		targets.Add (target3);

        player = GameObject.FindGameObjectWithTag("Player");

		currentTarget = targets[0];
		lastTarget = currentTarget;

		PathRequestManager.requestPath (transform.position, currentTarget.position, onPathFound);        
	}

	void Update()
    {
        
        //------------------//
        //Code of the states//
        //------------------//
        switch(States)
        {
            case enumStates.patrol:
                {
                    //Patrol, moves from one waypoint to the next waiting for a second before advancing forward
                    if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
                    {
                        if (timer <= 0 && (!distracted))
                        {
                            lastTarget = currentTarget;
                            currentTarget = targets[targetCounter];

                            PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);

                            timer += 60;
                            targetCounter++;
                            if (targetCounter > 2)
                            {
                                targetCounter = 0;
                            }
                        }
                        timer--;
                    }
                    
                }
                
                    break;
                
            case enumStates.idle:
                // idle, look around, without moving towards any waypoints
                    break;
            case enumStates.chase:
                    {
                        // chase the player constantly searching for a waypoint at the player position
                        currentTarget = player.transform;
                        PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);

                        // Escape from chase

                        Vector3 Player_direction = (player.transform.localPosition) - (this.transform.localPosition);
                        if (((Player_direction.x >= 10) || Player_direction.x <= -10 || Player_direction.z >= 10 || Player_direction.z <= -10))
                        {
                            escapeTimer += Time.deltaTime;
                            if (escapeTimer > 5)
                            {
                                if(lastTarget != null)
                                currentTarget = lastTarget;
                                PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
                                stateManager(0);
                            }
                        }
                    }
                    break;
            case enumStates.alert:

                    break;
            case enumStates.idleSuspicious:
                    break;
            case enumStates.distracted:
                    {
                        // Move towards distraction
                        PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
                        Vector3 bonedir = (currentTarget.transform.localPosition) - (this.transform.localPosition);
                        if (bonedir.x <= 4 && bonedir.x >= -4 && bonedir.z <= 4 && bonedir.z >= -4)
                        {
                            timer = 400;
                            stateManager(7);
                            distracted = false;
                            eatbone = true;
                        }
                    }

                    break;
            case enumStates.detectSound:
                    {
                        // when sound is heard, move towards the source
                        GameObject brokenObject = GameObject.FindGameObjectWithTag("Broken Object");
                        bone = GameObject.FindGameObjectWithTag("Bone");
                        if(brokenObject)
                        {
                            currentTarget = brokenObject.transform;
                            PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
                            Vector3 objectdir = (brokenObject.transform.localPosition) - (this.transform.localPosition);
                            if (objectdir.x <= 2 && objectdir.x >= -2 && objectdir.z <= 2 && objectdir.z >= -2 || !brokenObject) 
                            {
                                if (brokenObject)
                                    Destroy(brokenObject);
                                stateManager(0);
                                currentTarget = lastTarget;
                                PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
                    
                            }
                        }
                        else if(bone)
                        {
                            stateManager(5);
                            currentTarget = bone.transform;
                            GameObject temp = GameObject.FindGameObjectWithTag("Vision");
                            vision = temp.gameObject;
                            vision.SetActive(false);

                            temp = GameObject.FindGameObjectWithTag("Smell");
                            smell = temp.gameObject;
                            smell.SetActive(false);
                        }
                            //------------------//
                            //Fallback to patrol//
                            //------------------//
                        else
                        {
                            stateManager(0);
                        }
                    }

                    break;
            case enumStates.eatbone:
                    {
                        // holds the enemy still for long enough for the distraction to pass
                        if (!bone)
                        {
                            vision.SetActive(true);
                            smell.SetActive(true);                            
                            stateManager(0);
                            currentTarget = lastTarget;
                            if(currentTarget != null)
                            PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
                        }
                            
                        if (timer <= 0)
                        {
                            distracted = false;
                            vision.SetActive(true);
                            smell.SetActive(true);
                            eatbone = false;
                            Destroy(bone);
                            stateManager(0);
                            //PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
                           
                        }
                        timer--;
                    }

                    break;
            default:
                    break;
        }
        if (speed > 4)
        {


            Vector3 velocity = transform.GetComponent<Rigidbody>().velocity;
            if (velocity.x > maxspeed)
            {
                float temp = velocity.x - maxspeed;
                this.GetComponent<Rigidbody>().AddForce(new Vector3(-temp, 0, 0));
            }
            else if (velocity.y > maxspeed)
            {
                float temp = velocity.y - maxspeed;
                this.GetComponent<Rigidbody>().AddForce(new Vector3(0, -temp, 0));
            }
            else if (velocity.z > maxspeed)
            {
                float temp = velocity.z - maxspeed;
                this.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -temp));
            }
        }

        if(currentTarget != null)
        {
            vectorTransformPositionx = transform.position.x;
            vectorTransformPositionz = transform.position.z;

            vectorCurrentTargetx = currentTarget.position.x;
            vectorCurrentTargetz = currentTarget.position.z;

            if (vectorTransformPositionx < 0)
            {
                vectorTransformPositionx *= -1;
            }  

            if (vectorTransformPositionz < 0)
            {
                vectorTransformPositionz *= -1;
            }

            if (vectorCurrentTargetx < 0)
            {
                vectorCurrentTargetx *= -1;
            }

            if (vectorCurrentTargetz < 0)
            {
                vectorCurrentTargetz *= -1;
            }

            vectorx = (vectorTransformPositionx - vectorCurrentTargetx);
            vectorz = (vectorTransformPositionz - vectorCurrentTargetz);
        }
    }
    //-------------//
    //State Manager//
    //-------------//
    void stateManager(int value)
    {
        //if (value == 5)
        //    lastState = (int)States;
        States = (enumStates)value;

    }

	public void onPathFound(Vector3[] newPath, bool _pathSuccessful)
	{
		if (_pathSuccessful) 
		{
			path = newPath;
			StopCoroutine("followPath");
			StartCoroutine("followPath");
		}

	}

	IEnumerator followPath()
	{
		currentWaypoint = path [0];

		while (true) 
		{
			if(transform.position == currentWaypoint)
			{
				targetIndex ++;
				if(targetIndex >= path.Length)
				{
					targetIndex = 0;
					path = new Vector3[0];
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}
            if (currentTarget != null)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentTarget.position - transform.position), turnSpeed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            }
			yield return null;
		}
	}

	public void OnDrawGizmos()
	{
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i++) 
			{
				Gizmos.color = Color.black;
				Gizmos.DrawWireCube (path [i], Vector3.one);

				if (i == targetIndex) 
				{
					Gizmos.DrawLine (transform.position, path [i]);
				} 
				else 
				{
					Gizmos.DrawLine (path [i - 1], path [i]);
				}
			}
		} 
	
	}
}
//if (eatbone)
//{
//    currentTarget = lastTarget;
//    if (timer <= 0)
//    {
//        distracted = false;
//        vision.SetActive(true);
//        smell.SetActive(true);
//        eatbone = false;

//        stateManager(lastState);
//        currentTarget = lastTarget;
//        PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
//        Destroy(bone);
//    }
//    timer--;
//}
//if (!eatbone)
//{




//    if (States == enumStates.patrol)
//    {
//        Debug.Log("X: " + vectorx + " Z: " + vectorz);
//        if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
//        {
//            Debug.Log("Yohoo");
//            if (timer <= 0 && (!distracted))
//            {
//                lastTarget = currentTarget;
//                currentTarget = targets[targetCounter];

//                PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);

//                timer += 60;
//                targetCounter++;
//                if (targetCounter > 2)
//                {
//                    targetCounter = 0;
//                }
//            }
//            timer--;
//        }
//    }



//    if(States == enumStates.detectSound)
//    {
//        // Move to the broken object
//        GameObject brokenObject = GameObject.FindGameObjectWithTag("Broken Object");
//        currentTarget = brokenObject.transform;
//        PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
//        Vector3 dir = (brokenObject.transform.localPosition) - (this.transform.localPosition);
//        if (dir.x <= 2 && dir.x >= -2 && dir.z <= 2 && dir.z >= -2) 
//        {
//            stateManager(0);
//            currentTarget = lastTarget;
//            PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);

//        }

//    }
//    if (States == enumStates.distracted)
//    {
//        {
//            PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
//            Vector3 dir = (currentTarget.transform.localPosition) - (this.transform.localPosition);
//            if (dir.x <= 4 && dir.x >= -4 && dir.z <= 4 && dir.z >= -4)
//            {
//                Debug.Log("It's a bone!");
//                timer = 400;
//                distracted = false;
//                eatbone = true;


//            }
//        }

//    }
//    if (States == enumStates.chase)
//    {
//        // Move Enemy
//        Debug.Log(player.transform.localPosition);
//        currentTarget = player.transform;
//        PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);

//        // Escape from chase

//        Vector3 Player_direction = (player.transform.localPosition) - (this.transform.localPosition);
//        if (((Player_direction.x >= 10) || Player_direction.x <= -10 || Player_direction.z >= 10 || Player_direction.z <= -10))
//        {
//            escapeTimer += Time.deltaTime;
//            if (escapeTimer > 5)
//            {
//                currentTarget = lastTarget;
//                PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
//                stateManager(0);
//                //statePatrol();
//            }

//        }

//    }
//    if(States == enumStates.alert)
//    {

//  }
//}
//if (Input.GetKeyDown(KeyCode.T))
//{
//    lastState = (int)States;
//    stateManager(5);
//    bone = GameObject.FindGameObjectWithTag("Bone");
//    currentTarget = bone.transform;
//    GameObject temp = GameObject.FindGameObjectWithTag("Vision");
//    vision = temp.gameObject;
//    vision.SetActive(false);

//    temp = GameObject.FindGameObjectWithTag("Smell");
//    smell = temp.gameObject;
//    smell.SetActive(false);
//}
//void statePatrol()
//{
//    patrol = true;
//    lookForSound = false;
//    chasePlayer = false;
//}
//void stateLookForSound()
//{
//    patrol = false;
//    lookForSound = true;
//    chasePlayer = false;

//}
//void stateChasePlayer()
//{
//    escapeTimer = 0;
//    patrol = false;
//    lookForSound = false;
//    chasePlayer = true;

//}
//void stateDistracted()
//{
//    bone = GameObject.FindGameObjectWithTag("Bone");
//    currentTarget = bone.transform;
//    GameObject temp = GameObject.FindGameObjectWithTag("Vision");
//    vision = temp.gameObject;
//    vision.SetActive(false);

//    temp = GameObject.FindGameObjectWithTag("Smell");
//    smell = temp.gameObject;
//    smell.SetActive(false);
//    distracted = true;

//}