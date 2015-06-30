using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class grid : MonoBehaviour {
	
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;

	node[,] mainGrid;

	public bool displaymainGridGizmos;
	float nodeDiameter;
	int mainGridSizeX, mainGridSizeY;

	public int maxSize
	{
		get
		{

			return mainGridSizeX * mainGridSizeY;
		}
	}

	public List<node> getNeighbours(node node)
	{
		List<node> neighbours = new List<node> ();


		for (int x = -1; x<= 1; x++) 
		{
			for (int y = -1; y<= 1; y++)
			{
				if (x == 0 && y == 0)
				{
					continue;
				}

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;


				if(checkX >= 0 && checkX < mainGridSizeX && checkY >= 0 && checkY < mainGridSizeY)
				{
					neighbours.Add(mainGrid[checkX, checkY]);
				}
			}
		}

		return neighbours;

	}
	
	void Awake() 
	{
		nodeDiameter = nodeRadius*2;

		mainGridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		mainGridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		createmainGrid();
	}
	
	void createmainGrid() 
	{
		mainGrid = new node[mainGridSizeX,mainGridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward* gridWorldSize.y/2;
		
		for (int x = 0; x < mainGridSizeX; x ++) 
		{
			for (int y = 0; y < mainGridSizeY; y ++) 
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
				mainGrid[x,y] = new node(walkable,worldPoint,x,y);

			}
		}
	}






	public node nodeFromWorldPoint(Vector3 worldPosition) 

	{
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);
		

		int x = Mathf.RoundToInt((mainGridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((mainGridSizeY-1) * percentY);

		return mainGrid[x,y];
	}



	void OnDrawGizmos() 
	{
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));


				if (mainGrid != null && displaymainGridGizmos) 
			{
				foreach (node n in mainGrid) 

				{
					Gizmos.color = (n.walkable)?Color.white:Color.red;
					Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
				}
			}
	}
	

}