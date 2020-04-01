using UnityEngine;

public class ExtrudeWall : Extrude
{
	public override ElementType Type { get { return ElementType.Extrude; } }

	/// Ici déclarer les variables utilisées pour les extrudes wall.

	public override void Activate ()
	{
		base.Activate ();
		Debug.Log ("ExtrudeWall activé.");
		/// Ici implémenter le comportement d'une extrude wall quand elle est activée.
		transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + DistanceExtrude, init_pos.y, transform.position.z), Time.deltaTime * SpeedExtrude);
	}

	public override void Deactivate ()
	{
		base.Deactivate ();
		Debug.Log ("ExtrudeWall désactivé.");
		/// Ici implémenter le comportement d'une extrude wall quand elle est désactivée.
		transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x - DistanceExtrude, init_pos.y, transform.position.z), Time.deltaTime * SpeedExtrude);
	}
}