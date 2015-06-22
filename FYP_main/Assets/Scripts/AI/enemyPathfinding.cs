using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum enumStates
{
    Patrol = 0,
    Idle = 1,
    Chase = 2,
    Alert = 3,
    idleSuspicious = 4,
    Distracted = 5,
    detectSound = 6,
    eatBone = 7
}

public class enemyPathfinding : MonoBehaviour 
{

	public Transform target1;
	public Transform target2;
	public Transform target3;
	public Transform currentTarget;
	public Transform lastTarget;


    
    public enumStates States;
    GameObject vision;
    GameObject smell;
    GameObject bone;
    GameObject player; 

	List<Transform> targets = new List<Transform>();
    public List<Transform> alertArea = new List<Transform>();
	bool hasWaypointsLeft;
    public bool eatBone = false;
    public bool distracted = false;
    public float turnSpeed = 2.0f;
    public float escapeTimer = 0;
	public float speed = 10;
    float maxSpeed = 20;
	float waypointOffsetMin = -1.0f;
	float waypointOffsetMax = 1.0f;
	float vectorTransformPositionx = 0;
	float vectorTransformPositionz = 0;
	float vectorCurrentTargetx = 0;
	float vectorCurrentTargetz = 0;
	float vectorx;
    float vectorz;

	public int timer = 400;    
    public int lastState;
	int targetIndex;
	int targetCounter = 0;
    int areaCounter = 0;

	Vector3[] Path = new Vector3[0];
	Vector3 currentWaypoint;



	void Start()
	{
        
		targets.Add (target1);
		targets.Add (target2);
		targets.Add (target3);

        player = GameObject.FindGameObjectWithTag("player");

		currentTarget = targets[0];
		lastTarget = currentTarget;

		pathRequestManager.requestPath (transform.position, currentTarget.position, onPathFound);        
	}

	void Update()
    {
        

        //------------------//
        //Code of the states//
        //------------------//
        switch(States)
        {
            case enumStates.Patrol:
                {
                    //-----------------------------------------------------------------------------------------//
                    //Patrol, moves from one waypoint to the next waiting for a second before advancing forward//
                    //-----------------------------------------------------------------------------------------//
                    if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
                    {
                        if (timer <= 0 && (!distracted))
                        {
                            lastTarget = currentTarget;
                            currentTarget = targets[targetCounter];

                            pathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);

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

            case enumStates.Idle:
                    {
                        //--------------------------------------------------------//
                        // Idle, look around, without moving towards any waypoints//
                        //--------------------------------------------------------//
                        break;
                    }
            case enumStates.Chase:
                    {
                        //----------------------------------------------------------------------------//
                        // Chase the Player constantly searching for a waypoint at the Player position//
                        //----------------------------------------------------------------------------//
                        
                        currentTarget = player.transform;
                        pathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);

                        //-----------------//
                        //Escape from Chase//
                        //-----------------//
                        Vector3 playerDirection = (player.transform.localPosition) - (this.transform.localPosition);
                        if (((playerDirection.x >= 10) || playerDirection.x <= -10 || playerDirection.z >= 10 || playerDirection.z <= -10))
                        {
                            escapeTimer += Time.deltaTime;
                            if (escapeTimer > 5)
                            {
                                //escapeTimer = 0;
                                //if(lastTarget != null)

                                    currentTarget = alertArea[areaCounter];
                                    areaCounter++;
                                    pathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
                                    stateManager(3);


                            }
                        }
                        //else
                        //{
                        //    escapeTimer = 0;
                        //}
                    }
                    break;
            case enumStates.Alert:
                //-------------------------------------------------------------//
                //Search look around a room by moving from waypoint to waypoint//
                //-------------------------------------------------------------//
                    if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
                    {
                        if (timer <= 0 && (!distracted))
                        {
                            lastTarget = currentTarget;
                            currentTarget = alertArea[areaCounter];

                            pathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);

                            timer += 60;
                            areaCounter++;
                            if (areaCounter > 2)
                            {
                                areaCounter = 0;
                            }
                        }
                        timer--;
                    }
                    break;
            case enumStates.idleSuspicious:
                    {
                        //-----------------------------------------------//
                        //Stand on the spot and look at preset directions//
                        //-----------------------------------------------//
                        break;
                    }
            case enumStates.Distracted:
                    {
                        //-------------------------//
                        // Move towards distraction//
                        //-------------------------//
                        pathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
                        Vector3 bonedir = (currentTarget.transform.localPosition) - (this.transform.localPosition);
                        if (bonedir.x <= 4 && bonedir.x >= -4 && bonedir.z <= 4 && bonedir.z >= -4)
                        {
                            timer = 60;
                            stateManager(7);
                            distracted = false;
                            eatBone = true;
                        }
                    }

                    break;
            case enumStates.detectSound:
                    {
                        //---------------------------------------------//
                        // when sound is heard, move towards the source//
                        //---------------------------------------------//
                        GameObject brokenObject = GameObject.FindGameObjectWithTag("brokenObject");
                        bone = GameObject.FindGameObjectWithTag("Bone");
                        if(brokenObject)
                        {
                            Vector3 objectdir = (brokenObject.transform.localPosition) - (this.transform.localPosition);
                            if (objectdir.x <= 2 && objectdir.x >= -2 && objectdir.z <= 2 && objectdir.z >= -2 || !brokenObject) 
                            {
                                stateManager(0);
                                currentTarget = lastTarget;
                                pathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
                            }
                            else
                            {
                                currentTarget = brokenObject.transform;
                                pathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
                            }
                        }
                        else if(bone)
                        {
                            stateManager(5);
                            currentTarget = bone.transform;
                            GameObject temp = GameObject.FindGameObjectWithTag("vision");
                            vision = temp.gameObject;
                            vision.SetActive(false);

                            temp = GameObject.FindGameObjectWithTag("smell");
                            smell = temp.gameObject;
                            smell.SetActive(false);
                        }
                            //------------------//
                            //Fallback to Patrol//
                            //------------------//
                        else
                        {
                            stateManager(0);
                        }
                    }

                    break;
            case enumStates.eatBone:
                    {
                        //------------------------------------------------------------------//
                        // holds the enemy still for long enough for the distraction to pass//
                        //------------------------------------------------------------------//
                        if (!bone)
                        {
                            vision.SetActive(true);
                            smell.SetActive(true);                            
                            stateManager(0);
                            currentTarget = lastTarget;
                            if(currentTarget != null)
                            pathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
                        }
                            
                        if (timer <= 0)
                        {
                            distracted = false;
                            vision.SetActive(true);
                            smell.SetActive(true);
                            eatBone = false;
                            Destroy(bone);
                            stateManager(0);
                            pathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
                           
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
            if (velocity.x > maxSpeed)
            {
                float temp = velocity.x - maxSpeed;
                this.GetComponent<Rigidbody>().AddForce(new Vector3(-temp, 0, 0));
            }
            else if (velocity.y > maxSpeed)
            {
                float temp = velocity.y - maxSpeed;
                this.GetComponent<Rigidbody>().AddForce(new Vector3(0, -temp, 0));
            }
            else if (velocity.z > maxSpeed)
            {
                float temp = velocity.z - maxSpeed;
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
    public void stateManager(int value)
    {
        States = (enumStates)value;

    }

	public void onPathFound(Vector3[] newPath, bool _pathSuccessful)
	{
		if (_pathSuccessful) 
		{
			Path = newPath;
			StopCoroutine("followPath");
			StartCoroutine("followPath");
		}

	}

	IEnumerator followPath()
	{
		currentWaypoint = Path [0];

		while (true) 
		{
			if(transform.position == currentWaypoint)
			{
				targetIndex ++;
				if(targetIndex >= Path.Length)
				{
					targetIndex = 0;
					Path = new Vector3[0];
					yield break;
				}
				currentWaypoint = Path[targetIndex];
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
		if (Path != null) {
			for (int i = targetIndex; i < Path.Length; i++) 
			{
				Gizmos.color = Color.black;
				Gizmos.DrawWireCube (Path [i], Vector3.one);

				if (i == targetIndex) 
				{
					Gizmos.DrawLine (transform.position, Path [i]);
				} 
				else 
				{
					Gizmos.DrawLine (Path [i - 1], Path [i]);
				}
			}
		} 
	
	}

    public void setAlertArea(GameObject area)
    {
        Component[] transforms;
        alertArea.Clear();
        transforms = area.GetComponentsInChildren<Transform>();
        foreach(Transform Alert in transforms)
        {
            alertArea.Add(Alert);
        }
    }
}

//if (eatBone)
//{
//    currentTarget = lastTarget;
//    if (Timer <= 0)
//    {
//        Distracted = false;
//        vision.SetActive(true);
//        smell.SetActive(true);
//        eatBone = false;

//        stateManager(lastState);
//        currentTarget = lastTarget;
//        PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
//        Destroy(Bone);
//    }
//    Timer--;
//}
//if (!eatBone)
//{




//    if (States == enumStates.Patrol)
//    {
//        Debug.Log("X: " + vectorx + " Z: " + vectorz);
//        if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
//        {
//            Debug.Log("Yohoo");
//            if (Timer <= 0 && (!Distracted))
//            {
//                lastTarget = currentTarget;
//                currentTarget = Targets[targetCounter];

//                PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);

//                Timer += 60;
//                targetCounter++;
//                if (targetCounter > 2)
//                {
//                    targetCounter = 0;
//                }
//            }
//            Timer--;
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
//    if (States == enumStates.Distracted)
//    {
//        {
//            PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
//            Vector3 dir = (currentTarget.transform.localPosition) - (this.transform.localPosition);
//            if (dir.x <= 4 && dir.x >= -4 && dir.z <= 4 && dir.z >= -4)
//            {
//                Debug.Log("It's a Bone!");
//                Timer = 400;
//                Distracted = false;
//                eatBone = true;


//            }
//        }

//    }
//    if (States == enumStates.Chase)
//    {
//        // Move Enemy
//        Debug.Log(Player.transform.localPosition);
//        currentTarget = Player.transform;
//        PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);

//        // Escape from Chase

//        Vector3 playerDirection = (Player.transform.localPosition) - (this.transform.localPosition);
//        if (((playerDirection.x >= 10) || playerDirection.x <= -10 || playerDirection.z >= 10 || playerDirection.z <= -10))
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
//    if(States == enumStates.Alert)
//    {

//  }
//}
//if (Input.GetKeyDown(KeyCode.T))
//{
//    lastState = (int)States;
//    stateManager(5);
//    Bone = GameObject.FindGameObjectWithTag("Bone");
//    currentTarget = Bone.transform;
//    GameObject temp = GameObject.FindGameObjectWithTag("Vision");
//    vision = temp.gameObject;
//    vision.SetActive(false);

//    temp = GameObject.FindGameObjectWithTag("Smell");
//    smell = temp.gameObject;
//    smell.SetActive(false);
//}
//void statePatrol()
//{
//    Patrol = true;
//    lookForSound = false;
//    chasePlayer = false;
//}
//void stateLookForSound()
//{
//    Patrol = false;
//    lookForSound = true;
//    chasePlayer = false;

//}
//void stateChasePlayer()
//{
//    escapeTimer = 0;
//    Patrol = false;
//    lookForSound = false;
//    chasePlayer = true;

//}
//void stateDistracted()
//{
//    Bone = GameObject.FindGameObjectWithTag("Bone");
//    currentTarget = Bone.transform;
//    GameObject temp = GameObject.FindGameObjectWithTag("Vision");
//    vision = temp.gameObject;
//    vision.SetActive(false);

//    temp = GameObject.FindGameObjectWithTag("Smell");
//    smell = temp.gameObject;
//    smell.SetActive(false);
//    Distracted = true;

//}