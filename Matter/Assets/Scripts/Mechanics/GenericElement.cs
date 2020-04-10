using UnityEngine;

/// <summary>
/// Ne contient que les variables communes à tous les éléments (détecté, activé, etc.), ainsi
/// que des méthodes abstraites. Les classes filles viendront définir elles-mêmes leurs comportements.
/// </summary>
public abstract class GenericElement : MonoBehaviour
{
	public abstract ElementType Type { get; }

	public bool Detected;
	public bool Activated;
	public Vector3 Init_pos { get; protected set; }

	public virtual void Activate ()
	{
		Activated = true;
		/// Ici implémenter tout autre comportement COMMUN à tous les éléments quand ils sont activés.
	}

	public virtual void Deactivate ()
	{
		Activated = false;
		/// Ici implémenter tout autre comportement COMMUN à tous les éléments quand ils sont désactivés.
	}

	public virtual void apply()
	{
		Activated = !Activated;
	}
}