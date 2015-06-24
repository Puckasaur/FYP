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
    eatBone = 7
}

public class enemyPathfinding : MonoBehaviour 
{

    soundSphere sphereScript;


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
    GameObject newSphere;
    public GameObject sphere;

	List<Transform> targets = new List<Transform>();
    public List<Transform> alertArea = new List<Transform>();

	bool hasWaypointsLeft;
    public bool eatBone = false;
    public bool distracted = false;

    public float turnSpeed = 2.0f;
    public float escapeTimer = 0;
    public float alertTimer = 0;
    public float eatTimer = 0;
	public float speed = 10;

    float maxSpeed = 20;
    float maxScale = 60;
	float waypointOffsetMin = -1.0f;
	float waypointOffsetMax = 1.0f;
	float vectorTransformPositionx = 0;
	float vectorTransformPositionz = 0;
	float vectorCurrentTargetx = 0;
	float vectorCurrentTargetz = 0;
	float vectorx;
    float vectorz;


    //Idle Suspicious values
    public bool idleSuscpicious = false;
    public float firstDirection; //= 33;
    public float secondDirection; // = 66;
    public float thirdDirection; // = 78;
    List<float> directionDegrees = new List<float>();
    GameObject enemyObject;

    bool rotating = false;
    float rotationStep = 10.0f;
    public float rotationDegrees = 90;
    float currentAngle = 0;
    float targetAngle = 0;
    float angleOffsetMax = 1.0f;
    float angleOffsetMin = -1.0f;
    bool rotationInProgress = false;
    public bool rotationCompleted = false;
    float turnTimer = 200.0f;
    float currentTargetDirection;
    //
	public int timer = 60;
    public int idleTimer = 100;    
    public int barkTimer = 120;
    public int lastState;
    public int defaultEatTimer = 120;
    public int defaultIdleTimer = 100;
    public int defaultBarkTimer = 120;
    public int defaultTimer = 60;
    public int defaultAlertTimer = 400;
	int targetIndex;
	int targetCounter = 0;
    int areaCounter = 0;

	Vector3[] path = new Vector3[0];
	Vector3 currentWaypoint;

    //values if enemy doesn't receive a new waypoint to prevent them from being stuck
    Vector3 worldPositionNow;
    Vector3 worldPositionPast;
    int checkIfStuck = 100;
    bool isStuck = false;

	void Start()
	{        


        player = GameObject.FindGameObjectWithTag("player");
        setDirectionsForIdle();
        setTargetWaypoints();
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

            case enumStates.patrol:
                {
                    //-----------------------------------------------------------------------------------------//
                    //patrol, moves from one waypoint to the next waiting for a second before advancing forward//
                    //-----------------------------------------------------------------------------------------//
                    if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
                    {
                        //if (timer <= 0 && (!distracted))
                        //{
                        //    lastTarget = currentTarget;
                        //    currentTarget = targets[targetCounter];

                        //    targetCounter++;
                        //    if (targetCounter > 2)
                        //    {
                        //        targetCounter = 0;
                        //    }
                        //}
                        stateManager(1);

                    }
                    
                }
                
                    break;


            case enumStates.idle:
                    {
                        //--------------------------------------------------------//
                        // idle, look around, without moving towards any waypoints//
                        //--------------------------------------------------------//

                            if (idleTimer <= 0)
                            {
                                lastTarget = currentTarget;
                                currentTarget = targets[targetCounter];

                                pathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);

                                idleTimer += 100;
                                targetCounter++;
                                if (targetCounter > 2)
                                {
                                    targetCounter = 0;
                                }
                                stateManager(0);
                            }
                            idleTimer--;
                        break;
                    }
            case enumStates.chase:
                    {
                        //----------------------------------------------------------------------------//
                        // chase the Player constantly searching for a waypoint at the Player position//

                        //----------------------------------------------------------------------------//
                        
                        currentTarget = player.transform;
                        //pathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);

                        //------------------//
                        //Bark While chasing//
                        //------------------//
                        if (barkTimer < 0)
                        {
                            newSphere = (GameObject)Instantiate(sphere, this.transform.localPosition, Quaternion.identity);
                            newSphere.transform.parent = transform;
                            barkTimer = defaultBarkTimer;
                            if (newSphere)
                            {
                                sphereScript = newSphere.GetComponent<soundSphere>();
                                sphereScript.setMaxDiameter(maxScale);
                            }
                            
                        }
                        barkTimer--;
                        //-----------------//
                        //Escape from chase//
                        //-----------------//
                        Vector3 playerDirection = (player.transform.localPosition) - (this.transform.localPosition);
                        if (((playerDirection.x >= 10) || playerDirection.x <= -10 || playerDirection.z >= 10 || playerDirection.z <= -10))
                        {

                            escapeTimer += Time.deltaTime;
                            if (escapeTimer >= 5)
                            {
                                //escapeTimer = 0;
                                //if(lastTarget != null)
                                escapeTimer = 0;
                                    currentTarget = alertArea[areaCounter];
                                    areaCounter++;
                                    stateManager(3);


                            }
                        }

                    }
                    break;
            case enumStates.alert:
                //------------------------------------------------------//
                //Look around a room by moving from waypoint to waypoint//
                //------------------------------------------------------//

                    if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
                    {
                        if (timer <= 0 && (!distracted))
                        {
                            lastTarget = currentTarget;
                            currentTarget = alertArea[areaCounter];
                            areaCounter++;
                            if (areaCounter > 2)
                            {
                                areaCounter = 0;
                            }
                        }

                        
                    }
                if(alertTimer <= 0)
                {
                    alertTimer += defaultAlertTimer;
                    currentTarget = targets[0];
                    lastTarget = currentTarget;
                    targetCounter++;
                    if(targetCounter > 2)
                    {
                        targetCounter = 0;
                    }
                    stateManager(4);
                }
                    alertTimer--;

                    break;
            case enumStates.idleSuspicious:
                    {
                        //-----------------------------------------------//
                        //Stand on the spot and look at preset directions//
                        //-----------------------------------------------//
						

						if(idleTimer > 0)
						{
						rotateEnemy(currentTargetDirection, rotationStep);

							if (rotationCompleted)
							{
							print("rotation completed");
							directionDegrees.Add(directionDegrees[0]);
							directionDegrees.Remove(directionDegrees[0]);
							currentTargetDirection = directionDegrees[0];
							rotationCompleted = false;
					
							} 

						}
                      
						else if (idleTimer <= 0)
						{
							lastTarget = currentTarget;
							currentTarget = targets[targetCounter];
				
							pathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
				
							idleTimer += 100;
							targetCounter++;
							if (targetCounter > 2)
							{
								targetCounter = 0;
							}
							stateManager(0);
						}

						idleTimer--;						
                        break;
                    }
            case enumStates.distracted:

                    {
                        //-------------------------//
                        // Move towards distraction//
                        //-------------------------//

                        distracted = true;
                        Vector3 bonedir = (currentTarget.transform.localPosition) - (this.transform.localPosition);
                        if (bonedir.x <= 4 && bonedir.x >= -4 && bonedir.z <= 4 && bonedir.z >= -4)
                        {
                            stateManager(7);
                            distracted = false;
                            if (!eatBone)
                            {
                                eatTimer = defaultEatTimer;
                            }

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
                        bone = GameObject.FindGameObjectWithTag("bone");
                        if(brokenObject)
                        {
                            Vector3 objectdir = (brokenObject.transform.localPosition) - (this.transform.localPosition);
                            if (objectdir.x <= 2 && objectdir.x >= -2 && objectdir.z <= 2 && objectdir.z >= -2 || !brokenObject) 
                            {
                                stateManager(0);
                                currentTarget = lastTarget;


                            }
                            else
                            {
                                currentTarget = brokenObject.transform;


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

                    }

                    break;
            case enumStates.eatBone:
                    {
                        //------------------------------------------------------------------//
                        // holds the enemy still for long enough for the distraction to pass//
                        //------------------------------------------------------------------//

                        eatBone = true;
                        if (!bone)
                        {
                            vision.SetActive(true);
                            smell.SetActive(true);
                            alertTimer += 500;
                            stateManager(3);
                            currentTarget = alertArea[areaCounter];
                        }
                            
                        if (eatTimer <= 0)

                        {
                            eatTimer = defaultEatTimer;// 120;
                            distracted = false;
                            vision.SetActive(true);
                            smell.SetActive(true);
                            eatBone = false;

                            currentTarget = alertArea[areaCounter];
                            Destroy(bone);
                            alertTimer += 500;
                            stateManager(3);
                           
                        }
                        eatTimer--;

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

        if(timer <= 0)
        {
            timer+=60;
            pathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
        }
        timer--;
        //-------------//
        //End of Update//
        //-------------//
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

    void setDirectionsForIdle()
    {

        directionDegrees.Add(firstDirection);
        directionDegrees.Add(secondDirection);
        directionDegrees.Add(thirdDirection);

    }

    void setTargetWaypoints()
    {

        targets.Add(target1);
        targets.Add(target2);
        targets.Add(target3);
    }

    void rotateEnemy(float targetDegrees, float rotationStep)
    {
        //print ("turnTimer  >> " + turnTimer);
        //rotationInProgress = true;
        //		while (rotationInProgress == true) 
        float rotationDifference = 0;


        if (turnTimer <= 0)
        {
            //print (turnTimer + "  << TurnTimer");
            if (rotationInProgress == false)
            {
                currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
                targetAngle = targetDegrees;//currentAngle + targetDegrees;
                rotationInProgress = true;
                print("current angle:  " + currentAngle + "target angle:  " + targetAngle);
            }

            else if (rotationInProgress)
            {
                if (turnTimer == 0 && rotationDifference >= 0)
                {
                    if (targetAngle <= 180 && targetAngle >= 0) //decide which side the target is. 0-180 left, 0 - (-180)
                    {
                        if (targetAngle <= 90)// decide which sector the target is. 4 different sectors 0-90, 90-180, 0-(-90), (-90)- (-180)
                        {

                            if (currentAngle <= targetAngle && turnTimer == 0)
                            {
                                print("entered the rotation loop");
                                transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * -1);
                                currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
                                rotationDifference = targetAngle - currentAngle;
                                print(rotationDifference + " << rotation    " + targetAngle + " <<  target Angle    " + currentAngle + " << current Angle");
                                if (rotationDifference < 0)
                                {
                                    rotationDifference = rotationDifference * -1;
                                }

                                print(currentAngle + "  << current Angle  " + angleOffsetMin + "  <<angleOffsetMin    " + angleOffsetMax + "  <<angleOffsetMax   " + rotationDifference + "  << rotationDifference");
                                if (currentAngle == targetAngle || angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
                                {
                                    rotationCompleted = true;
                                    rotationInProgress = false;
                                    turnTimer += 200f * Time.deltaTime;
                                    print(rotationCompleted + " rotationCompleted" + rotationInProgress + "  rotation in progress  " + turnTimer + " <<  turnTimer");
                                }
                            }
                            else if (currentAngle > targetAngle && turnTimer == 0)
                            {

                                print("entered the rotation loop 2" + "   targetAnle  >>   " + targetAngle);
                                transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * 1);
                                currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
                                rotationDifference = targetAngle - currentAngle;
                                if (currentAngle == targetAngle && angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
                                {
                                    rotationCompleted = true;
                                    rotationInProgress = false;
                                    turnTimer += 200f * Time.deltaTime;
                                    print(rotationCompleted + " rotationCompleted" + rotationInProgress + "  rotation in progress  " + turnTimer + " <<  turnTimer");
                                }

                            }

                        }
                        else if (targetAngle > 90 && turnTimer == 0)// decide which sector the target is
                        {
                            if (currentAngle <= targetAngle)
                            {
                                print("entered the rotation loop");
                                transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * -1);
                                currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
                                rotationDifference = targetAngle - currentAngle;
                                print(rotationDifference + " << rotation difference   " + targetAngle + " <<  target Angle    " + currentAngle + " << current Angle");
                                if (rotationDifference < 0)
                                {
                                    rotationDifference = rotationDifference * -1;
                                }


                                if (currentAngle == targetAngle || angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
                                {
                                    rotationCompleted = true;
                                    rotationInProgress = false;
                                    turnTimer += 200f * Time.deltaTime;
                                    print(rotationCompleted + " rotationCompleted" + rotationInProgress + "  rotation in progress  " + turnTimer + " <<  turnTimer");
                                }
                            }
                            else if (currentAngle > targetAngle && turnTimer == 0)
                            {
                                print("entered the rotation loop 2");
                                transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * -1);
                                currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
                                rotationDifference = targetAngle - currentAngle;
                                if (currentAngle == targetAngle && angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
                                {
                                    rotationCompleted = true;
                                    rotationInProgress = false;
                                    turnTimer += 200f * Time.deltaTime;
                                    print(rotationCompleted + " rotationCompleted" + rotationInProgress + "  rotation in progress  " + turnTimer + " <<  turnTimer");
                                }
                            }
                        }
                    }
                    else if (targetAngle < 0 && targetAngle > -180)  //decide which side the target is
                    {
                        if (targetAngle >= -90)// decide which sector the target is. 4 different sectors 0-90, 90-180, 0-(-90), (-90)- (-180)
                        {
                            if (currentAngle >= targetAngle && turnTimer == 0)
                            {
                                print("entered the rotation loop -90 - 0");
                                transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * 1);
                                currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
                                rotationDifference = targetAngle - currentAngle;
                                print(rotationDifference + " << rotation    " + targetAngle + " <<  target Angle    " + currentAngle + " << current Angle");
                                if (rotationDifference < 0)
                                {
                                    rotationDifference = rotationDifference * -1;
                                }

                                print(currentAngle + "  << current Angle  " + angleOffsetMin + "  <<angleOffsetMin    " + angleOffsetMax + "  <<angleOffsetMax   " + rotationDifference + "  << rotationDifference");
                                if (currentAngle == targetAngle || angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
                                {
                                    rotationCompleted = true;
                                    rotationInProgress = false;
                                    turnTimer += 200f * Time.deltaTime;
                                    print(rotationCompleted + " rotationCompleted" + rotationInProgress + "  rotation in progress  " + turnTimer + " <<  turnTimer");
                                }
                            }
                            else if (currentAngle < targetAngle && turnTimer == 0)
                            {

                                print("entered the rotation loop 2" + "   targetAnle  >>   " + targetAngle);
                                transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * -1);
                                currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
                                rotationDifference = targetAngle - currentAngle;
                                if (currentAngle == targetAngle && angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
                                {
                                    rotationCompleted = true;
                                    rotationInProgress = false;
                                    turnTimer += 200f * Time.deltaTime;
                                    print(rotationCompleted + " rotationCompleted" + rotationInProgress + "  rotation in progress  " + turnTimer + " <<  turnTimer");
                                }

                            }
                        }
                        if (targetAngle < -90)// decide which sector the target is. 4 different sectors 0-90, 90-180, 0-(-90), (-90)- (-180)
                        {
                            if (currentAngle >= targetAngle && turnTimer == 0)
                            {
                                print("entered the rotation loop -90 - 0");
                                transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * 1);
                                currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
                                rotationDifference = targetAngle - currentAngle;
                                print(rotationDifference + " << rotation    " + targetAngle + " <<  target Angle    " + currentAngle + " << current Angle");
                                if (rotationDifference < 0)
                                {
                                    rotationDifference = rotationDifference * -1;
                                }

                                print(currentAngle + "  << current Angle  " + angleOffsetMin + "  <<angleOffsetMin    " + angleOffsetMax + "  <<angleOffsetMax   " + rotationDifference + "  << rotationDifference");
                                if (currentAngle == targetAngle || angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
                                {
                                    rotationCompleted = true;
                                    rotationInProgress = false;
                                    turnTimer += 200f * Time.deltaTime;
                                    print(rotationCompleted + " rotationCompleted" + rotationInProgress + "  rotation in progress  " + turnTimer + " <<  turnTimer");
                                }
                            }
                            else if (currentAngle < targetAngle && turnTimer == 0)
                            {

                                print("entered the rotation loop 2" + "   targetAnle  >>   " + targetAngle);
                                transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * -1);
                                currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
                                rotationDifference = targetAngle - currentAngle;
                                if (currentAngle == targetAngle && angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
                                {
                                    rotationCompleted = true;
                                    rotationInProgress = false;
                                    turnTimer += 200f * Time.deltaTime;
                                    print(rotationCompleted + " rotationCompleted" + rotationInProgress + "  rotation in progress  " + turnTimer + " <<  turnTimer");
                                }

                            }
                        }
                    }
                }

            }


        }
        else
        {
            turnTimer--;
            if (turnTimer < 0)
            {
                turnTimer = 0;
            }


            //print ("turn timer  " + turnTimer);
        }

    }
    //------------------------------------------------------------//
    //Sets an area from a room the enemy is in for the alert-state//
    //------------------------------------------------------------//

    public void setAlertArea(GameObject area)
    {
        Component[] transforms;
        alertArea.Clear();
        transforms = area.GetComponentsInChildren<Transform>();

        foreach(Transform alert in transforms)
        {
            if(alert.tag == "Waypoint")
            {
                alertArea.Add(alert);
            }

        }
    }
}

//if (eatBone)
//{
//    currentTarget = lastTarget;
//    if (Timer <= 0)
//    {

//        distracted = false;
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





//    if (States == enumStates.patrol)
//    {
//        Debug.Log("X: " + vectorx + " Z: " + vectorz);
//        if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
//        {
//            Debug.Log("Yohoo");

//            if (Timer <= 0 && (!distracted))
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

//    if (States == enumStates.distracted)
//    {
//        {
//            PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
//            Vector3 dir = (currentTarget.transform.localPosition) - (this.transform.localPosition);
//            if (dir.x <= 4 && dir.x >= -4 && dir.z <= 4 && dir.z >= -4)
//            {
//                Debug.Log("It's a Bone!");
//                Timer = 400;

//                distracted = false;

//                eatBone = true;


//            }
//        }

//    }

//    if (States == enumStates.chase)

//    {
//        // Move Enemy
//        Debug.Log(Player.transform.localPosition);
//        currentTarget = Player.transform;
//        PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);


//        // Escape from chase


//        Vector3 playerDirection = (Player.transform.localPosition) - (this.transform.localPosition);
//        if (((playerDirection.x >= 10) || playerDirection.x <= -10 || playerDirection.z >= 10 || playerDirection.z <= -10))
//        {
//            escapeTimer += Time.deltaTime;
//            if (escapeTimer > 5)
//            {
//                currentTarget = lastTarget;
//                PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
//                stateManager(0);

//                //statepatrol();

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
//    Bone = GameObject.FindGameObjectWithTag("Bone");
//    currentTarget = Bone.transform;
//    GameObject temp = GameObject.FindGameObjectWithTag("Vision");
//    vision = temp.gameObject;
//    vision.SetActive(false);

//    temp = GameObject.FindGameObjectWithTag("Smell");
//    smell = temp.gameObject;
//    smell.SetActive(false);
//}

//void statepatrol()
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

//void statechasePlayer()
//{
//    escapeTimer = 0;
//    patrol = false;

//    lookForSound = false;
//    chasePlayer = true;

//}

//void statedistracted()

//{
//    Bone = GameObject.FindGameObjectWithTag("Bone");
//    currentTarget = Bone.transform;
//    GameObject temp = GameObject.FindGameObjectWithTag("Vision");
//    vision = temp.gameObject;
//    vision.SetActive(false);

//    temp = GameObject.FindGameObjectWithTag("Smell");
//    smell = temp.gameObject;
//    smell.SetActive(false);

//    distracted = true;


//}