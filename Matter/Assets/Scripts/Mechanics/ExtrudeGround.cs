using UnityEngine;

public class ExtrudeGround : Extrude
{
	public override ElementType Type { get { return ElementType.Extrude; } }

	/// Ici déclarer les variables utilisées pour les extrudes ground.

	public override void Activate()
	{
		base.Activate();
		Debug.Log("ExtrudeGround activé.");
		/// Ici implémenter le comportement d'une extrude ground quand elle est activée.

		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, init_pos.y + DistanceExtrude*10, transform.localPosition.z), Time.deltaTime * SpeedExtrude);
	}

	public override void Deactivate ()
	{
		base.Deactivate ();
		Debug.Log ("ExtrudeGround désactivé.");
		/// Ici implémenter le comportement d'une extrude ground quand elle est désactivée.

		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, init_pos.y - DistanceExtrude*10, transform.localPosition.z), Time.deltaTime * SpeedExtrude);

	}
}