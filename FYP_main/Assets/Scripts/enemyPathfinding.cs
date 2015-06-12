using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class enemyPathfinding : MonoBehaviour {

	//Waypoints for pathfinding
	public Transform target1;
	public Transform target2;
	public Transform target3;
	public Transform currentTarget;
	public Transform lastTarget;
	//public GameObject player;
	
	
	List<Transform> targets = new List<Transform>();
	
	//Pathfinding values
	Vector3[] path = new Vector3[0];
	Vector3 currentWaypoint;
	int targetIndex;
	int targetCounter = 0;
	int timer = 60;
	public bool loopWaypoints;
	
	//waypoint offset
	float waypointOffsetMin = -1.0f;
	float waypointOffsetMax = 1.0f;
	
	//values for different states
	public bool patrol = true;
	public bool lookForSound = false;
	public bool chasePlayer = false;
	public bool idle = false;
	
	//Movement speed values
	public float turnSpeed = 2.0f;
	public float escapeTimer = 0;
	public float speed = 10;
	public float maxSpeed = 20;
	
	//values for general movement
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
        // if(patrol)
        //{ 
        if(speed >4)
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
        vectorz = (vectorTransformPositionz - vectorCurrentTargetz -5);

        if (patrol)
        {
            Debug.Log("X: "+vectorx +" Z: " +vectorz);
            if (vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
            {
                Debug.Log("Yohoo");
                if (timer <= 0)
                {
                    lastTarget = currentTarget;
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

        if (lookForSound)
        {
            // Move to the broken object
            GameObject brokenObject = GameObject.FindGameObjectWithTag("Broken Object");
            Vector3 dir = (brokenObject.transform.localPosition) - (this.transform.localPosition);
            if (brokenObject.tag == "Broken Object")
            {

                transform.localRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);
                this.GetComponent<Rigidbody>().AddForce(dir * speed);
            }
            // stop looking after reaching the object
            if (dir.x <= 2 && dir.x >= -2 && dir.z <= 2 && dir.z >= -2)
                m_patrol();
        }
        if (chasePlayer)
        {
            // Move Enemy

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Debug.Log(player.transform.localPosition);


            currentTarget = player.transform;
            PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);

            // Escape from chase

            Vector3 Player_direction = (player.transform.localPosition) - (this.transform.localPosition);
            if (((Player_direction.x >= 10) || Player_direction.x <= -10 || Player_direction.z >= 10 || Player_direction.z <= -10))
            {
                escapeTimer += Time.deltaTime;
                if (escapeTimer > 5)
                {
                    currentTarget = lastTarget;
                    PathRequestManager.requestPath(transform.position, currentTarget.position, onPathFound);
                    m_patrol();
                }

            }

        }
    }
    //-------------//
    //State Manager//
    //-------------//
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
