using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicWall : Pic
{
	public override void Activate()
	{
		base.Activate();
		Debug.Log("Pic activé.");
		/// Ici implémenter le comportement d'un pic quand il est activé.

		transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + _distancePics, init_pos.y, transform.position.z), Time.deltaTime * _speedPics);
	}

	public override void Deactivate()
	{
		base.Deactivate();
		Debug.Log("Pic désactivé.");
		/// Ici implémenter le comportement d'un pic quand il est désactivé.

		transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x - _distancePics, init_pos.y, transform.position.z), Time.deltaTime * _speedPics);
	}
}
