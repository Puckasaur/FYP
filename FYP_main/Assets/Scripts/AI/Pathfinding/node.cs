using UnityEngine;
using System.Collections;


public class node : IHeapItem<node>
{
	
	public bool walkable;
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;

	public int gCost;
	public int hCost;

	public int mainHeapIndex;

	public node parent;

	public node(bool mainWalkable, Vector3 mainWorldPos, int mainGridX, int mainGridY)
	{
		walkable = mainWalkable;
		worldPosition = mainWorldPos;
		gridX = mainGridX;
		gridY = mainGridY;

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

			return mainHeapIndex;
		}
		set
		{
			mainHeapIndex  = value;
		}
	}

	public int CompareTo(node nodeToCompare)

	{
		int compare = fCost.CompareTo (nodeToCompare.fCost);

		if (compare == 0) 
		{
			compare = hCost.CompareTo(nodeToCompare.hCost);
		}
		return -compare;
	}
}
