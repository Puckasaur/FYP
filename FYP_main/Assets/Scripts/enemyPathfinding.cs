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
	float waypointOffsetMin = -1.2f;
	float waypointOffsetMax = 1.2f;


	public float speed = 10;
	Vector3[] path;
	int targetIndex;
	int targetCounter = 0;



	void Start()
	{

		targets.Add (target1);
		targets.Add (target2);
		targets.Add (target3);

		currentTarget = target1;
		lastTarget = currentTarget;
		PathRequestManager.requestPath (transform.position, currentTarget.position, onPathFound);
	}

	void Update()
	{
		if(waypointOffsetMin < (transform.position.x - currentTarget.position.x) && waypointOffsetMax > (transform.position.x - currentTarget.position.x)
		   || waypointOffsetMax < (transform.position.z - currentTarget.position.z) && waypointOffsetMax > (transform.position.z - currentTarget.position.z)|| 
		  
		  
		   waypointOffsetMin < (transform.position.x - lastTarget.position.x) && waypointOffsetMax > (transform.position.x - lastTarget.position.x)
		   || waypointOffsetMax < (transform.position.z - lastTarget.position.z) && waypointOffsetMax > (transform.position.z - lastTarget.position.z) 
		   )
		{
			print("New Path requested");
			requestNewPath();
			PathRequestManager.requestPath (transform.position, currentTarget.position, onPathFound);
		}
	}

	void requestNewPath()
	{	
			if (loopWaypoints) 
			{
			if(targets[targetCounter - 1] == null)
				targets.Add(targets[targetCounter - 1]);
			}
			targetCounter++;
			lastTarget = targets [targetCounter];

			print("New path set");
			
			currentTarget = targets[targetCounter];			

			if (targets [targetCounter - 1] != null) 
			{
			targets.Remove(targets[targetCounter - 1]);
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
			else
			print("_pathSuccessful = " + _pathSuccessful);
	}

	IEnumerator followPath()
	{
		Vector3 currentWaypoint = path [0];
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
			for (int i = targetIndex; i < path.Length; i++) {
				Gizmos.color = Color.black;
				Gizmos.DrawWireCube (path [i], Vector3.one);

				if (i == targetIndex) {
					Gizmos.DrawLine (transform.position, path [i]);
				} else {
					Gizmos.DrawLine (path [i - 1], path [i]);
				}
			}
		} 
	
	}
}
