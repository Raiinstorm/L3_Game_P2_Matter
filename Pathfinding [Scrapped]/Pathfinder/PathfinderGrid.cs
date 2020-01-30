﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderGrid : MonoBehaviour
{

	public Transform StartPosition;
	public LayerMask WallMask;
	public Vector2 vGridWorldSize;//A vector2 to store the width and height of the graph in world units.
	public float fNodeRadius;//This stores how big each square on the graph will be
	public float fDistanceBetweenNodes;//The distance that the squares will spawn from eachother.

	PathfinderNode[,] NodeArray;
	public List<PathfinderNode> FinalPath;//The completed path that the red line will be drawn along


	float fNodeDiameter;
	int iGridSizeX, iGridSizeY;//Size of the Grid in Array units.


	private void Start()//Ran once the program starts
	{
		fNodeDiameter = fNodeRadius * 2;
		iGridSizeX = Mathf.RoundToInt(vGridWorldSize.x / fNodeDiameter);//Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.
		iGridSizeY = Mathf.RoundToInt(vGridWorldSize.y / fNodeDiameter);//
		CreateGrid();//Draw the grid
	}

	void CreateGrid()
	{
		NodeArray = new PathfinderNode[iGridSizeX, iGridSizeY];
		Vector3 bottomLeft = transform.position - Vector3.right * vGridWorldSize.x / 2 - Vector3.forward * vGridWorldSize.y / 2;//Get the real world position of the bottom left of the grid.
		for (int x = 0; x < iGridSizeX; x++)//Loop through the array of nodes.
		{
			for (int y = 0; y < iGridSizeY; y++)//
			{
				Vector3 worldPoint = bottomLeft + Vector3.right * (x * fNodeDiameter + fNodeRadius) + Vector3.forward * (y * fNodeDiameter + fNodeRadius);//Get the world co ordinates of the bottom left of the graph
				bool Wall = true;//Make the node a wall

				//If the node is not being obstructed
				//Quick collision check against the current node and anything in the world at its position. If it is colliding with an object with a WallMask,
				//The if statement will return false.
				if (Physics.CheckSphere(worldPoint, fNodeRadius, WallMask))
				{
					Wall = false;//Object is not a wall
				}

				NodeArray[x, y] = new PathfinderNode(Wall, worldPoint, x, y);//Create a new node in the array.
			}
		}
	}

	//Function that gets the neighboring nodes of the given node.
	public List<PathfinderNode> GetNeighboringNodes(PathfinderNode a_NeighborNode)
	{
		List<PathfinderNode> NeighborList = new List<PathfinderNode>();//Make a new list of all available neighbors.

		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				//if we are on the node tha was passed in, skip this iteration.
				if (x == 0 && y == 0)
				{
					continue;
				}

				int checkX = a_NeighborNode.iGridX + x;
				int checkY = a_NeighborNode.iGridY + y;

				//Make sure the node is within the grid.
				if (checkX >= 0 && checkX < iGridSizeX && checkY >= 0 && checkY < iGridSizeY)
				{
					NeighborList.Add(NodeArray[checkX, checkY]); //Adds to the neighbours list.
				}

			}
		}

		return NeighborList;//Return the neighbors list.
	}

	//Gets the closest node to the given world position.
	public PathfinderNode NodeFromWorldPoint(Vector3 a_vWorldPos)
	{
		float ixPos = ((a_vWorldPos.x + vGridWorldSize.x / 2) / vGridWorldSize.x);
		float iyPos = ((a_vWorldPos.z + vGridWorldSize.y / 2) / vGridWorldSize.y);

		ixPos = Mathf.Clamp01(ixPos);
		iyPos = Mathf.Clamp01(iyPos);

		int ix = Mathf.RoundToInt((iGridSizeX - 1) * ixPos);
		int iy = Mathf.RoundToInt((iGridSizeY - 1) * iyPos);

		return NodeArray[ix, iy];
	}


	//Function that draws the wireframe
	private void OnDrawGizmos()
	{

		Gizmos.DrawWireCube(transform.position, new Vector3(vGridWorldSize.x, 1, vGridWorldSize.y));//Draw a wire cube with the given dimensions from the Unity inspector

		if (NodeArray != null)//If the grid is not empty
		{
			foreach (PathfinderNode n in NodeArray)//Loop through every node in the grid
			{
				if (n.bIsWall)//If the current node is a wall node
				{
					Gizmos.color = Color.white;//Set the color of the node
				}
				else
				{
					Gizmos.color = Color.yellow;//Set the color of the node
				}


				if (FinalPath != null)//If the final path is not empty
				{
					if (FinalPath.Contains(n))//If the current node is in the final path
					{
						Gizmos.color = Color.red;//Set the color of that node
					}

				}


				Gizmos.DrawCube(n.vPosition, Vector3.one * (fNodeDiameter - fDistanceBetweenNodes));//Draw the node at the position of the node.
			}
		}
	}
}
