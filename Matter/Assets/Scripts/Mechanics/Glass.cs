using UnityEngine;

public class Glass : GenericElement
{
	public override ElementType Type { get { return ElementType.Glass; } }

	/// Ici déclarer les variables utilisées pour la glass.

	public override void Activate ()
	{
		base.Activate ();
		Debug.Log ("Glass activé.");
		/// Ici implémenter le comportement de la glass quand elle est activée.
	}

	public override void apply(float enable = 1)
	{
		throw new System.NotImplementedException();
	}

	public override void Deactivate ()
	{
		base.Deactivate ();
		Debug.Log ("Glass désactivé.");
		/// Ici implémenter le comportement de la glass quand elle est désactivée.
	}
}