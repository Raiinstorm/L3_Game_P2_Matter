using UnityEngine;

public class Robin_Pic : Robin_GenericElement
{
	public override ElementType Type { get { return ElementType.Pics; } }

	/// Ici déclarer les variables utilisées pour les pics.

	public float SpeedPics = 15f;
	public float DistancePics = 10f;
	protected Vector3 init_pos;
	void Start()
	{
		init_pos = transform.position;
	}

	public override void Activate ()
	{
		base.Activate ();
		Debug.Log ("Robin_Pic activé.");
		/// Ici implémenter le comportement d'un pic quand il est activé.
		
		transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, init_pos.y + DistancePics, transform.position.z), Time.deltaTime * SpeedPics);
	}

	public override void Deactivate ()
	{
		base.Deactivate ();
		Debug.Log ("Robin_Pic désactivé.");
		/// Ici implémenter le comportement d'un pic quand il est désactivé.
		
		transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, init_pos.y - DistancePics, transform.position.z), Time.deltaTime * SpeedPics);
	}
}