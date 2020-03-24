using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingSpawn : MonoBehaviour
{
	private void Awake()
	{
		RestartBlocking.spawns.Add(transform.position);
	}
}
