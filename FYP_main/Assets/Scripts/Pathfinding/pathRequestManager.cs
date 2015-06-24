using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class pathRequestManager : MonoBehaviour 
{

	Queue<pathRequest> pathRequestQueue = new Queue<pathRequest> ();
	pathRequest currentPathRequest;

<<<<<<< HEAD
	static pathRequestManager Instance;
=======
	static pathRequestManager instance;
>>>>>>> origin/Toni_Sound&Vision
	pathfinding mainPathfinding;

	bool isProcessingPath;


	void Awake()
	{
<<<<<<< HEAD
		Instance = this;
=======
		instance = this;
>>>>>>> origin/Toni_Sound&Vision
		mainPathfinding = GetComponent<pathfinding> ();
	}

	public static void requestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
	{
		pathRequest newRequest = new pathRequest (pathStart, pathEnd, callback);
<<<<<<< HEAD
		Instance.pathRequestQueue.Enqueue (newRequest);
		Instance.tryProcessNext ();
=======
		instance.pathRequestQueue.Enqueue (newRequest);
		instance.tryProcessNext ();
>>>>>>> origin/Toni_Sound&Vision
	}

	void tryProcessNext()
	{
		if (!isProcessingPath && pathRequestQueue.Count > 0) 
		{
			currentPathRequest = pathRequestQueue.Dequeue();
			isProcessingPath = true;
			mainPathfinding.startFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
		}
	}

	public void finishedProcessingPath(Vector3[] path, bool success)
	{
		currentPathRequest.callback(path, success);
		isProcessingPath = false;
		tryProcessNext ();
	}

	struct pathRequest
	{
		public Vector3 pathStart;
		public Vector3 pathEnd;
		public Action<Vector3[], bool> callback;

<<<<<<< HEAD
		public pathRequest(Vector3 Start, Vector3 End, Action<Vector3[], bool> callBack)
		{
			pathStart = Start;
			pathEnd = End;
=======
		public pathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> callBack)
		{
			pathStart = start;
			pathEnd = end;
>>>>>>> origin/Toni_Sound&Vision
			callback = callBack;
		}
	}
}
