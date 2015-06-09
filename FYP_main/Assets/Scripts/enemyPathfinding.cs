using UnityEngine;
using System.Collections;

public class enemyPathfinding : MonoBehaviour {

	public Transform target;
	public float speed = 10;
	Vector3[] path;
	int targetIndex;

	void Start()
	{
		PathRequestManager.requestPath (transform.position, target.position, onPathFound);
	}

	void Update()
	{


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
