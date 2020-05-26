using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
	[SerializeField] Transform _respawn;

	bool _passed;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Character" && !_passed)
		{
			_passed = true;
			PlayerControllerV3 playerScript = other.gameObject.GetComponent<PlayerControllerV3>();
			playerScript.RespawnPosition = _respawn.position;
		}
	}
}
