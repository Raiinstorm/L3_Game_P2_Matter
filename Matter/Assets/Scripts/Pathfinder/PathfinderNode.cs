﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderNode
{

	public int iGridX;
	public int iGridY;

	public bool bIsWall;
	public Vector3 vPosition;

	public PathfinderNode ParentNode;//For the AStar algoritm, will store what node it previously came from so it cn trace the shortest path.

	public int igCost;//The cost of moving to the next square.
	public int ihCost;//The distance to the goal from this node.

	public int FCost { get { return igCost + ihCost; } }//Quick get function to add G cost and H Cost, and since we'll never need to edit FCost, we dont need a set function.

	public PathfinderNode(bool a_bIsWall, Vector3 a_vPos, int a_igridX, int a_igridY)//Constructor
	{
		bIsWall = a_bIsWall;//Tells the program if this node is being obstructed.
		vPosition = a_vPos;//The world position of the node.
		iGridX = a_igridX;//X Position in the Node Array
		iGridY = a_igridY;//Y Position in the Node Array
	}

}
