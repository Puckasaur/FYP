﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum enumStatesFatDog
{
	
	patrol = 0,
    idle = 1,  // not needed
    chase = 2, // cone of vision & ring of smell
    alert = 3, // cone of vision & ring of smell
    idleSuspicious = 4, // basic state
	distracted = 5,
	detectSound = 6,
	eatBone = 7
}

public class fatDogAi : MonoBehaviour {
	
	soundSphere sphereScript;
	RaycastHit hit;
	
	public Vector3 respawnPosition;
	
	public Transform target1;
	
	public Transform currentTarget;
	public Transform lastTarget;
	public Vector3 lastSeenPosition = new Vector3(0,0,0);
	
	public enumStates States;
	GameObject vision;
	GameObject smell;
	GameObject bone;
	GameObject player;
	GameObject newSphere;
	public GameObject sphere;
	public GameObject soundSource;
	GameObject brokenObject;
	
	NavMeshAgent agent;
	List<Transform> targets = new List<Transform>();
	public bool eatBone = false;
	public bool distracted = false;
	
	public float turnSpeed = 2.0f;
	
	public float speed = 2;
	
	float maxSpeed = 5;
	float maxScale = 60;
	float waypointOffsetMin = -2.05f;
	float waypointOffsetMax = 2.05f;
	float vectorTransformPositionx = 0;
	float vectorTransformPositionz = 0;
	float vectorCurrentTargetx = 0;
	float vectorCurrentTargetz = 0;
	float vectorx;
	float vectorz;
	
	
	//Idle Suspicious values
	public bool idleSuscpicious = false;
	public float firstDirection;
	public float secondDirection;
	public float thirdDirection;
    public float fourthDirection;
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
    public float turnTimer;
	float currentTargetDirection;
	int turnCounter = 0;
	
	//So many timers
	public int timer;
	public int idleTimer;    
	public int barkTimer;
	public float escapeTimer;
	public float eatTimer;
	public int defaultEatTimer;
	public int defaultIdleTimer;
	public int defaultBarkTimer;
	public int defaultTimer;
	public int defaultEscapeTimer;
	public int playerOutOfSight;
	int targetIndex;
	int targetCounter = 0;
	public int areaCounter = 0;
	public float defaultTurnTimer;
	public int defaultDetectSoundTimer;
	int detectSoundTimer;

	public float patrolSpeed;
	public float chaseSpeed;
    public float chaseRange;
	
	Vector3[] path = new Vector3[0];
	Vector3 currentWaypoint;
	
	//values if enemy doesn't receive a new waypoint to prevent them from being stuck
	Vector3 worldPositionNow;
	Vector3 worldPositionPast;
	//int checkIfStuck = 100;
	//bool isStuck = false;
	
	void Start()
	{
		
		//respawnPosition = this.transform.position;
		player = GameObject.FindGameObjectWithTag("player");
		setDirectionsForIdle();
		setTargetWaypoints();
		currentTarget = targets[0];
		lastTarget = currentTarget;
		agent = GetComponent<NavMeshAgent>();
		agent.speed = patrolSpeed;
		agent.SetDestination(currentTarget.position);
        stateManager(0);
		//Setting Timers
		timer = defaultTimer;
		eatTimer = defaultEatTimer;
		idleTimer = defaultIdleTimer;
		barkTimer = defaultBarkTimer;
		escapeTimer = defaultEscapeTimer;
		turnTimer = defaultTurnTimer;
		detectSoundTimer = defaultDetectSoundTimer;
	}
	
	void Update()
	{
        /// Calcumalationen for ze vector difference///
        if (currentTarget != null)
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
            print(vectorTransformPositionx + " <<  vectorTransformPositionx   " + vectorCurrentTargetx + " << vectorCurrentTargetx");
            print(vectorTransformPositionz + " <<  vectorTransformPositionz   " + vectorCurrentTargetz + " << vectorCurrentTargetz");
            vectorx = (vectorTransformPositionx - vectorCurrentTargetx);
            vectorz = (vectorTransformPositionz - vectorCurrentTargetz);
        }

        /// End of Calcumalationen for ze vector difference///


		
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
			if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
			{
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
				
				if(agent.SetDestination(currentTarget.position) != null)
				{
					agent.SetDestination(currentTarget.position);
				}
				
				
				idleTimer = defaultIdleTimer;
				targetCounter++;
				if (targetCounter >= targets.Count)
					
				{
					targetCounter = 0;
				}
				stateManager(4);
			}
			idleTimer--;
            if (idleTimer <= 0)
            {
                idleTimer = 0;
            }
			break;
		}
			
		case enumStates.chase:
		{
                   
            Physics.Linecast(transform.position, player.transform.position, out hit);
            if (hit.collider.tag != player.GetComponent<Collider>().tag)
            {
                if (vectorx < chaseRange || vectorz < chaseRange)
                {
                    if (barkTimer < 0)
                    {
                        bark();
                    }
                    barkTimer--;                    
                }               
            }

		}
			break;
			
			
			
		case enumStates.alert:
		{
            stateManager(4);
		}
			break;
		case enumStates.idleSuspicious:
		{
			//-----------------------------------------------//
			//Stand on the spot and look at preset directions//
			//-----------------------------------------------//

            if (turnCounter < directionDegrees.Count)
			{
				currentTargetDirection = directionDegrees[0];	
				rotateEnemy(currentTargetDirection, rotationStep);
				
				if (rotationCompleted)
				{	
					print ("rotationCompleted !>> " + directionDegrees[0]);
					directionDegrees.Add(directionDegrees[0]);
					directionDegrees.Remove(directionDegrees[0]);							
					rotationCompleted = false;
					turnCounter++;
					turnTimer += defaultTurnTimer * Time.deltaTime;
				} 
				
			}

            else if (turnCounter >= directionDegrees.Count)
			{

					//alertTimer = defaultAlertTimer;
					turnCounter = 0;
					stateManager(3);

				
				
			}
            idleTimer--;
            if (idleTimer < 0)
            {
                idleTimer = 0;
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
			
			//detectSoundTimer--;
			//if(detectSoundTimer <= 0)
			currentTarget = soundSource.transform;
			if (vectorx >= (waypointOffsetMin * 2) && vectorx <= (waypointOffsetMax * 2) && vectorz >= (waypointOffsetMin * 2) && vectorz <= (waypointOffsetMax * 2))
				//alertTimer = defaultAlertTimer;
			stateManager(3);
			//detectSoundTimer += defaultDetectSoundTimer;
			//}
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
				//alertTimer += defaultAlertTimer;
				stateManager(3);
				//currentTarget = alertArea[areaCounter];
			}
			
			if (eatTimer <= 0)
				
			{
				eatTimer = defaultEatTimer;// 120;
				distracted = false;
				vision.SetActive(true);
				smell.SetActive(true);
				eatBone = false;
				
				//currentTarget = alertArea[areaCounter];
				Destroy(bone);
				//alertTimer += defaultAlertTimer;
				stateManager(3);
				
			}
			eatTimer--;
			
		}
			
			break;
		default:
			break;
		}
		
		if(timer <= 0)
		{
			timer+=defaultTimer;
			
			if(States != enumStates.idleSuspicious)
			{
				if(agent.SetDestination(currentTarget.position) != null)
				{
					agent.SetDestination(currentTarget.position);
				}
			}
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
	
	void setDirectionsForIdle()
	{
		directionDegrees.Add(firstDirection);
		directionDegrees.Add(secondDirection);
		directionDegrees.Add(thirdDirection);
        directionDegrees.Add(fourthDirection);
	}
	
	void setTargetWaypoints()
	{
		if (target1 != null) 
		{
			targets.Add(target1);
		}
	}

    void bark()
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
	
	//==================================================//
	//================Rotate Enemy======================//
	//==================================================//
	
	
	void rotateEnemy(float targetDegrees, float rotationStep)
	{
		float rotationDifference = 0;
		
		if (turnTimer <= 0)
		{
			//print (turnTimer + "  << TurnTimer");
			if (rotationInProgress == false)
			{
				currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
				targetAngle = targetDegrees;//currentAngle + targetDegrees;
				rotationInProgress = true;
				//print("current angle:  " + currentAngle + "target angle:  " + targetAngle);
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
								print("entered the rotation loop");
								transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * 1);
								currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
								rotationDifference = targetAngle - currentAngle;
								//print(rotationDifference + " << rotation    " + targetAngle + " <<  target Angle    " + currentAngle + " << current Angle");
								if (rotationDifference < 0)
								{
									rotationDifference = rotationDifference * -1;
								}
								
								//print(currentAngle + "  << current Angle  " + angleOffsetMin + "  <<angleOffsetMin    " + angleOffsetMax + "  <<angleOffsetMax   " + rotationDifference + "  << rotationDifference");
								if (currentAngle == targetAngle && angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
								{
									rotationCompleted = true;
									rotationInProgress = false;
                                    turnTimer += defaultTurnTimer; // *Time.deltaTime;
									//print(rotationCompleted + " rotationCompleted" + rotationInProgress + "  rotation in progress  " + turnTimer + " <<  turnTimer");
								}
							}
							else //if (currentAngle > targetAngle && turnTimer == 0)
							{
								
								print("entered the rotation loop 2");
								transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * -1);
								currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
								rotationDifference = targetAngle - currentAngle;
								if (currentAngle == targetAngle && angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
								{
									rotationCompleted = true;
									rotationInProgress = false;
                                    turnTimer += defaultTurnTimer; // *Time.deltaTime;
									//print(rotationCompleted + " rotationCompleted" + rotationInProgress + "  rotation in progress  " + turnTimer + " <<  turnTimer");
								}
								
							}
							
						}
						
						//=============//
						//Second Sector//
						//=============//
						
						else if (targetAngle > 90 && targetAngle <= 180)// decide which sector the target is
						{
							if ( currentAngle > targetAngle || currentAngle <= targetAngle - 180 )
							{
								print("entered the rotation loop 3");
								transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * 1);
								currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
								rotationDifference = targetAngle - currentAngle;
								//print(rotationDifference + " << rotation difference   " + targetAngle + " <<  target Angle    " + currentAngle + " << current Angle");
								if (rotationDifference < 0)
								{
									rotationDifference = rotationDifference * -1;
								}
								
								
								if (currentAngle == targetAngle || angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
								{
									rotationCompleted = true;
									rotationInProgress = false;
                                    turnTimer += defaultTurnTimer; // *Time.deltaTime;
									//print(rotationCompleted + " rotationCompleted" + rotationInProgress + "  rotation in progress  " + turnTimer + " <<  turnTimer");
								}
							}
							else //if (currentAngle > targetAngle || targetAngle - 180 >= currentAngle)
							{
								print("entered the rotation loop 4");
								transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * -1);
								currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
								rotationDifference = targetAngle - currentAngle;
								if (currentAngle == targetAngle && angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
								{
									rotationCompleted = true;
									rotationInProgress = false;
                                    turnTimer += defaultTurnTimer; // *Time.deltaTime;
									//print(rotationCompleted + " rotationCompleted" + rotationInProgress + "  rotation in progress  " + turnTimer + " <<  turnTimer");
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
								print("entered the rotation loop 5");
								transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * 1);
								currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
								rotationDifference = targetAngle - currentAngle;
								//print(rotationDifference + " << rotation    " + targetAngle + " <<  target Angle    " + currentAngle + " << current Angle");
								if (rotationDifference < 0)
								{
									rotationDifference = rotationDifference * -1;
								}
								
								//print(currentAngle + "  << current Angle  " + angleOffsetMin + "  <<angleOffsetMin    " + angleOffsetMax + "  <<angleOffsetMax   " + rotationDifference + "  << rotationDifference");
								if (currentAngle == targetAngle || angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
								{
									rotationCompleted = true;
									rotationInProgress = false;
                                    turnTimer += defaultTurnTimer; // *Time.deltaTime;
									//print(rotationCompleted + " rotationCompleted" + rotationInProgress + "  rotation in progress  " + turnTimer + " <<  turnTimer");
								}
							}
							else //if (currentAngle < targetAngle && turnTimer == 0)
							{
								
								print("entered the rotation loop 6");
								transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * -1);
								currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
								rotationDifference = targetAngle - currentAngle;
								if (currentAngle == targetAngle && angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
								{
									rotationCompleted = true;
									rotationInProgress = false;
                                    turnTimer += defaultTurnTimer; // *Time.deltaTime;
									//print(rotationCompleted + " rotationCompleted" + rotationInProgress + "  rotation in progress  " + turnTimer + " <<  turnTimer");
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
								print("entered the rotation loop 7");
								transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * 1);
								currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
								rotationDifference = targetAngle - currentAngle;
								//print(rotationDifference + " << rotation    " + targetAngle + " <<  target Angle    " + currentAngle + " << current Angle");
								if (rotationDifference < 0)
								{
									rotationDifference = rotationDifference * -1;
								}
								
								//print(currentAngle + "  << current Angle  " + angleOffsetMin + "  <<angleOffsetMin    " + angleOffsetMax + "  <<angleOffsetMax   " + rotationDifference + "  << rotationDifference");
								if (currentAngle == targetAngle || angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
								{
									rotationCompleted = true;
									rotationInProgress = false;
                                    turnTimer += defaultTurnTimer; // *Time.deltaTime;
									//print(rotationCompleted + " rotationCompleted" + rotationInProgress + "  rotation in progress  " + turnTimer + " <<  turnTimer");
								}
							}
							else //if (currentAngle < targetAngle && turnTimer == 0)
							{
								
								print("entered the rotation loop 8");
								transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * -1);
								currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
								rotationDifference = targetAngle - currentAngle;
								if (currentAngle == targetAngle && angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
								{
									rotationCompleted = true;
									rotationInProgress = false;
                                    turnTimer += defaultTurnTimer; // *Time.deltaTime;
									//print(rotationCompleted + " rotationCompleted" + rotationInProgress + "  rotation in progress  " + turnTimer + " <<  turnTimer");
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
	

}
