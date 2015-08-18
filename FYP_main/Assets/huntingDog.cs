using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum enumStatesHunter
{
    chase = 2,
    alert = 3,
    idleSuspicious = 4,
    returnToSpawner = 5
}
public class huntingDog : MonoBehaviour {

    soundSphere hunterSphereScript;
    public spawnHunter hunterSpawnScript;
    RaycastHit hit;
    public Transform currentTarget;
    public enumStatesHunter statesHunter;
    GameObject player;
    GameObject newSphere;
    public GameObject sphere;
    NavMeshAgent agent;
    public List<Transform> alertArea = new List<Transform>();
    float maxScale = 20;
    float waypointOffsetMin = -2.05f;
    float waypointOffsetMax = 2.05f;
    float vectorx;
    float vectorz;
    public float firstDirection; //= 33;
    public float secondDirection; // = 66;
    public float thirdDirection; // = 78;
    public List<float> directionDegrees = new List<float>();
    GameObject enemyObject;
    float rotationStep = 65.0f;
    public float currentAngle = 0;
    public float targetAngle = 0;
    public float angleOffsetMax = 10.0f;
    public float angleOffsetMin = -10.0f;
    bool rotationInProgress = false;
    public bool rotationCompleted = false;
    public float turnTimer = 100.0f;
    float currentTargetDirection;
    public int areaCounter = 0;
    int turnCounter = 0;
    int tempcounters = 0;
    public int defaultBarkTimer;
    public int defaultAlertTimer;
    public int defaultIdleTimer;
    public int defaultTimer;
    public float defaultTurnTimer;
    public float defaultEscapeTimer;
    float barkTimer;
    float alertTimer;
    public float escapeTimer;
    int timer;
    int idleTimer; 
    public float patrolSpeed;
    public float chaseSpeed;
    public float chaseRange;
    float vectorTransformPositionx = 0;
    float vectorTransformPositionz = 0;
    float vectorCurrentTargetx = 0;
    float vectorCurrentTargetz = 0;
    //Leap values
    public float leapRange;
    float leapTimer;
    public float defaultLeapTimer;
    public float chargeRange;
    Vector3 enemyRotation;

	// Use this for initialization
	void Start () 
    {
        player = GameObject.FindGameObjectWithTag("player");
        currentTarget = player.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = chaseSpeed;
        agent.SetDestination(currentTarget.position);

        setDirectionsForIdle();

        timer = defaultTimer;
        idleTimer = defaultIdleTimer;
        barkTimer = defaultBarkTimer;
        alertTimer = defaultAlertTimer;
        turnTimer = defaultTurnTimer;
        escapeTimer = defaultEscapeTimer;
        hunterSpawnScript = transform.GetComponentInParent<spawnHunter>();
	}
	
	// Update is called once per frame
	void Update () 
    {

        switch (statesHunter)
        {
            case enumStatesHunter.chase:
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
                        enemyRotation = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                        transform.LookAt(enemyRotation);
                        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, 5*chaseSpeed /6 * Time.deltaTime);

                        leapTimer--;
                        if (leapTimer <= 0)
                        {
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
                            hunterSphereScript = newSphere.GetComponent<soundSphere>();
                            hunterSphereScript.setMaxDiameter(maxScale);
                        }
                    }
                    barkTimer--;
                    //-----------------//
                    //Escape from chase//
                    //-----------------//

                    Physics.Linecast(transform.position, player.transform.position, out hit);
                    if (hit.collider.tag != player.GetComponent<Collider>().tag)
                    {
                        escapeTimer--;
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

                    }
                    else
                    {
                        agent.speed = chaseSpeed;
                        currentTarget = player.transform;
                        escapeTimer = defaultEscapeTimer;
                    }
                }
                break;
            case enumStatesHunter.alert:
                //------------------------------------------------------//
                //Look around a room by moving from waypoint to waypoint//
                //------------------------------------------------------//

                if (alertTimer == 0 || alertTimer < 0)
                {
                        stateManager(4);
                }
                if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
                {
                    if (timer <= 0)
                    {
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

                break;
            case enumStatesHunter.idleSuspicious:
                {
                    //-----------------------------------------------//
                    //Stand on the spot and look at preset directions//
                    //-----------------------------------------------//

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
                        //selfDestruct();
                        stateManager(5);
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
                        stateManager(3);
                    }

                    idleTimer--;

                }

                    break;
            case enumStatesHunter.returnToSpawner:
                {
                    currentTarget = transform.parent;
                    if(vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
                    {
                        selfDestruct();
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
        if (timer <= 0)
        {
            timer += defaultTimer;

            if (statesHunter != enumStatesHunter.idleSuspicious)
            {
                if (agent.SetDestination(currentTarget.position) != null)
                {
                    agent.SetDestination(currentTarget.position);
                }
            }
        }
        timer--;
	}
    public void stateManager(int value)
    {
        statesHunter = (enumStatesHunter)value;
    }
    void rotateEnemy(float targetDegrees, float rotationStep)
    {
        float rotationDifference = 0;

        if (turnTimer <= 0)
        {
            if (rotationInProgress == false)
            {
                currentAngle = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
                targetAngle = targetDegrees;//currentAngle + targetDegrees;
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
                                transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * -1);
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

                                transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * 1);
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
                                    turnTimer += defaultTurnTimer; // *Time.deltaTime;
                                }
                            }
                            else //if (currentAngle > targetAngle || targetAngle - 180 >= currentAngle)
                            {
                                transform.Rotate(Vector3.up * Time.deltaTime * rotationStep * -1);
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
                                    turnTimer += defaultTurnTimer; // *Time.deltaTime;
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
                                    turnTimer += defaultTurnTimer; // *Time.deltaTime;
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
                                    turnTimer += defaultTurnTimer; // *Time.deltaTime;
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
                                    turnTimer += defaultTurnTimer; // *Time.deltaTime;
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
    void setDirectionsForIdle()
    {
        directionDegrees.Add(firstDirection);
        directionDegrees.Add(secondDirection);
        directionDegrees.Add(thirdDirection);

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
    public void selfDestruct()
    {
        if(hunterSpawnScript)
        hunterSpawnScript.spawnedHunters--;
        Destroy(gameObject);
    }
}
