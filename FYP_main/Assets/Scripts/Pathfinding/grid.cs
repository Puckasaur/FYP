using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class grid : MonoBehaviour {
	
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
<<<<<<< HEAD
	Node[,] mainGrid;

	public bool displayGridGizmos;
	float nodeDiameter;
	int gridSizeX, gridSizeY;
=======
	node[,] mainGrid;

	public bool displaymainGridGizmos;
	float nodeDiameter;
	int mainGridSizeX, mainGridSizeY;
>>>>>>> origin/Toni_Sound&Vision

	public int maxSize
	{
		get
		{
<<<<<<< HEAD
			return gridSizeX * gridSizeY;
		}
	}

	public List<Node> getNeighbours(Node node)
	{
		List<Node> neighbours = new List<Node> ();
=======
			return mainGridSizeX * mainGridSizeY;
		}
	}

	public List<node> getNeighbours(node node)
	{
		List<node> neighbours = new List<node> ();
>>>>>>> origin/Toni_Sound&Vision

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

<<<<<<< HEAD
				if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
				{
                    neighbours.Add(mainGrid[checkX, checkY]);
=======
				if(checkX >= 0 && checkX < mainGridSizeX && checkY >= 0 && checkY < mainGridSizeY)
				{
					neighbours.Add(mainGrid[checkX, checkY]);
>>>>>>> origin/Toni_Sound&Vision
				}
			}
		}

		return neighbours;

	}
	
	void Awake() 
	{
		nodeDiameter = nodeRadius*2;
<<<<<<< HEAD
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		createGrid();
	}
	
	void createGrid() 
	{
		mainGrid = new Node[gridSizeX,gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward* 	gridWorldSize.y/2;
		
		for (int x = 0; x < gridSizeX; x ++) 
		{
			for (int y = 0; y < gridSizeY; y ++) 
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool Walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
				mainGrid[x,y] = new Node(Walkable,worldPoint,x,y);
=======
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
>>>>>>> origin/Toni_Sound&Vision
			}
		}
	}





<<<<<<< HEAD
	public Node NodeFromWorldPoint(Vector3 worldPosition) 
=======
	public node nodeFromWorldPoint(Vector3 worldPosition) 
>>>>>>> origin/Toni_Sound&Vision
	{
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);
		
<<<<<<< HEAD
		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
=======
		int x = Mathf.RoundToInt((mainGridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((mainGridSizeY-1) * percentY);
>>>>>>> origin/Toni_Sound&Vision
		return mainGrid[x,y];
	}



	void OnDrawGizmos() 
	{
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));

<<<<<<< HEAD
				if (mainGrid != null && displayGridGizmos) 
			{
				foreach (Node n in mainGrid) 
=======
				if (mainGrid != null && displaymainGridGizmos) 
			{
				foreach (node n in mainGrid) 
>>>>>>> origin/Toni_Sound&Vision
				{
					Gizmos.color = (n.walkable)?Color.white:Color.red;
					Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
				}
			}
	}
	

}