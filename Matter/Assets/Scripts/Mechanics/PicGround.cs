using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicGround : Pic
{
	public override ElementType Type { get { return ElementType.Pics; } }
	/// Ici déclarer les variables utilisées pour les extrudes wall.
	public override void Activate()
	{
		base.Activate();
		Debug.Log("Pic activé.");
		/// Ici implémenter le comportement d'un pic quand il est activé.

		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, init_pos.y + _distancePics*10, transform.localPosition.z), Time.deltaTime * _speedPics);
	}

	public override void Deactivate()
	{
		base.Deactivate();
		Debug.Log("Pic désactivé.");
		/// Ici implémenter le comportement d'un pic quand il est désactivé.

		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, init_pos.y - _distancePics*10, transform.localPosition.z), Time.deltaTime * _speedPics);
	}
}
