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
    ringOfSmell ringOfSmellScript;
    coneOfVision coneOfVisionScript;
    soundSphere sphereScript;
    RaycastHit hit;
    public Vector3 respawnPosition;
    public Transform target1;
    public Transform target2;
    public Transform target3;
    public Transform target4;
    public GameObject visionRotator;
    public Transform currentTarget;
    public Transform lastTarget;
    public enumStates States;
    GameObject vision;
    GameObject smell;
    GameObject bone;
    public GameObject player;
    GameObject newSphere;
    public GameObject sphere;
    public GameObject soundSource;
    GameObject brokenObject;

    public bool isOnWaypoint = false;
    public NavMeshAgent agent;
    public List<Transform> targets = new List<Transform>();
    public List<Transform> alertArea = new List<Transform>();

    public bool isPaired = false;
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
    public bool SeekForSmellSource = false;
    public bool agentStopped = false;

    //So many timers
    int tempcounters = 0;
    int timer;
    public int idleTimer;
    int barkTimer;
    float escapeTimer;
    public float alertTimer;
    public float eatTimer;
    float failTimer;
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

    //Stuck thingies
    float tempPosX = 0;
    float tempPosZ = 0;
    bool visited = false;
    //end of stuck thingies
    List<Transform> usedWaypoints = new List<Transform>();
    List<Vector3> waypointLocations = new List<Vector3>();
    List<Transform> tempAlertWaypoints = new List<Transform>();
    float closestWaypointValue = 0;
    int currentWaypointIndex = 0;
    float waypointLocationValue = 0;
    Vector3 waypointDifference;
    //for smart rotation
    public Vector3 tempSmellPosition;
    public bool continueRotation = false;

    Collider playerCollider;
    Animator patrolAnim;

    //These variables are for the enemies to use when they smell a bone
    float maxRange = 1.5f;
    Vector3 soundSourcePos;
    Transform tempWaypointPos;


    //This is for Animator to see enemies actual speeds, it uses normal update atm.
    //It can be changed to FixedUpdate if it gives better results
    Vector3 previousPosition;
    Vector3 currentPosition;
    public float currentSpeed;


    //values if enemy doesn't receive a new waypoint to prevent them from being stuck	
    void Start()
    {
        
        leapTimer = defaultLeapTimer;
        visionRotator = GameObject.FindGameObjectWithTag("visionRotator");
        respawnPosition = this.transform.position;
        player = GameObject.FindGameObjectWithTag("player");
        ringOfSmellScript = player.GetComponentInChildren<ringOfSmell>();
        coneOfVisionScript = GetComponentInChildren<coneOfVision>();
        patrolAnim = gameObject.GetComponent<Animator>();
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
        //patrolAnim = GetComponentInChildren<Animator>();
    }

    void Update()
    {

        if (agent.velocity != Vector3.zero)
        {
            Vector3 currentMove = transform.position - previousPosition;
            currentSpeed = currentMove.magnitude / Time.deltaTime;
            previousPosition = transform.position;
        }


        GetComponent<Rigidbody>().WakeUp();
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

                    if (agentStopped == true && isPaired)
                    {
                        agentStopped = false;
                        agent.Resume();
                    }
                    
                    //patrolAnim.SetBool("patrolIdle", false);

                    //agentStopped = false;
                    //agent.Resume();
                    //patrolAnim.SetBool("patrolWalk", true);
                    //patrolAnim.SetBool("patrolRun", false);


                    if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
                    { 
                        stateManager(1);
						
						if (agentStopped == false)
                        {
                            agentStopped = true;
                            agent.Stop();
                            patrolAnim.SetBool("patrolWalk", false);
                            patrolAnim.SetBool("patrolRun", false);
                            //patrolAnim.SetBool("patrolIdle", true);
                        }
                    }
                    //if (!SeekForSmellSource)
                    //{
                    //    transform.rotation = new Quaternion(0, 0, 0, 0);
                    //}
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

                    if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
                    {

                        if (idleTimer <= 0)
                        {
                            if (currentTarget.gameObject.tag != "bone")
                            {
                                lastTarget = currentTarget;
                                //patrolAnim.SetBool("patrolWalk", false);
                                //patrolAnim.SetBool("patrolRun", false);
                                //patrolAnim.SetBool("patrolIdle", true);
                            }                            
                            if( isPaired == true)
                            {
                                idleTimer = defaultIdleTimer;
                            isOnWaypoint = true;
                            }
                            //if (isPaired == true && idleTimer <= 0)
                            //{ stateManager(4); }
                            //if (agent.SetDestination(currentTarget.position) != null)
                            //{

                            //    // agent.SetDestination(currentTarget.position);
                            //}

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
                            //if (agentStopped == true)
                            //{
                            //    agentStopped = false;
                            //    agent.Resume();
                            //}
                            //stateManager(0);
                        }
                        idleTimer--;
                        if (idleTimer <= 0)
                        {
                            idleTimer = 0;
                        }
                    }
                    else
                    {
                        patrolAnim.SetBool("patrolWalk", false);
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
                    //--------------------------//
                    //Leap Attack While Chasing //
                    //--------------------------//

                    patrolAnim.SetBool("patrolWalk", false);
                    patrolAnim.SetBool("patrolRun", true);
                    //patrolAnim.SetBool("patrolIdle", false);


                    if (vectorx < chargeRange || vectorz < chargeRange)
                    {
                        agent.autoBraking = false;
                        enemyRotation = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                        transform.LookAt(enemyRotation);
                        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, chaseSpeed / 2 * Time.deltaTime);

                        leapTimer--;
                        if (leapTimer <= 0)
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
                if (distracted)
                {
                    stateManager(5);
                }
                
                if (agentStopped == true)
                {
                    agentStopped = false;
                    agent.Resume();
                }
                //if (ringOfSmellScript.smellDetected == false)
               // {
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
                //}
                //else 
                //{
                //    alertTimer--;
                //    if (alertTimer <= 0)
                //    {
                //        alertTimer = 0;
                //    }
                //}

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
                   // if (ringOfSmellScript.smellDetected == false)
                   // {
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
                    //}

                    //else if (ringOfSmellScript.smellDetected == true)
                    //{
                    //    RotateDogWhileSmelling();
                    //}
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
                     
                    // tempWaypointPos.position = currentTarget.position;
                    print("sound squid > " + soundSource);
                    if (soundSource && soundSource.tag != "bone")
                    {
                        if (RandomPoint(soundSource.transform.position, maxRange, out soundSourcePos))
                        {
                            Debug.DrawRay(soundSourcePos, Vector3.up, Color.blue, 5.0f);
                            tempWaypointPos = soundSource.transform;
                            currentTarget = tempWaypointPos;//soundSourcePos;
                        }
                    }
                    else if (soundSource.tag == "bone")
                    {
                        print("This is the wonderland it should have always been");
                        currentTarget = soundSource.transform;                        
                    }
                    agent.SetDestination(currentTarget.transform.position);
                    print("Set destination to " + currentTarget.transform.position);

                    //---------------------------------------------//
                    // when sound is heard, move towards the source//
                    //---------------------------------------------//

                    

                    if (vectorx >= (waypointOffsetMin * 2) && vectorx <= (waypointOffsetMax * 2) && vectorz >= (waypointOffsetMin * 2) && vectorz <= (waypointOffsetMax * 2))
                    {
                        alertTimer = defaultAlertTimer;
                        stateManager(7);
                        //currentTarget.position = tempWaypointPos;
                    }
                    //organizeAlertWaypoints();
                    //stateManager(3);



                }
                break;
            case enumStates.eatBone:
                {
                    //------------------------------------------------------------------//
                    // holds the enemy still for long enough for the distraction to pass//
                    //------------------------------------------------------------------//
                    if (soundSource != null && soundSource.tag == "bone")
                    {
                        //print("bone set");
                        bone = soundSource;
                    }

                    eatBone = true;
                    if (!bone)
                    {
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
                        print(bone + " trying to destroy boen");
                        alertTimer += defaultAlertTimer;
                        stateManager(3);

                    }
                    eatTimer--;

                }

                break;
            case enumStates.smell:
                {
                    SeekForSmellSource = true;
                    agentStopped = true;
                    agent.Stop();
                    Vector3 relative = transform.InverseTransformPoint(tempSmellPosition);//player.transform.position);
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

            //RotateDogWhileSmelling();
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
                if (/*agent.SetDestination(currentTarget.position)*/ currentTarget != null && !currentTarget.Equals(null))
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

        if (target4 != null)
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

    void organizeAlertWaypoints()
    {
        //tempAlertWaypoints.Clear();
        //foreach (Transform alert in alertArea)
        //{
        //    tempAlertWaypoints.Add(alert);
        //    waypointLocations.Add(alert.position);
        //}
        for (int i = 0; i < alertArea.Count; i++)
        {
            //if (tempAlertWaypoints[i] != null)
            //{

            //}
            if (alertArea[i] != null || tempAlertWaypoints[i] != null)
            {
                tempAlertWaypoints.Add(alertArea[i]);
                waypointLocations.Add(tempAlertWaypoints[i].position);
            }
            else
            {
            }

        }

        //if (soundSource)
        //{
        Vector3 closestWaypoint = new Vector3(Mathf.Pow((waypointLocations[0].x - soundSource.transform.position.x), 2.0f), Mathf.Pow((waypointLocations[0].y - soundSource.transform.position.y), 2.0f), Mathf.Pow((waypointLocations[0].z - soundSource.transform.position.z), 2.0f));
        //}
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

  bool RandomPoint(Vector3 center, float range, out Vector3 result)
  {
      for (int i = 0; i < 6; i++)
      {
          Vector3 randomPoint = center + Random.insideUnitSphere * range;
          NavMeshHit hit;
          if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
          {
              result = hit.position;
              return true;
          }
      }
      result = Vector3.zero;
      return false;
  }
}