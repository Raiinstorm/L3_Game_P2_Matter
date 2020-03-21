using UnityEngine;

public class Robin_Glass : Robin_GenericElement
{
	public override ElementType Type { get { return ElementType.Glass; } }

	/// Ici déclarer les variables utilisées pour la glass.

	public override void Activate ()
	{
		base.Activate ();
		Debug.Log ("Robin_Glass activé.");
		/// Ici implémenter le comportement de la glass quand elle est activée.
	}

	public override void Deactivate ()
	{
		base.Deactivate ();
		Debug.Log ("Robin_Glass désactivé.");
		/// Ici implémenter le comportement de la glass quand elle est désactivée.
	}
}