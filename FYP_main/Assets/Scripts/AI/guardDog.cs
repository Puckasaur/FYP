using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class guardDog : MonoBehaviour
{

    soundSphere sphereScript;
    RaycastHit hit;


    public Transform target1;
    public Transform target2; 
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

    NavMeshAgent Fortyseven;
    List<Transform> targets = new List<Transform>();
    public List<Transform> alertArea = new List<Transform>();

    bool hasWaypointsLeft;
    public bool eatBone = false;
    public bool distracted = false;

    public float turnSpeed = 2.0f;

    public float speed = 10;

    float maxScale = 60;
    float waypointOffsetMin = -1.0f;
    float waypointOffsetMax = 1.0f;
    //float vectorTransformPositionx = 0;
    //float vectorTransformPositionz = 0;
    //float vectorCurrentTargetx = 0;
    //float vectorCurrentTargetz = 0;
    float vectorx;
    float vectorz;


    //Idle Suspicious values
    public bool idleSuscpicious = false;
    public float firstDirection; //= 33;
    public float secondDirection; // = 66;
    public float thirdDirection; // = 78;
    List<float> directionDegrees = new List<float>();
    GameObject enemyObject;

    //bool rotating = false;
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
    //So many timers
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
    public int turnCounter = 0;

    Vector3[] path = new Vector3[0];
    Vector3 currentWaypoint;
    Vector3 bonedir;

    //values if enemy doesn't receive a new waypoint to prevent them from being stuck
    Vector3 worldPositionNow;
    Vector3 worldPositionPast;
    //int checkIfStuck = 100;
    //bool isStuck = false;

    void Start()
    {

        setTargetWaypoints();
        player = GameObject.FindGameObjectWithTag("player");
        setDirectionsForIdle();
        timer = defaultTimer;
        idleTimer = defaultIdleTimer;
        barkTimer = defaultBarkTimer;
        alertTimer = defaultAlertTimer;
        currentTarget = targets[0];
        lastTarget = currentTarget;
        Fortyseven = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
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
                    if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
                    {

                        stateManager(1);

                    }

                }

                break;

            case enumStates.idle:
                {
                    NavMesh.
                    NavMesh area = NavMesh.GetAreaFromName;
                    {
                        NavMeshPath = invalid
                    }
                    //--------------------------------------------------------//
                    // idle, look around, without moving towards any waypoints//
                    //--------------------------------------------------------//

                    if (idleTimer <= 0)
                    {
                        lastTarget = currentTarget;
                        currentTarget = targets[targetCounter];

                        Fortyseven.SetDestination(currentTarget.position);
                        idleTimer = defaultIdleTimer;
                        targetCounter++;
                        if (targetCounter > 1)
                        {
                            targetCounter = 0;
                        }
                        stateManager(0);
                    }
                    idleTimer--;
                    break;
                }
            case enumStates.alert:
                //------------------------------------------------------//
                //Look around a room by moving from waypoint to waypoint//
                //------------------------------------------------------//
                {
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

                    if (alertTimer <= 0)
                    {

                        if (turnCounter != 0)
                        {
                            turnCounter = 0;
                        }
                        if (idleTimer != 50)
                        {
                            idleTimer = 50;
                        }
                        currentTarget = lastTarget;
                        stateManager(4);
                    }

                    alertTimer--;
                    if (alertTimer <= 0)
                    {
                        alertTimer = 0;
                    }

                    break;
                }
            case enumStates.idleSuspicious:
                {
                    //-----------------------------------------------//
                    //Stand on the spot and look at preset directions//
                    //-----------------------------------------------//


                    if (turnCounter < 3)
                    {

                        currentTargetDirection = directionDegrees[0];
                        print(currentTargetDirection + " << currentTargetDirection");




                        if (rotationCompleted)
                        {

                            directionDegrees.Add(directionDegrees[0]);
                            directionDegrees.Remove(directionDegrees[0]);
                            rotationCompleted = false;
                            print("turnTimer  " + turnTimer);

                            turnCounter++;
                            turnTimer += 100;
                            print("currentTargetDirection >> " + currentTargetDirection);

                        }

                    }


                    else if (turnCounter > 2)
                    {

                        if (idleTimer <= 0)
                        {


                            if (currentTarget != null)
                            {
                                Fortyseven.SetDestination(currentTarget.position);
                            }
                            stateManager(1);
                            print("state manager: patrol!");
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


                    if (!bone)
                    {
                        vision.SetActive(true);
                        smell.SetActive(true);
                        alertTimer += defaultAlertTimer;
                        stateManager(3);
                        currentTarget = alertArea[areaCounter];
                    }
                    distracted = true;
                    if (currentTarget != null)
                    {
                        bonedir = (currentTarget.transform.localPosition) - (this.transform.localPosition);
                    }
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
                    GameObject brokenObject = GameObject.FindGameObjectWithTag("Broken Object");
                    bone = GameObject.FindGameObjectWithTag("bone");
                    if (brokenObject)
                    {
                        Vector3 objectdir = (brokenObject.transform.localPosition) - (this.transform.localPosition);
                        if (objectdir.x <= 2 && objectdir.x >= -2 && objectdir.z <= 2 && objectdir.z >= -2 || !brokenObject)
                        {
                            //stateManager(0);
                            //currentTarget = lastTarget;
                            currentTarget = alertArea[areaCounter];
                            areaCounter++;
                            if (areaCounter > 2)
                            {
                                areaCounter = 0;
                            }
                            stateManager(3);

                        }
                        else
                        {
                            currentTarget = brokenObject.transform;


                        }
                    }
                    else if (bone)
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
                        alertTimer += defaultAlertTimer;
                        stateManager(3);
                        currentTarget = alertArea[areaCounter];
                    }

                    else if (eatTimer <= 0)
                    {
                        eatTimer = defaultEatTimer;// 120;
                        distracted = false;
                        vision.SetActive(true);
                        smell.SetActive(true);
                        eatBone = false;

                        currentTarget = alertArea[areaCounter];
                        areaCounter++;
                        if (areaCounter > 2)
                        {
                            areaCounter = 0;
                        }
                        stateManager(3);
                        Destroy(bone);


                    }
                    eatTimer--;

                }

                break;
            default:
                break;
        }

        //if (timer <= 0 && currentTarget != null)
        //{
        //    timer += defaultTimer;
        //    Fortyseven.SetDestination(currentTarget.position);

        //}
        //timer--;
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

    }
void setTargetWaypoints()
    {

        targets.Add(target1);
        targets.Add(target2);

    }
}
