using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTarget : MonoBehaviour
{
	public Transform player;
	public void LookingTarget()
	{
		transform.LookAt(player);
	}
}
