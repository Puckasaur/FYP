using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class grid : MonoBehaviour {
	
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	node[,] mainGrid;

	public bool displayGridGizmos;
	float nodeDiameter;
	int gridSizeX, gridSizeY;

	public int maxSize
	{
		get
		{
			return gridSizeX * gridSizeY;
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

				if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
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
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		createGrid();
	}
	
	void createGrid() 
	{
		mainGrid = new node[gridSizeX,gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward* 	gridWorldSize.y/2;
		
		for (int x = 0; x < gridSizeX; x ++) 
		{
			for (int y = 0; y < gridSizeY; y ++) 
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool Walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
				mainGrid[x,y] = new node(Walkable,worldPoint,x,y);
			}
		}
	}





	public node nodeFromWorldPoint(Vector3 worldPosition) 
	{
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);
		
		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
		return mainGrid[x,y];
	}



	void OnDrawGizmos() 
	{
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));

				if (mainGrid != null && displayGridGizmos) 
			{
				foreach (node n in mainGrid) 
				{
					Gizmos.color = (n.walkable)?Color.white:Color.red;
					Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
				}
			}
	}
	

}