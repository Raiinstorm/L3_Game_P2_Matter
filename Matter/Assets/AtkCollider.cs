using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkCollider : MonoBehaviour
{
	public Collider player;

	private void OnTriggerEnter(Collider other)
	{
		if(other == player)
		{
			Debug.LogError("PlayerPerdUnPV");
			//link player.hp with collider
		}
	}
}
