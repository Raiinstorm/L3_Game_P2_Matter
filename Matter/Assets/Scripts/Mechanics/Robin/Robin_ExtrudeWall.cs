using UnityEngine;

public class Robin_ExtrudeWall : Robin_Extrude
{
	public override ElementType Type { get { return ElementType.Extrude; } }

	/// Ici déclarer les variables utilisées pour les extrudes wall.

	public override void Activate ()
	{
		base.Activate ();
		Debug.Log ("Robin_ExtrudeWall activé.");
		/// Ici implémenter le comportement d'une extrude wall quand elle est activée.
	}

	public override void Deactivate ()
	{
		base.Deactivate ();
		Debug.Log ("Robin_ExtrudeWall désactivé.");
		/// Ici implémenter le comportement d'une extrude wall quand elle est désactivée.
	}
}