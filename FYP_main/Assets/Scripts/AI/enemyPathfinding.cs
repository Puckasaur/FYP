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
	eatBone = 7,
    smell = 8
}

public class enemyPathfinding : MonoBehaviour
{
    //Detection variables//

    //smell detection

    ringOfSmell ringOfSmellScript;
    GameObject bone;
    
    //rotation after smelling values
    public Vector3 tempSmellPosition;
    public bool continueRotation = false;

    //These variables are for the enemies to use when they smell a bone
    float maxRange = 1.5f;
    Vector3 soundSourcePos;
    Transform tempWaypointPos;

    //end of rotation after smelling values

    //sound detection
    soundSphere sphereScript;
    GameObject newSphere;
    public GameObject sphere;
    public GameObject soundSource;
    GameObject brokenObject;

    //vision detection
    coneOfVision coneOfVisionScript;


    //end of Detection variables

    RaycastHit hit;
    public Vector3 respawnPosition;

    //Pathfinding variables
    public Transform target1;
    public Transform target2;
    public Transform target3;
    public Transform target4;

    public Transform currentTarget;
    public Transform lastTarget;

    float maxScale = 20;
    float waypointOffsetMin = -2.05f;
    float waypointOffsetMax = 2.05f;
    float vectorTransformPositionx = 0;
    float vectorTransformPositionz = 0;
    float vectorCurrentTargetx = 0;
    float vectorCurrentTargetz = 0;
    float vectorx;
    float vectorz;
	
	  Vector3[] path = new Vector3[0];
    Vector3 currentWaypoint;
    //End of Pathfinding variables

    public GameObject visionRotator;
    public GameObject player;
    public enumStates States;
    
    public float patrolSpeed;
    public float chaseSpeed;
    public float chaseRange;
  
    public NavMeshAgent agent;
    public List<Transform> targets = new List<Transform>();
    public List<Transform> alertArea = new List<Transform>();

    public bool isOnWaypoint = false;
    public bool isPaired = false;
    public bool eatBone = false;
    public bool distracted = false;


    //Idle Suspicious variables
    float rotationDifference = 0;
    public bool idleSuscpicious = false;
    public float firstDirection;    //-These are used to determine where the opponen will look when it
    public float secondDirection;   // reaches the waypoint.
    public float thirdDirection;    //Insert integer to set the angle between -180 and 180. 
    List<float> directionDegrees = new List<float>();
    GameObject enemyObject;

    
    float rotationStep = 65.0f;             //-Enemies turning speed
    public float rotationDegrees = 90;
    public float currentAngle = 0;
    public float targetAngle = 0;
    public float angleOffsetMax = 10.0f;    // -These values are used to prevent the Unity from missing
    public float angleOffsetMin = -10.0f;   // the right angle during updates.
    public float turnTimer = 100.0f;        // -This is used to determine how long the enemy will sit idling between turning from a single angle to another.
    float currentTargetDirection;           
    int turnCounter = 0;
    bool rotating = false;
    bool rotationInProgress = false;
    public bool rotationCompleted = false;
    public bool SeekForSmellSource = false;
    public bool agentStopped = false;
    // end of Idle Suspicious variables

    //Timers
    int tempcounters = 0;
    int timer;
    public int idleTimer;
    int barkTimer;
    float escapeTimer;
    public float alertTimer;
    public float eatTimer;
    public int defaultEatTimer;
    public int defaultIdleTimer;
    public int defaultBarkTimer;
    public int defaultTimer;
    public int defaultAlertTimer;
    public int defaultEscapeTimer;
    public int targetCounter = 0;
    public int areaCounter = 0;
    public float defaultTurnTimer;
    public int defaultDetectSoundTimer;
    int detectSoundTimer;
    public float turnTowardsSmellTimer;
    public float defaultTurnTowardsSmellTimer;
    public float agentNotMovingTimer;
    public float defaultAgentNotMovingTimer;
    //end of Timers

  

    //Charge variables
    float chargeTimer;
    public float defaultChargeTimer;
    public float chargeRange;
    Vector3 enemyRotation;
    //end of Charge variables

    //If enemies get stuck variables
    float tempPosX = 0;
    float tempPosZ = 0;
    bool visited = false;
    //end of If enemies get stuck variables

    //Alert waypoint organization variables
    List<Transform> usedWaypoints = new List<Transform>();
    List<Vector3> waypointLocations = new List<Vector3>();
    List<Transform> tempAlertWaypoints = new List<Transform>();
    float closestWaypointValue = 0;
    int currentWaypointIndex = 0;
    float waypointLocationValue = 0;
    Vector3 waypointDifference;
    //end of Alert waypoint organization variables

    //Misc variables
    Collider playerCollider;
    Animator patrolAnim;
    bool randomPointSelected = false;

    //This is for Animator guy to see enemies actual speeds, it uses normal update atm.
    //It can be changed to FixedUpdate if it gives better results
    Vector3 previousPosition;
    Vector3 currentPosition;
    public float currentSpeed;
    //end of Misc variables


    void Start()
    {
        respawnPosition = this.transform.position;

        visionRotator = GameObject.FindGameObjectWithTag("visionRotator");        
        player = GameObject.FindGameObjectWithTag("player");
        ringOfSmellScript = player.GetComponentInChildren<ringOfSmell>();
        coneOfVisionScript = GetComponentInChildren<coneOfVision>();
        patrolAnim = gameObject.GetComponent<Animator>();
		
        //Setting waypoints and Navigation Mesh agent
        setDirectionsForIdle();
        setTargetWaypoints();
        currentTarget = targets[0];
        lastTarget = currentTarget;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        agent.SetDestination(currentTarget.position);
        //end of Setting waypoints and Navigation Mesh agent

        //Setting Timers
        chargeTimer = defaultChargeTimer;
        timer = defaultTimer;
        eatTimer = defaultEatTimer;
        barkTimer = defaultBarkTimer;
        alertTimer = defaultAlertTimer;
        escapeTimer = defaultEscapeTimer;
        turnTimer = defaultTurnTimer;
        detectSoundTimer = defaultDetectSoundTimer;
        turnTowardsSmellTimer = defaultTurnTowardsSmellTimer;
        agentNotMovingTimer = defaultAgentNotMovingTimer;
        //end of Setting Timers

        //Misc 
        playerCollider = player.GetComponent<Collider>();
        //end of Misc
    }


    void Update()
    {
        //Velocity meter for the animator guy to see enemies actual speed
        if (agent.velocity != Vector3.zero)
        {
            Vector3 currentMove = transform.position - previousPosition;
            currentSpeed = currentMove.magnitude / Time.deltaTime;
            previousPosition = transform.position;
            updateAnimator();
        }
        //end of Timer for the animator guy to see enemies actual speed

        //To prevent opponent from sleeping
        GetComponent<Rigidbody>().WakeUp();
        //end of To prevent opponent from sleeping


        //------------------//
        //Code of the states//
        //------------------//
        switch (States)
        {

            case enumStates.patrol:
                {
                    //-----------------------------------------------------------------------------------------//
                    //patrol, moves from one waypoint to the next waiting for a second before advancing forward//
                    //-----------------------------------------------------------------------------------------//
                    patrolAnim.SetBool("patrolRun", false);
                    if (agentStopped == true && isPaired)
                    {
                        agentStopped = false;
                        agent.Resume();
                    }

                    //Check if the player is within offset range from the current target

                    if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
                    {
                        stateManager(1);

                        if (agentStopped == false)
                        {
                            agentStopped = true;
                            agent.Stop();

                           // patrolAnim.SetBool("patrolWalk", false);
                            patrolAnim.SetBool("patrolRun", false);

                        }
                    }
                }

                break;
            case enumStates.idle:
                {
                    //--------------------------------------------------------//
                    // idle, look around, without moving towards any waypoints//
                    //--------------------------------------------------------//
                    patrolAnim.SetBool("patrolRun", false);
                    if (agentStopped == false)
                    {
                        agentStopped = true;
                        agent.Stop();
                    }
                    //Check if the player is within offset range from the current target
                    if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
                    {

                        if (idleTimer <= 0)
                        {
                            if (currentTarget.gameObject.tag != "bone")
                            {
                                lastTarget = currentTarget;
                            }
                            if (isPaired == true)
                            {
                                idleTimer = defaultIdleTimer;
                                isOnWaypoint = true;
                            }
                            idleTimer = defaultIdleTimer;
                            if (isPaired == false)
                            {
                                currentTarget = targets[targetCounter];
                                stateManager(0);
                                targetCounter++;
                                if (targetCounter >= targets.Count)
                                {
                                    targetCounter = 0;
                                }
                            }
                            agent.speed = patrolSpeed;
                        }
                        idleTimer--;
                        if (idleTimer <= 0)
                        {
                            idleTimer = 0;
                        }
                    }
                    else
                    {
                        patrolAnim.SetBool("patrolRun", false);
                        stateManager(0);
                    }

                    break;
                }

            case enumStates.chase:
                {
                    //----------------------------------------------------------------------------//
                    // chase the Player constantly searching for a waypoint at the Player position//
                    //----------------------------------------------------------------------------//

                    patrolAnim.SetBool("patrolRun", true);

                    if (vectorx < chargeRange || vectorz < chargeRange)
                    {
                        agent.autoBraking = false;
                        enemyRotation = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                        transform.LookAt(enemyRotation);
                        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, chaseSpeed / 2 * Time.deltaTime);

                        chargeTimer--;
                        if (chargeTimer <= 0)
                        {
                            agent.autoBraking = true;
                            chargeTimer = defaultChargeTimer;
                        }
                    }
                    else
                    {
                        chargeTimer--;
                        if (chargeTimer <= 0)
                        {
                            agent.autoBraking = true;
                            chargeTimer = defaultChargeTimer;
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


                    Physics.Linecast(this.transform.position, player.transform.position, out hit);
                    Debug.DrawLine(this.transform.position, player.transform.position);
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
                        if (currentTarget != player.transform && currentTarget.tag != "bone")
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
                patrolAnim.SetBool("patrolRun", false);
                if (distracted)
                {
                    stateManager(5);
                }

                if (agentStopped == true)
                {
                    agentStopped = false;
                    agent.Resume();
                }
                if (alertTimer == 0 || alertTimer < 0)
                {
                    if (lastTarget != null)
                    {
                        currentTarget = lastTarget;
                        stateManager(4);
                    }
                }
                //Check if the player is within offset range from the current target
                if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
                {
                    if (timer <= 0 && (!distracted))
                    {
                        if (currentTarget != null && currentTarget.gameObject.tag != "bone")
                        {
                            lastTarget = currentTarget;
                        }
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
                break;
            case enumStates.idleSuspicious:
                {
                    //-----------------------------------------------//
                    //Stand on the spot and look at preset directions//
                    //-----------------------------------------------//
                    patrolAnim.SetBool("patrolRun", false);
                    if (agentStopped == false)
                    {
                        agentStopped = true;
                        agent.Stop();
                    }
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
                    patrolAnim.SetBool("patrolRun", false);
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
                    patrolAnim.SetBool("patrolRun", false);

                    if (soundSource && soundSource.tag != "bone")
                    {                                                  
                            if (RandomPoint(soundSource.transform.position, maxRange, out soundSourcePos))
                            {
                                randomPointSelected = true;
                                Debug.DrawRay(soundSourcePos, Vector3.up, Color.blue, 5.0f);
                                tempWaypointPos = soundSource.transform;
                                currentTarget = tempWaypointPos;//soundSourcePos;
                            }
                      
                       
                    }
                    else if (soundSource.tag == "bone")
                    {

                        currentTarget = soundSource.transform;
                    }
                    agent.SetDestination(currentTarget.transform.position);

                    //---------------------------------------------//
                    // when sound is heard, move towards the source//
                    //---------------------------------------------//    

                    //Check if the player is within offset range from the current target
                    if (vectorx >= (waypointOffsetMin) && vectorx <= (waypointOffsetMax) && vectorz >= (waypointOffsetMin) && vectorz <= (waypointOffsetMax))
                    {                       
                         Physics.Linecast(this.transform.position, soundSource.transform.position, out hit);
                        Debug.DrawLine(this.transform.position, soundSource.transform.position);
                        alertTimer = defaultAlertTimer;

                        if (hit.collider != null)
                        { 
                                if (agentStopped == false)
                                {
                                    agentStopped = true;
                                    agent.Stop();
                                }
                            if(hit.collider.tag == "bone")
                            {
                                randomPointSelected = false;
                                stateManager(7);
                            }                              
                            
                            else
                            {
                                randomPointSelected = false;                               
                                stateManager(3);
                            }
                        }
                        else
                        {
                            randomPointSelected = false;
                            stateManager(3);
                        }
                    }
                }
                break;

            case enumStates.eatBone:
                {
                    //------------------------------------------------------------------//
                    // holds the enemy still for long enough for the distraction to pass//
                    //------------------------------------------------------------------//
                    patrolAnim.SetBool("patrolRun", false);
                    if (soundSource != null && soundSource.tag == "bone")
                    {
                        bone = soundSource;
                    }                        

                        if (hit.collider.tag == "bone")
                        {
                            eatBone = true;
                            if (!bone)
                            {
                                eatBone = true;
                                alertTimer += defaultAlertTimer;
                                stateManager(3);
                                eatTimer = defaultEatTimer;
                                currentTarget = alertArea[areaCounter];
                            }

                            if (eatTimer <= 0)
                            {
                                eatTimer = defaultEatTimer;
                                distracted = false;
                                eatBone = false;

                                currentTarget = alertArea[areaCounter];
                                Destroy(bone);
                                alertTimer += defaultAlertTimer;
                                stateManager(3);
                            }
                            eatTimer--;
                            if (eatTimer < 0)
                            {
                                eatTimer = 0;
                            }
                        }
                    }
                
                break;

            case enumStates.smell:
                {
                    patrolAnim.SetBool("patrolRun", false);
                    //------------------------------------------------//
                    //turns enemy towards player's last known location//
                    //------------------------------------------------//

                    SeekForSmellSource = true;
                    agentStopped = true;
                    agent.Stop();
                    Vector3 relative = transform.InverseTransformPoint(tempSmellPosition);
                    float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                    transform.Rotate(0, angle * Time.deltaTime * 1.5f, 0);
                    if (angle < 5.0f && angle > -5.0f)
                    {
                        stateManager(1);
                    }
                }
                break;

            default:
                break;

        }
                if (currentTarget != null)
                {
                    //vector calculations used multiple time in the update
                    vectorTransformPositionx = transform.position.x;
                    vectorTransformPositionz = transform.position.z;

                    vectorCurrentTargetx = currentTarget.position.x;
                    vectorCurrentTargetz = currentTarget.position.z;

                    vectorx = (vectorTransformPositionx - vectorCurrentTargetx);
                    vectorz = (vectorTransformPositionz - vectorCurrentTargetz);
                    if (vectorz < 0)
                    {
                        vectorz *= -1;
                    }
                    if (vectorx < 0)
                    {
                        vectorx *= -1;
                    }
                }
                if (ringOfSmellScript.smellDetected == true)
                {

                    Vector3 direction = (player.transform.position - transform.position).normalized;
                    Physics.Raycast(transform.position, direction, out hit, (ringOfSmellScript.radius * 0.48f));
                    Debug.DrawRay(transform.position, direction * ringOfSmellScript.radius * 0.48f, Color.yellow);
                    if (hit.collider != null)
                    {
                        if (hit.collider.tag == player.GetComponent<Collider>().tag)
                        {
                            checkContinuousSmelling(player.transform.position);
                        }
                        else
                        {
                            turnTowardsSmellTimer = defaultTurnTowardsSmellTimer;
                        }
                    }
                    else
                    {
                        turnTowardsSmellTimer = defaultTurnTowardsSmellTimer;
                    }
                }
            //-------------//
            //End of Update//
            //-------------//
        }
    

  
    void LateUpdate()
    {
        //Just to make sure enemies will have a target
        if (timer <= 0)
        {
            timer += defaultTimer;

            if (States != enumStates.idleSuspicious)
            {               
                if (currentTarget != null && !currentTarget.Equals(null))
                {
                    agent.SetDestination(currentTarget.position);
                }
                else
                {
                    currentTarget = lastTarget;
                }
            }            
        }
        timer--;

       //To decide what player should do when he's not moving in the end of an update.
        if (agentStopped == true)
        {
            if (States == enumStates.patrol)
            {                
                agentStopped = false;
                agent.Resume(); 
            }

            else if (States == enumStates.chase || States == enumStates.alert)
            {                
                agentNotMovingTimer--;
                if (agentNotMovingTimer <= 0)
                {
                    agentStopped = false;
                    agent.Resume();
                    agentNotMovingTimer = defaultAgentNotMovingTimer;
                }
                else if (agentStopped == false)
                {
                    agentNotMovingTimer = defaultAgentNotMovingTimer;
                }
            }
            else if (States == enumStates.idle)
            {                
                agentStopped = false;
                agent.Stop();
            }           
        }

        //To make sure even if the enemies lose their target for some reason
        //they will become recover and start to move again. 
        agentNotMovingTimer--;
        if (agentNotMovingTimer < 0)
        {
            agentNotMovingTimer = 0;
        }
        if (agentNotMovingTimer == 0)
        {
            if (visited == false)
            {               
                tempPosX = vectorx;
                tempPosZ = vectorz;
                visited = true;
                agentNotMovingTimer = defaultAgentNotMovingTimer;
            }
            else
            {
                //Check if the player is within offset range from the current target
                if (tempPosX == vectorx || tempPosX >= (vectorx - waypointOffsetMin) || tempPosX <= (vectorx - waypointOffsetMax) && tempPosZ == vectorz || tempPosX >= (vectorz - waypointOffsetMin) || tempPosZ <= (vectorz - waypointOffsetMax))
                {                    
                    agent.SetDestination(currentTarget.position);
                    agentNotMovingTimer = defaultAgentNotMovingTimer;
                }                
                visited = false;
            }
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
        // Last edited by Marc
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

        if (target4 != null)
        {
            targets.Add(target4);
        }
        //end of Last edited by Marc
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

    void organizeAlertWaypoints()
    {
        for (int i = 0; i < alertArea.Count; i++)
        {          
            if (alertArea[i] != null || tempAlertWaypoints[i] != null)
            {
                tempAlertWaypoints.Add(alertArea[i]);
                waypointLocations.Add(tempAlertWaypoints[i].position);
            }
        }
        Vector3 closestWaypoint = new Vector3(Mathf.Pow((waypointLocations[0].x - soundSource.transform.position.x), 2.0f), Mathf.Pow((waypointLocations[0].y - soundSource.transform.position.y), 2.0f), Mathf.Pow((waypointLocations[0].z - soundSource.transform.position.z), 2.0f));
        closestWaypoint.x = Mathf.Sqrt(waypointLocations[0].x);
        closestWaypoint.y = Mathf.Sqrt(waypointLocations[0].y);
        closestWaypoint.z = Mathf.Sqrt(waypointLocations[0].z);

        //Choose one of the waypoints to be the closest
        closestWaypointValue = (closestWaypoint.x + closestWaypoint.y + closestWaypoint.z);

        //Run the check if it's really the closest waypoint we're looking for
        for (int i = 0; i < waypointLocations.Count; i++)
        {

            waypointLocations[i] = new Vector3(Mathf.Sqrt(Mathf.Pow(waypointLocations[i].x, 2.0f)), Mathf.Sqrt(Mathf.Pow(waypointLocations[i].y, 2.0f)), Mathf.Sqrt(Mathf.Pow(waypointLocations[i].z, 2.0f)));

            waypointLocationValue = (waypointLocations[i].x + waypointLocations[i].y + waypointLocations[i].z);

            if (waypointLocationValue < closestWaypointValue)
            {
                closestWaypoint = waypointLocations[i];
            }
        }
        //set the closest waypoint to be the first one on the list
        for (int i = 0; i < waypointLocations.Count; i++)
        {
            if (tempAlertWaypoints[i].position == closestWaypoint && !usedWaypoints.Contains(tempAlertWaypoints[i]))
            {
                alertArea.Add(tempAlertWaypoints[i]);
                currentWaypointIndex = i;
                //set waypoint into a used waypoints list to prevent the AI from using it multiple times
                usedWaypoints.Add(tempAlertWaypoints[i]);
            }
        }

        //-------------------------------------------------------//
        // search the closest waypoint from the current waypoint//
        //-----------------------------------------------------//
        for (int i = 0; i < waypointLocations.Count; i++)
        {
            // closestWaypoint = new Vector3(Mathf.Pow((waypointLocations[i].x - waypointLocations[currentWaypointIndex].x), 2.0f), Mathf.Pow((waypointLocations[0].y - waypointLocations[currentWaypointIndex].x), 2.0f), Mathf.Pow((waypointLocations[0].z - waypointLocations[currentWaypointIndex].z), 2.0f));

            //set closest waypoint to a random, not used waypoint
            for (int y = 0; y < waypointLocations.Count; y++)
            {

                if (!usedWaypoints.Contains(tempAlertWaypoints[y]))
                {

                    closestWaypoint.x = Mathf.Pow((waypointLocations[i].x - waypointLocations[currentWaypointIndex].x), 2.0f);
                    closestWaypoint.y = Mathf.Pow((waypointLocations[i].y - waypointLocations[currentWaypointIndex].y), 2.0f);
                    closestWaypoint.z = Mathf.Pow((waypointLocations[i].z - waypointLocations[currentWaypointIndex].z), 2.0f);

                    closestWaypoint.x = Mathf.Sqrt(waypointLocations[0].x);
                    closestWaypoint.y = Mathf.Sqrt(waypointLocations[0].y);
                    closestWaypoint.z = Mathf.Sqrt(waypointLocations[0].z);

                    closestWaypointValue = (closestWaypoint.x + closestWaypoint.y + closestWaypoint.z);
                    }
            }

            //Run the check if it's really the closest waypoint we're looking for
            for (int y = 0; y < waypointLocations.Count; y++)
            {

                waypointLocations[y] = new Vector3(Mathf.Sqrt(Mathf.Pow(waypointLocations[y].x, 2.0f)), Mathf.Sqrt(Mathf.Pow(waypointLocations[y].y, 2.0f)), Mathf.Sqrt(Mathf.Pow(waypointLocations[y].z, 2.0f)));

                waypointLocationValue = (waypointLocations[i].x + waypointLocations[i].y + waypointLocations[i].z);

                if (waypointLocationValue < closestWaypointValue)
                {
                    closestWaypoint = waypointLocations[i];
                }
            }

            //add the closest waypoint to the list
            for (int y = 0; y < waypointLocations.Count; y++)
            {
                if (tempAlertWaypoints[y].position == closestWaypoint && !usedWaypoints.Contains(tempAlertWaypoints[y]))
                {
                    alertArea.Add(tempAlertWaypoints[y]);
                    currentWaypointIndex = y;

                    //set waypoint into a used waypoints list to prevent the AI from using it multiple times//
                    usedWaypoints.Add(tempAlertWaypoints[y]);
                }
            }
        }
    }

    //Whenever enemy enters a new room with a different alert area, it will be given new waypoints
    //to navigate if it goes into a alert state.
    public void setAlertArea(GameObject area)
    {
        Component[] transforms;
        alertArea.Clear();
        transforms = area.GetComponentsInChildren<Transform>();

        foreach (Transform alert in transforms)
        {
            if (alert.tag == "Waypoint")
            {
                alertArea.Add(alert);
            }

        }
    }

    //Actual function to rotate enemy when the player is staying too close of the enemies
    public void rotateDogWhileSmelling( Vector3 targetTransformPosition)
    {
        if (ringOfSmellScript.smellingPlayer)
        {
            SeekForSmellSource = true;
            agentStopped = true;
            agent.Stop();
            Vector3 relative = transform.InverseTransformPoint(targetTransformPosition);
            float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;          
            transform.Rotate(0, angle * Time.deltaTime * 1.5f, 0);
        }
    }

    //if player stays within smelling range enemies will get suspicious and eventually turn to look
    //for the source of the smell.
  public  void checkContinuousSmelling(Vector3 targetTransformPosition)
    {
        turnTowardsSmellTimer--;
        if (turnTowardsSmellTimer <= 0)
        {            
            turnTowardsSmellTimer = 0;
            SeekForSmellSource = true;
            rotateDogWhileSmelling(targetTransformPosition);
        }       
    }

    //This is used to prevent enemies from being unable to find a path to the destructible object.
    //It will take a random points around the object for the enemy to navigate to.
  bool RandomPoint(Vector3 center, float range, out Vector3 result)
  {
          Vector3 randomPoint = center + Random.insideUnitSphere * range;
          NavMeshHit hit;
          if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
          {
              result = hit.position;
              return true;
          }
      result = Vector3.zero;
      return false;
  }

	void updateAnimator()
	{
		patrolAnim.SetFloat ("patrolMovement", currentSpeed);
	}
}