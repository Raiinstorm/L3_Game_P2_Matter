﻿using UnityEngine;

public class Robin_Pic : Robin_GenericElement
{
	public override ElementType Type { get { return ElementType.Pics; } }
	
	/// Ici déclarer les variables utilisées pour les pics.

	public override void Activate ()
	{
		base.Activate ();
		Debug.Log ("Robin_Pic activé.");
		/// Ici implémenter le comportement d'un pic quand il est activé.
	}

	public override void Deactivate ()
	{
		base.Deactivate ();
		Debug.Log ("Robin_Pic désactivé.");
		/// Ici implémenter le comportement d'un pic quand il est désactivé.
	}
}