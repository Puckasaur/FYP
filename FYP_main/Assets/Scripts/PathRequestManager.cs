using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathRequestManager : MonoBehaviour {

	Queue<pathRequest> pathRequestQueue = new Queue<pathRequest> ();
	pathRequest currentPathRequest;

	static PathRequestManager instance;
	pathfinding _pathfinding;

	bool isProcessingPath;


	void Awake()
	{
		instance = this;
		_pathfinding = GetComponent<pathfinding> ();
	}

	public static void requestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
	{
		print ("pathStart: " + pathStart + "pathEnd " + pathEnd);
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
			_pathfinding.startFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
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

		public pathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
		{
			pathStart = _start;
			pathEnd = _end;
			callback = _callback;
		}
	}
}
