using UnityEngine;
using System.Collections;

<<<<<<< HEAD
public class Node : IHeapItem<Node>
=======
public class node : IHeapItem<node>
>>>>>>> origin/Toni_Sound&Vision
{
	
	public bool walkable;
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;

	public int gCost;
	public int hCost;
<<<<<<< HEAD
	public int _heapIndex;

	public Node parent;

	public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
	{
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
=======
	public int mainHeapIndex;

	public node parent;

	public node(bool mainWalkable, Vector3 mainWorldPos, int mainGridX, int mainGridY)
	{
		walkable = mainWalkable;
		worldPosition = mainWorldPos;
		gridX = mainGridX;
		gridY = mainGridY;
>>>>>>> origin/Toni_Sound&Vision

	}
	public int fCost
	{
		get
		{
			return gCost + hCost;
		}
	}

	public int heapIndex
	{
		get
		{
<<<<<<< HEAD
			return _heapIndex;
		}
		set
		{
			_heapIndex  = value;
		}
	}

	public int CompareTo(Node nodeToCompare)
=======
			return mainHeapIndex;
		}
		set
		{
			mainHeapIndex  = value;
		}
	}

	public int CompareTo(node nodeToCompare)
>>>>>>> origin/Toni_Sound&Vision
	{
		int compare = fCost.CompareTo (nodeToCompare.fCost);

		if (compare == 0) 
		{
			compare = hCost.CompareTo(nodeToCompare.hCost);
		}
		return -compare;
	}
}
