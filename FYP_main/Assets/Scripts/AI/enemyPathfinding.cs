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
	RaycastHit hit; 
	
	
	public Transform target1;
	public Transform target2;
	public Transform target3;
	public Transform currentTarget;
	public Transform lastTarget;
	public Vector3 lastSeenPosition;
	
	
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
	public List<Transform> alertArea = new List<Transform>();
	
	bool hasWaypointsLeft;
	public bool eatBone = false;
	public bool distracted = false;
	
	public float turnSpeed = 2.0f;
	
	public float speed = 10;
	
	float maxSpeed = 20;
	float maxScale = 60;
	float waypointOffsetMin = -0.05f;
	float waypointOffsetMax = 0.05f;
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
	float rotationStep = 65.0f;
	public float rotationDegrees = 90;
	public float currentAngle = 0;
	float targetAngle = 0;
	public float angleOffsetMax = 10.0f;
	public float angleOffsetMin = -10.0f;
	bool rotationInProgress = false;
	public bool rotationCompleted = false;
	public float turnTimer = 100.0f;
	float currentTargetDirection;
	int turnCounter = 0;

	//So many timers
	int tempcounters = 0;
	public int timer;
	public int idleTimer;    
	public int barkTimer;
	public float escapeTimer;
	public float alertTimer;
	public float eatTimer;
	public int defaultEatTimer;
	public int defaultIdleTimer;
	public int defaultBarkTimer;
	public int defaultTimer;
	public int defaultAlertTimer;
	public int defaultEscapeTimer;
	public int playerOutOfSight;
	int targetIndex;
	int targetCounter = 0;
	public int areaCounter = 0;
	public float defaultTurnTimer;
	public int defaultDetectSoundTimer;
	int detectSoundTimer;
	
	Vector3[] path = new Vector3[0];
	Vector3 currentWaypoint;
	
	//values if enemy doesn't receive a new waypoint to prevent them from being stuck
	Vector3 worldPositionNow;
	Vector3 worldPositionPast;
	//int checkIfStuck = 100;
	//bool isStuck = false;
	
	void Start()
	{        
		
		
		player = GameObject.FindGameObjectWithTag("player");
		setDirectionsForIdle();
		setTargetWaypoints();
		currentTarget = targets[0];
		//print (targets [0] + "targets[0]");
		lastTarget = currentTarget;
		agent = GetComponent<NavMeshAgent>();
		
		agent.SetDestination(currentTarget.position);
		//pathRequestManager.requestPath (transform.position, currentTarget.position, onPathFound);        
		
		//Setting Timers
		timer = defaultTimer;
		eatTimer = defaultEatTimer;
		idleTimer = defaultIdleTimer;
		barkTimer = defaultBarkTimer;
		alertTimer = defaultAlertTimer;
		escapeTimer = defaultEscapeTimer;
		turnTimer = defaultTurnTimer;
		detectSoundTimer = defaultDetectSoundTimer;
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
			//print (currentTarget + " << currentTarget chase 1");

			//currentTarget = lastTarget;
			//----------------------------------------------------------------------------//
			// chase the Player constantly searching for a waypoint at the Player position//
			//----------------------------------------------------------------------------//
			//currentTarget = player.transform;
			//------------------//
			//Bark While chasing//
			//------------------//
			if (barkTimer < 0)
			{
				//print (currentTarget + " << currentTarget chase 2");
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
			//print (hit);
			if (hit.collider == player.GetComponent<Collider>())
			{
				lastSeenPosition = player.transform.position;
				currentTarget.position = lastSeenPosition;
				//print (currentTarget + " << currentTarget chase 3");
			}
			else{				//timer = defaultTimer;
				if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
				{
					//print("ImOuttaHere");
					escapeTimer = defaultEscapeTimer;
					playerOutOfSight = 2;
					if(alertArea[areaCounter] != null)
					{
					currentTarget = alertArea[areaCounter];
					}

					areaCounter++;
					if(areaCounter > 2)
					{
						areaCounter = 0;
						//print (currentTarget + " << currentTarget chase 4");
					}
					stateManager(3);
				}
			}
			escapeTimer-= Time.deltaTime;
		}
			break;



		case enumStates.alert:
			//------------------------------------------------------//
			//Look around a room by moving from waypoint to waypoint//
			//------------------------------------------------------//

			//print (currentTarget + " <<  currentTarget Alert 1");
//
//			if(alertTimer == 0 || alertTimer < 0)
//			{
//				if(lastTarget != null)
//				{
//					currentTarget = lastTarget;
//					stateManager(0);
//				}
//			}
//			Physics.Linecast(transform.position, player.transform.position, out hit);
//			if (hit.collider == player.GetComponent<Collider>())
//			{
//				lastSeenPosition = player.transform.position;
//				currentTarget.position = lastSeenPosition;
//
//				tempcounters = 0;
//				stateManager(2);
//			}
//			else
//			{

				if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
				{

					//print (vectorx + "  << vectorX  " + vectorz + "  << vectorz" + waypointOffsetMin + "  <<  waypointoffsetMin  " + waypointOffsetMax + "  << waypointoffsetmax  ");
					if (timer <= 0 && (!distracted))
					{	
						lastTarget = currentTarget;
						if(alertArea[areaCounter] != null)
						{
						currentTarget = alertArea[areaCounter];
						}

						areaCounter++;
						if (areaCounter > 2)
						{
							areaCounter = 0;
						}
						if(tempcounters < 6)
						{
							if(turnCounter != 0)
							{
								turnCounter = 0;
							}
							if(idleTimer != 30)
							{
								idleTimer = 30;
							}
							print ("state vaihdettu: 4");
							stateManager(4);
							tempcounters++;
						}
					}
					
					
				}
								
				alertTimer--;
				if(alertTimer <= 0)
				{
					alertTimer = 0;
				}
				

//			}

			break;
		case enumStates.idleSuspicious:
		{
			//-----------------------------------------------//
			//Stand on the spot and look at preset directions//
			//-----------------------------------------------//
			if(alertTimer > 0)
			{
				alertTimer--;
			}

			if(alertTimer < 0)
			{
				alertTimer= 0;
			}
			if(turnCounter < 3)
			{
				currentTargetDirection = directionDegrees[0];	
				rotateEnemy(currentTargetDirection, rotationStep);
				print ("enemy rotating!");
				//turnCounter++;

				if (rotationCompleted)
				{	
					print ("rotationCompleted !");
					directionDegrees.Add(directionDegrees[0]);
					directionDegrees.Remove(directionDegrees[0]);							
					rotationCompleted = false;
					turnCounter++;
					turnTimer += defaultTurnTimer * Time.deltaTime;
				} 
				
			}			
			
			if (turnCounter > 2)
			{
					print (tempcounters + " << tempcounters");
					if(tempcounters > 5)
					{
						tempcounters = 0;
						stateManager(0);
					}

					if(tempcounters < 6)
					{
					print ("vaihtaa alertiin");
						stateManager(3);
					}

				idleTimer--;	
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

			detectSoundTimer--;
			if(detectSoundTimer <= 0)
			{
				stateManager(4);
				detectSoundTimer += defaultDetectSoundTimer;
			}
			//---------------------------------------------//
			// when sound is heard, move towards the source//
			//---------------------------------------------//
			if(GameObject.FindGameObjectWithTag("brokenObject") != null)
			{
				brokenObject = GameObject.FindGameObjectWithTag("brokenObject");
			}

			if(GameObject.FindGameObjectWithTag("bone") != null)
			{
				bone = GameObject.FindGameObjectWithTag("bone");
			}

			if(brokenObject)
			{
				Vector3 objectdir = (brokenObject.transform.localPosition) - (this.transform.localPosition);
				if (objectdir.x <= 2 && objectdir.x >= -2 && objectdir.z <= 2 && objectdir.z >= -2 || !brokenObject) 
				{
					stateManager(0);
					if(lastTarget != null)
					{
						currentTarget = lastTarget;
						print ("currentTarget0: " + currentTarget);
					}

					
					
				}
				else
				{
					if(brokenObject.transform != null)
					{

						currentTarget = brokenObject.transform;
						print ("currentTarget1: " + currentTarget);
					}

					
					
				}
			}
			else if(bone)
			{
				stateManager(5);
				if(bone.transform != null)
				{
					currentTarget = bone.transform;
					print ("currentTarget2: " + currentTarget);
				}

				
				GameObject temp = GameObject.FindGameObjectWithTag("Vision");
				vision = temp.gameObject;
				vision.SetActive(false);
				
				temp = GameObject.FindGameObjectWithTag("Smell");
				smell = temp.gameObject;
				smell.SetActive(false);
			}
			else
			{
				currentTarget = soundSource.transform;
				print ("current target3: " + currentTarget);
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
				alertTimer += defaultAlertTimer;
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
				alertTimer += defaultAlertTimer;
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
			timer+=defaultTimer;

			if(States != enumStates.idleSuspicious)
			{
			if(agent.SetDestination(currentTarget.position) != null)
			{
				agent.SetDestination(currentTarget.position);
			}
			}
			//pathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
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
	
	//==================================================//
	//================rotate enemy======================//
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
						if (targetAngle <= 90)// decide which sector the target is. 4 different sectors 0-90, 90-180, 0-(-90), (-90)- (-180)
						{
							
							if (currentAngle <= targetAngle || currentAngle > targetAngle - 180)
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
								if (currentAngle == targetAngle || angleOffsetMin <= rotationDifference && rotationDifference <= angleOffsetMax)
								{
									rotationCompleted = true;
									rotationInProgress = false;
									turnTimer += defaultTurnTimer * Time.deltaTime;
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
									turnTimer += defaultTurnTimer * Time.deltaTime;
									//print(rotationCompleted + " rotationCompleted" + rotationInProgress + "  rotation in progress  " + turnTimer + " <<  turnTimer");
								}
								
							}
							
						}
						
						//=============//
						//Second Sector//
						//=============//
						
						else if (targetAngle > 90 && turnTimer == 0)// decide which sector the target is
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
									turnTimer += defaultTurnTimer * Time.deltaTime;
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
									turnTimer += defaultTurnTimer * Time.deltaTime;
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
									turnTimer += defaultTurnTimer * Time.deltaTime;
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
									turnTimer += defaultTurnTimer * Time.deltaTime;
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
									turnTimer += defaultTurnTimer * Time.deltaTime;
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
									turnTimer += defaultTurnTimer * Time.deltaTime;
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
