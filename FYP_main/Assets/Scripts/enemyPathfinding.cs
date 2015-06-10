using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class enemyPathfinding : MonoBehaviour {

	public Transform target1;
	public Transform target2;
	public Transform target3;
	public Transform currentTarget;
	public Transform lastTarget;


	List<Transform> targets = new List<Transform>();
	public bool loopWaypoints;
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

		vectorx = (vectorTransformPositionx -vectorCurrentTargetx);
		vectorz = (vectorTransformPositionz - vectorCurrentTargetz);

		if(vectorx >= waypointOffsetMin && vectorx <= waypointOffsetMax && vectorz >= waypointOffsetMin && vectorz <= waypointOffsetMax)
		{
			if (timer <= 0) 
		{

			currentTarget = targets[targetCounter];

			PathRequestManager.requestPath (transform.position, currentTarget.position, onPathFound);
				
				timer += 60;
			targetCounter++;
			if(targetCounter > 2)
			{
				targetCounter = 0;
			}
		}
			timer--;
		}
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
