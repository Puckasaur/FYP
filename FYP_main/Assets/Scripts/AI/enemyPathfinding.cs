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
	ringOfSmell ringOfSmellScript;
	coneOfVision coneOfVisionScript;
	soundSphere sphereScript;
	RaycastHit hit;

    public Vector3 respawnPosition;
	public Transform target1;
	public Transform target2;
	public Transform target3;
	public Transform target4;

	public Transform currentTarget;
	public Transform lastTarget;
    public enumStates States;
	GameObject vision;
	GameObject smell;
	GameObject bone;
	GameObject player;
	GameObject newSphere;
	public GameObject sphere;
	public GameObject soundSource;
	GameObject brokenObject;

	public NavMeshAgent agent;
	public List<Transform> targets = new List<Transform>();
	public List<Transform> alertArea = new List<Transform>();
	
	public bool eatBone = false;
	public bool distracted = false;
	float maxScale = 20;
	float waypointOffsetMin = -2.05f;
	float waypointOffsetMax = 2.05f;
	float vectorTransformPositionx = 0;
	float vectorTransformPositionz = 0;
	float vectorCurrentTargetx = 0;
	float vectorCurrentTargetz = 0;
	float vectorx;
	float vectorz;
	
	//Idle Suspicious values

    float rotationDifference = 0;
	public bool idleSuscpicious = false;
	public float firstDirection; //= 33;
	public float secondDirection; // = 66;
	public float thirdDirection; // = 78;
	List<float> directionDegrees = new List<float>();
	GameObject enemyObject;
	
	bool rotating = false;
	float rotationStep = 65.0f;
	public float rotationDegrees = 90;
	public float currentAngle = 0;
	public float targetAngle = 0;
	public float angleOffsetMax = 10.0f;
	public float angleOffsetMin = -10.0f;
	bool rotationInProgress = false;
	public bool rotationCompleted = false;
	public float turnTimer = 100.0f;
	float currentTargetDirection;
	int turnCounter = 0;
    bool SeekForSmellSource = false;
    public bool agentStopped = false;

	//So many timers
	int tempcounters = 0;
	int timer;
	int idleTimer;    
	int barkTimer;
	float escapeTimer;
	public float alertTimer;
	float eatTimer;
    float failTimer;
	public int defaultEatTimer;
	public int defaultIdleTimer;
	public int defaultBarkTimer;
    public int defaultTimer;
	public int defaultAlertTimer;
	public int defaultEscapeTimer;
	int targetCounter = 0;
	public int areaCounter = 0;
	public float defaultTurnTimer;
	public int defaultDetectSoundTimer;
	int detectSoundTimer;
    public float turnTowardsSmellTimer;
    public float defaultTurnTowardsSmellTimer;
    public float patrolSpeed;
    public float chaseSpeed;
    public float chaseRange;
    public float agentNotMovingTimer;
    public float defaultAgentNotMovingTimer;
    
	Vector3[] path = new Vector3[0];
	Vector3 currentWaypoint;
	//Leap values
    public float leapRange;
    float leapTimer;
    public float defaultLeapTimer;
    Vector3 leapPosition;
    public float impulse;
    public float chargeRange;
    Vector3 enemyRotation;

    Collider playerCollider;

	//values if enemy doesn't receive a new waypoint to prevent them from being stuck	
	void Start()
	{
        
        leapTimer = defaultLeapTimer;

        respawnPosition = this.transform.position;
		player = GameObject.FindGameObjectWithTag("player");
        ringOfSmellScript = player.GetComponentInChildren<ringOfSmell>();
        coneOfVisionScript = GetComponentInChildren<coneOfVision>();
       
        //setPlayerOffSetTransform(playerOffSetTransform, player.transform);
        
		setDirectionsForIdle();
		setTargetWaypoints();
		currentTarget = targets[0];
		lastTarget = currentTarget;
		agent = GetComponent<NavMeshAgent>();
		agent.speed = patrolSpeed;
		agent.SetDestination(currentTarget.position);

		//Setting Timers
		timer = defaultTimer;
		eatTimer = defaultEatTimer;
		//idleTimer = defaultIdleTimer;
		barkTimer = defaultBarkTimer;
		alertTimer = defaultAlertTimer;
		escapeTimer = defaultEscapeTimer;
		turnTimer = defaultTurnTimer;
		detectSoundTimer = defaultDetectSoundTimer;
        turnTowardsSmellTimer = defaultTurnTowardsSmellTimer;
        agentNotMovingTimer = defaultAgentNotMovingTimer;

        playerCollider = player.GetComponent<Collider>();
	}
	
	void Update()
	{


		GetComponent<Rigidbody>().WakeUp();
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
            agentStopped = false;
            agent.Resume();
			if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
			{
				stateManager(1);
                if (agentStopped == false)
                {
                    agentStopped = true;
                    agent.Stop();
                }
			}
			
		}
			
			break;
		case enumStates.idle:
		{
			//--------------------------------------------------------//
			// idle, look around, without moving towards any waypoints//
			//--------------------------------------------------------//
            if (agentStopped == false)
            {
                agentStopped = true;
                agent.Stop();
            }
            

                if (ringOfSmellScript.smellDetected == false)
                {

                    if (idleTimer <= 0)
                    {
                        lastTarget = currentTarget;
                        currentTarget = targets[targetCounter];

                        if (agent.SetDestination(currentTarget.position) != null)
                        {

                           // agent.SetDestination(currentTarget.position);
                        }


                        idleTimer = defaultIdleTimer;
                        targetCounter++;
                        if (targetCounter >= targets.Count)
                        {
                            targetCounter = 0;
                        }
                        agent.speed = patrolSpeed;
                        if (agentStopped == true)
                        {
                            agentStopped = false;
                            agent.Resume();
                        }
                        stateManager(0);
                    }
                    idleTimer--;
                    if (idleTimer <= 0)
                    {
                        idleTimer = 0;
                    }

                }
                else
                {
                    RotateDogWhileSmelling();
                }
            
			break;
		}

		case enumStates.chase:
		{
			//----------------------------------------------------------------------------//
			// chase the Player constantly searching for a waypoint at the Player position//
			//----------------------------------------------------------------------------//
            //--------------------------//
            //Leap Attack While Chasing //
            //--------------------------//

            if (vectorx < chargeRange || vectorz < chargeRange)
            {    
                agent.autoBraking = false;
                enemyRotation = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z );                
                transform.LookAt(enemyRotation);
                transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, chaseSpeed/2 * Time.deltaTime);
                
                leapTimer--;
                    if(leapTimer <= 0)
                    {
                       // GetComponent<Rigidbody>().AddForce(leapPosition * impulse, ForceMode.Impulse);
                        agent.autoBraking = true;
                        leapTimer = defaultLeapTimer;
                    }
            }
            else
            {
                leapTimer--;
                if (leapTimer <= 0)
                {
                    agent.autoBraking = true;
                    leapTimer = defaultLeapTimer;
                }
            }

			//------------------//
			//Bark While chasing//
			//------------------// 
			if (barkTimer < 0)
			{
				newSphere = (GameObject)Instantiate(sphere, this.transform.position, Quaternion.identity);
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
			

			Physics.Linecast(transform.position, player.transform.position, out hit);
            Debug.DrawLine(transform.position, player.transform.position);
            if (hit.collider.tag != playerCollider.tag)
            {
                if (vectorx >= chaseRange || vectorz >= chaseRange)
                {
                    agent.speed = patrolSpeed;
                    if (alertArea[areaCounter] != null)
                    {
                        currentTarget = alertArea[areaCounter];
                    }

                    areaCounter++;
                    if (areaCounter > 2)
                    {
                        areaCounter = 0;
                    }
                    alertTimer = defaultAlertTimer;
                    stateManager(3);
                }
                else if (escapeTimer <= 0)
                {
                    agent.speed = patrolSpeed;
                    if (alertArea[areaCounter] != null)
                    {
                        currentTarget = alertArea[areaCounter];
                    }

                    areaCounter++;
                    if (areaCounter > 2)
                    {
                        areaCounter = 0;
                    }
                    alertTimer = defaultAlertTimer;
                    stateManager(3);
                }
                escapeTimer--;
            }
            else
            {
                agent.speed = chaseSpeed;
                if (currentTarget != player.transform)
                {
                    lastTarget = currentTarget;
                }            
                currentTarget = player.transform;
            }
		}
			break;



		case enumStates.alert:
			//------------------------------------------------------//
			//Look around a room by moving from waypoint to waypoint//
			//------------------------------------------------------//
            if (ringOfSmellScript.smellDetected == false)
            {
                if (alertTimer == 0 || alertTimer < 0)
                {
                    if (lastTarget != null)
                    {
                        currentTarget = lastTarget;
                        stateManager(4);
                    }
                }
                if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
                {
                    if (timer <= 0 && (!distracted))
                    {
                        lastTarget = currentTarget;
                        if (alertArea[areaCounter] != null)
                        {
                            currentTarget = alertArea[areaCounter];
                        }

                        areaCounter++;
                        if (areaCounter > 2)
                        {
                            areaCounter = 0;
                        }
                        if (tempcounters < 6)
                        {
                            if (turnCounter != 0)
                            {
                                turnCounter = 0;
                            }
                            if (idleTimer != defaultIdleTimer)
                            {
                                idleTimer = defaultIdleTimer;
                            }
                            tempcounters++;
                            stateManager(4);

                        }
                    }


                }
                else
                {
                    alertTimer--;
                    if (alertTimer <= 0)
                    {
                        alertTimer = 0;
                    }
                }
            }
            else if (ringOfSmellScript.smellDetected == true)
            {
                RotateDogWhileSmelling();
            }
								
			break;
		case enumStates.idleSuspicious:
		{
			//-----------------------------------------------//
			//Stand on the spot and look at preset directions//
			//-----------------------------------------------//
            if (agentStopped == false)
            {
                agentStopped = true;
                agent.Stop();
            }
            if (ringOfSmellScript.smellDetected == false)
            {

                if (coneOfVisionScript.playerSeen == true)
                {
                    agentStopped = false;
                    agent.Resume();
                    stateManager(2);
                }


                if (alertTimer > 0)
                {
                    alertTimer--;
                }

                if (alertTimer < 0)
                {
                    alertTimer = 0;
                }
                if (alertTimer <= 0)
                {
                    agent.speed = patrolSpeed;
                    turnCounter = 0;
                    agentStopped = false;
                    agent.Resume();
                    stateManager(0);
                }
                if (turnCounter < 3)
                {
                    currentTargetDirection = directionDegrees[0];
                    rotateEnemy(currentTargetDirection, rotationStep);

                    if (rotationCompleted)
                    {
                        directionDegrees.Add(directionDegrees[0]);
                        directionDegrees.Remove(directionDegrees[0]);
                        rotationCompleted = false;
                        turnCounter++;
                        turnTimer += defaultTurnTimer * Time.deltaTime;
                    }

                }

                if (turnCounter > 2)
                {


                    alertTimer = defaultAlertTimer;
                    turnCounter = 0;
                    agentStopped = false;
                    agent.Resume();
                    stateManager(3);
                }

                idleTimer--;
            }

            else if (ringOfSmellScript.smellDetected == true)
            {
                RotateDogWhileSmelling();
            }
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
            currentTarget = soundSource.transform;
            if (vectorx >= (waypointOffsetMin * 2) && vectorx <= (waypointOffsetMax * 2) && vectorz >= (waypointOffsetMin * 2) && vectorz <= (waypointOffsetMax * 2))
                alertTimer = defaultAlertTimer;
				stateManager(3);

			//---------------------------------------------//
			// when sound is heard, move towards the source//
			//---------------------------------------------//

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
				alertTimer += defaultAlertTimer;
				stateManager(3);
				currentTarget = alertArea[areaCounter];
			}
			
			if (eatTimer <= 0)
				
			{
				eatTimer = defaultEatTimer;
				distracted = false;
				vision.SetActive(true);
				smell.SetActive(true);
				eatBone = false;
				
				currentTarget = alertArea[areaCounter];
				Destroy(bone);
				alertTimer += defaultAlertTimer;
				stateManager(3);
				
			}
			eatTimer--;
			
		}
			
			break;
		default:
			break;
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
            if(vectorz <0)
            {
                vectorz *= -1;
            }
              if(vectorx <0)
            {
                vectorx *= -1;
            }
		}
		

		//-------------//
		//End of Update//
		//-------------//
	}
    void LateUpdate()
    {
        if (timer <= 0)
        {
            timer += defaultTimer;

            if (States != enumStates.idleSuspicious)
            {
             //   Vector3 tempVector = currentTarget.position;
                if (agent.SetDestination(currentTarget.position) != null)
                {
                    agent.SetDestination(currentTarget.position);
                }
            }
        }
        timer--;

        if (agentStopped == true)
        {
            agentNotMovingTimer--;
            if (agentNotMovingTimer <= 0)
            {
                agent.Resume();
                agentNotMovingTimer = defaultAgentNotMovingTimer;
            }
        }
        else if (agentStopped == false)
        {
            agentNotMovingTimer = defaultAgentNotMovingTimer;
        }

    }
	
	//-------------//
	//State Manager//
	//-------------//
	public void stateManager(int value)
	{
		States = (enumStates)value;
	}

	void setDirectionsForIdle()
	{
		
		directionDegrees.Add(firstDirection);
		directionDegrees.Add(secondDirection);
		directionDegrees.Add(thirdDirection);

	}
	
	void setTargetWaypoints()
	{
		/*
		targets.Add(target1);
		targets.Add(target2);
		targets.Add(target3);
        */

        // Marc's tests
        if (target1 != null)
        {
            targets.Add(target1);
        }

        if (target2 != null)
        {
            targets.Add(target2);
        }

        if (target3 != null)
        {
            targets.Add(target3);
        }

		if(target4 != null)
		{
			targets.Add(target4);
		}
        // End of Marc's tests
	}


    //==================================================//
    //================Rotate Enemy======================//
    //==================================================//


    void rotateEnemy(float targetDegrees, float rotationStep)
    {
        rotationDifference = 0;

        if (turnTimer <= 0)
        {
            if (rotationInProgress == false)
            {
                currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
                targetAngle = targetDegrees;
                rotationInProgress = true;
            }

            else if (rotationInProgress)
            {
                if (turnTimer == 0 && rotationDifference >= 0)
                {
                    if (targetAngle <= 180 && targetAngle >= 0) //decide which side the target is. 0-180 left, 0 - (-180)
                    {
                        //=============//
                        // First Sector//
                        //=============//
                        if (targetAngle <= 90 && targetAngle >= 0)// decide which sector the target is. 4 different sectors 0-90, 90-180, 0-(-90), (-90)- (-180)
                        {

                            if (currentAngle <= targetAngle && currentAngle > targetAngle - 180)
                            {
                                transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * -1); // Vector3.up * Time.deltaTime * rotationStep * -1
                                currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
                                rotationDifference = targetAngle - currentAngle;

                                if (rotationDifference < 0)
                                {
                                    rotationDifference = rotationDifference * -1;
                                }

                                if (currentAngle == targetAngle || angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
                                {
                                    rotationCompleted = true;
                                    rotationInProgress = false;
                                    turnTimer += defaultTurnTimer;
                                }
                            }
                            else //if (currentAngle > targetAngle && turnTimer == 0)
                            {
                                transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * 1); //Vector3.up * Time.deltaTime * rotationStep * 1
                                currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
                                rotationDifference = targetAngle - currentAngle;
                                if (currentAngle == targetAngle && angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
                                {
                                    rotationCompleted = true;
                                    rotationInProgress = false;
                                    turnTimer += defaultTurnTimer; // *Time.deltaTime;
                                }

                            }

                        }

                        //=============//
                        //Second Sector//
                        //=============//

                        else if (targetAngle > 90 && targetAngle <= 180)// decide which sector the target is
                        {
                            if (currentAngle > targetAngle || currentAngle <= targetAngle - 180)
                            {
                                transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * 1);
                                currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
                                rotationDifference = targetAngle - currentAngle;
                                if (rotationDifference < 0)
                                {
                                    rotationDifference = rotationDifference * -1;
                                }


                                if (currentAngle == targetAngle || angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
                                {
                                    rotationCompleted = true;
                                    rotationInProgress = false;
                                    turnTimer += defaultTurnTimer;
                                }
                            }
                            else
                            {
                                transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * -1);
                                currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
                                rotationDifference = targetAngle - currentAngle;
                                if (currentAngle == targetAngle && angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
                                {
                                    rotationCompleted = true;
                                    rotationInProgress = false;
                                    turnTimer += defaultTurnTimer;
                                }
                            }
                        }
                    }

                    else if (targetAngle < 0 && targetAngle > -180)  //decide which side the target is
                    {

                        //=============//
                        //Third Sector //
                        //=============//
                        if (targetAngle >= -90)// decide which sector the target is. 4 different sectors 0-90, 90-180, 0-(-90), (-90)- (-180)
                        {
                            if (currentAngle >= targetAngle && currentAngle <= 180 + targetAngle)
                            {
                                transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * 1);
                                currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
                                rotationDifference = targetAngle - currentAngle;
                                if (rotationDifference < 0)
                                {
                                    rotationDifference = rotationDifference * -1;
                                }
                                if (currentAngle == targetAngle || angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
                                {
                                    rotationCompleted = true;
                                    rotationInProgress = false;
                                    turnTimer += defaultTurnTimer; 
                                }
                            }
                            else //if (currentAngle < targetAngle && turnTimer == 0)
                            {
                                transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * -1);
                                currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
                                rotationDifference = targetAngle - currentAngle;
                                if (currentAngle == targetAngle && angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
                                {
                                    rotationCompleted = true;
                                    rotationInProgress = false;
                                    turnTimer += defaultTurnTimer;
                                }

                            }
                        }
                        //=============//
                        //Fourth Sector//
                        //=============//
                        else if (targetAngle < -90)// decide which sector the target is. 4 different sectors 0-90, 90-180, 0-(-90), (-90)- (-180)
                        {
                            if (currentAngle >= targetAngle && currentAngle <= 180 + targetAngle)
                            {                              
                                transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * 1);
                                currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
                                rotationDifference = targetAngle - currentAngle;
                                if (rotationDifference < 0)
                                {
                                    rotationDifference = rotationDifference * -1;
                                }                               
                                if (currentAngle == targetAngle || angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
                                {
                                    rotationCompleted = true;
                                    rotationInProgress = false;
                                    turnTimer += defaultTurnTimer; 
                                }
                            }
                            else //if (currentAngle < targetAngle && turnTimer == 0)
                            {
                                transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * -1);
                                currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
                                rotationDifference = targetAngle - currentAngle;
                                if (currentAngle == targetAngle && angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
                                {
                                    rotationCompleted = true;
                                    rotationInProgress = false;
                                    turnTimer += defaultTurnTimer; 
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

    public void RotateDogWhileSmelling()
    {
        agentStopped = true;
        agent.Stop();
        Vector3 relative = transform.InverseTransformPoint(player.transform.position);
        float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
        transform.Rotate(0, angle * Time.deltaTime * 1.0f, 0);      
    }

    void checkContinuousSmelling()
    {
        minusSmellTimer();
        if (turnTowardsSmellTimer == 0)
        {
            SeekForSmellSource = true;
        }
        if (SeekForSmellSource == true)
        {
            agentStopped = true;
            agent.Stop();
            RotateDogWhileSmelling();
        }

    }

    void minusSmellTimer()
    {
        turnTowardsSmellTimer--;
    if(turnTowardsSmellTimer <= 0)
    {
        turnTowardsSmellTimer = 0;
    }
    }
}
