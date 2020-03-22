using UnityEngine;

public class Robin_ExtrudeGround : Robin_Extrude
{
	public override ElementType Type { get { return ElementType.Extrude; } }

	/// Ici déclarer les variables utilisées pour les extrudes ground.

	public override void Activate()
	{
		base.Activate();
		Debug.Log("Robin_ExtrudeGround activé.");
		/// Ici implémenter le comportement d'une extrude ground quand elle est activée.

		transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, init_pos.y + DistanceExtrude, transform.position.z), Time.deltaTime * SpeedExtrude);
	}

	public override void Deactivate ()
	{
		base.Deactivate ();
		Debug.Log ("Robin_ExtrudeGround désactivé.");
		/// Ici implémenter le comportement d'une extrude ground quand elle est désactivée.

		transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, init_pos.y - DistanceExtrude, transform.position.z), Time.deltaTime * SpeedExtrude);
	}
}