using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class pathfinding : MonoBehaviour 
{

	PathRequestManager prm;
	Grid grid;

	void Awake()
	{
		prm = GetComponent<PathRequestManager>();
		grid = GetComponent<Grid>();
	}

	public void startFindPath(Vector3 startPos, Vector3 targetPos)
	{
		StartCoroutine (findPath (startPos, targetPos));
	}

	IEnumerator findPath(Vector3 startPos, Vector3 targetPos)
	{
		Stopwatch sw = new Stopwatch ();
		sw.Start ();

		Vector3[] waypoints = new Vector3[0];
		bool pathSuccessful = false;

		Node startNode = grid.NodeFromWorldPoint (startPos);
		Node targetNode = grid.NodeFromWorldPoint (targetPos);

		if (targetNode.walkable) {
		

				Heap<Node> openSet = new Heap<Node> (grid.maxSize);
				HashSet<Node> closedSet = new HashSet<Node> ();
				openSet.Add (startNode);
			
			
				while (openSet.count > 0) {
					Node currentNode = openSet.removeFirst ();
				
					closedSet.Add (currentNode);
				
					if (currentNode == targetNode) {
						sw.Stop ();
						//print ("path found in: " + sw.ElapsedMilliseconds + " ms");
						pathSuccessful = true;				
						break;
					}
				
					foreach (Node neighbour in grid.getNeighbours(currentNode)) {
						if (!neighbour.walkable || closedSet.Contains (neighbour)) {
							continue;
						}
					
						int newMovementCostToNeighbour = currentNode.gCost + getDistance (currentNode, neighbour);
					
						if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains (neighbour)) {
							neighbour.gCost = newMovementCostToNeighbour;
							neighbour.hCost = getDistance (neighbour, targetNode);
							neighbour.parent = currentNode;
						
							if (!openSet.Contains (neighbour)) {
								openSet.Add (neighbour);
							} else {
								openSet.updateItem (neighbour);
							}
						}
					}
				}
			}

			yield return null;
			if (pathSuccessful) 
			{
				waypoints = retracePath (startNode, targetNode);
			}
			prm.finishedProcessingPath (waypoints, pathSuccessful);
		
	}
	Vector3[] retracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node> ();
		Node currentNode = endNode;

		while (currentNode != startNode) 
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Add (startNode);
		Vector3[] waypoints = simplifyPath (path);
		Array.Reverse (waypoints);
		return waypoints;

	
	}

	Vector3[] simplifyPath(List<Node> path)
	{
		List<Vector3> waypoints = new List<Vector3> ();
		Vector2 directionOld = Vector2.zero;

		for (int i = 0; i < path.Count - 1; i++) 
		{
			Vector2 directionNew = new Vector2(path[i + 1].gridX - path[i].gridX, path[i + 1].gridY - path[i].gridY);
			if(directionNew != directionOld)
			{
				waypoints.Add (path[i].worldPosition);
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}

	int getDistance(Node nodeA, Node nodeB)
	{
		int distanceX = Mathf.Abs (nodeA.gridX - nodeB.gridX);
		int distanceY = Mathf.Abs (nodeA.gridY - nodeB.gridY);

		if (distanceX > distanceY) 
		{
			return 14 * distanceY + 10 * (distanceX - distanceY);
		} 

		else 
		{
			return 14* distanceY + 10 * (distanceY - distanceX);
		}
	}

}
