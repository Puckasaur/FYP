﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class enemyPathfinding : MonoBehaviour {

	public Transform target1;
	public Transform target2;
	public Transform target3;
	public Transform currentTarget;
	public Transform lastTarget;
    //public GameObject player;


	List<Transform> targets = new List<Transform>();
	public bool loopWaypoints;
    public bool patrol = true;
    public bool lookForSound = false;
    public bool chasePlayer = false;
    //public float speed = 1.0f;
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
	int timer = 60;

	float vectorTransformPositionx = 0;
	float vectorTransformPositionz = 0;

	float vectorCurrentTargetx = 0;
	float vectorCurrentTargetz = 0;

	float vectorx;
	float vectorz;

	void Start()
	{


		targets.Add (target1);
		targets.Add (target2);
		targets.Add (target3);


		currentTarget = targets[0];
		lastTarget = currentTarget;

		PathRequestManager.requestPath (transform.position, currentTarget.position, onPathFound);

	}

	void Update()
    {
        if(patrol)
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

            if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
            {
                if (timer <= 0)
                {

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
        else
        {
            StopCoroutine("followPath");
        }
        if(lookForSound)
        {
            GameObject brokenObject = GameObject.FindGameObjectWithTag("Broken Object");
            Vector3 dir = (brokenObject.transform.localPosition) - (this.transform.localPosition);
            if (brokenObject.tag == "Broken Object")
            {
                
                transform.localRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);
                this.GetComponent<Rigidbody>().AddForce(dir * speed);
            }
            if (dir.x <= 2 && dir.x>= -2 && dir.z<= 2 && dir.z >= -2)
                m_patrol();
        }
        if(chasePlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 Player_direction = (player.transform.localPosition) - (this.transform.localPosition);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Player_direction), turnSpeed * Time.deltaTime);
            Debug.Log("Found You!");
            this.GetComponent<Rigidbody>().AddForce(Player_direction * speed);
            float disx = Player_direction.x;
            float disz = Player_direction.z;
            Debug.Log("X:"+ disx+ "Z:"+ disz);
            if(((Player_direction.x >= 10) || Player_direction.x <= -10 || Player_direction.z >= 10 || Player_direction.z<= -10))
            {
                escapeTimer += Time.deltaTime;
                if (escapeTimer > 5)
                {
                    PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
                    m_patrol();
                }
                   
            }
                
        }
    }
       
    void m_patrol()
    {
        patrol = true;
        lookForSound = false;
        chasePlayer = false;
    }
    void m_lookForSound()
    {
        patrol = false;
        lookForSound = true;
        chasePlayer = false;

    }
    void m_chasePlayer()
    {
        escapeTimer = 0;
        patrol = false;
        lookForSound = false;
        chasePlayer = true;

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
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentTarget.position-transform.position ), turnSpeed * Time.deltaTime);//this.transform.position - currentTarget.transform.position;
			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
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
