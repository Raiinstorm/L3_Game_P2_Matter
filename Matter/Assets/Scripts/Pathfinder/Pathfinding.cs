﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

	PathfinderGrid GridReference;
	public Transform StartPosition;
	public Transform TargetPosition;

	private void Awake()
	{
		GridReference = GetComponent<PathfinderGrid>();
	}

	private void Update()
	{
		FindPath(StartPosition.position, TargetPosition.position);
	}

	void FindPath(Vector3 a_StartPos, Vector3 a_TargetPos)
	{
		PathfinderNode StartNode = GridReference.NodeFromWorldPoint(a_StartPos);
		PathfinderNode TargetNode = GridReference.NodeFromWorldPoint(a_TargetPos);

		List<PathfinderNode> OpenList = new List<PathfinderNode>();//List of nodes for the open list
		HashSet<PathfinderNode> ClosedList = new HashSet<PathfinderNode>();//Hashset of nodes for the closed list

		OpenList.Add(StartNode);//Add the starting node to the open list to begin the program

		while (OpenList.Count > 0)//Whilst there is something in the open list
		{
			PathfinderNode CurrentNode = OpenList[0];//Create a node and set it to the first item in the open list
			for (int i = 1; i < OpenList.Count; i++)//Loop through the open list starting from the second object
			{
				if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].ihCost < CurrentNode.ihCost)//If the f cost of that object is less than or equal to the f cost of the current node
				{
					CurrentNode = OpenList[i];//Set the current node to that object
				}
			}
			OpenList.Remove(CurrentNode);//Remove that from the open list
			ClosedList.Add(CurrentNode);//And add it to the closed list

			if (CurrentNode == TargetNode)//If the current node is the same as the target node
			{
				GetFinalPath(StartNode, TargetNode);//Calculate the final path
			}

			foreach (PathfinderNode NeighborNode in GridReference.GetNeighboringNodes(CurrentNode))//Loop through each neighbor of the current node
			{
				if (!NeighborNode.bIsWall || ClosedList.Contains(NeighborNode))//If the neighbor is a wall or has already been checked
				{
					continue;//Skip it
				}
				int MoveCost = CurrentNode.igCost + GetManhattenDistance(CurrentNode, NeighborNode);//Get the F cost of that neighbor

				if (MoveCost < NeighborNode.igCost || !OpenList.Contains(NeighborNode))//If the f cost is greater than the g cost or it is not in the open list
				{
					NeighborNode.igCost = MoveCost;//Set the g cost to the f cost
					NeighborNode.ihCost = GetManhattenDistance(NeighborNode, TargetNode);//Set the h cost
					NeighborNode.ParentNode = CurrentNode;//Set the parent of the node for retracing steps

					if (!OpenList.Contains(NeighborNode))//If the neighbor is not in the openlist
					{
						OpenList.Add(NeighborNode);//Add it to the list
					}
				}
			}

		}
	}



	void GetFinalPath(PathfinderNode a_StartingNode, PathfinderNode a_EndNode)
	{
		List<PathfinderNode> FinalPath = new List<PathfinderNode>();//List to hold the path sequentially 
		PathfinderNode CurrentNode = a_EndNode;//Node to store the current node being checked

		while (CurrentNode != a_StartingNode)//While loop to work through each node going through the parents to the beginning of the path
		{
			FinalPath.Add(CurrentNode);//Add that node to the final path
			CurrentNode = CurrentNode.ParentNode;//Move onto its parent node
		}

		FinalPath.Reverse();//Reverse the path to get the correct order

		GridReference.FinalPath = FinalPath;//Set the final path

	}

	int GetManhattenDistance(PathfinderNode a_nodeA, PathfinderNode a_nodeB)
	{
		int ix = Mathf.Abs(a_nodeA.iGridX - a_nodeB.iGridX);//x1-x2
		int iy = Mathf.Abs(a_nodeA.iGridY - a_nodeB.iGridY);//y1-y2

		return ix + iy;//Return the sum
	}
}
