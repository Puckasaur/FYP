using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class pathRequestManager : MonoBehaviour 
{

	Queue<pathRequest> pathRequestQueue = new Queue<pathRequest> ();
	pathRequest currentPathRequest;


	static pathRequestManager instance;
	pathfinding mainPathfinding;

	bool isProcessingPath;


	void Awake()
	{

		instance = this;
		mainPathfinding = GetComponent<pathfinding> ();
	}

	public static void requestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
	{
		pathRequest newRequest = new pathRequest (pathStart, pathEnd, callback);

		instance.pathRequestQueue.Enqueue (newRequest);
		instance.tryProcessNext ();

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


		public pathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> callBack)
		{
			pathStart = start;
			pathEnd = end;

			callback = callBack;
		}
	}
}