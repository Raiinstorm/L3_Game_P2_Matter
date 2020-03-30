using UnityEngine;

public class Pic : GenericElement
{
	public override ElementType Type { get { return ElementType.Pics; } }

	/// Ici déclarer les variables utilisées pour les pics.

	[SerializeField] float _speedPics = 15f;
	[SerializeField] float _distancePics = 10f;
	[SerializeField] int _damage = 12;
	protected Vector3 init_pos;
	void Start()
	{
		init_pos = transform.position;
	}

	public override void Activate ()
	{
		base.Activate ();
		Debug.Log ("Pic activé.");
		/// Ici implémenter le comportement d'un pic quand il est activé.
		
		transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, init_pos.y + _distancePics, transform.position.z), Time.deltaTime * _speedPics);
	}

	public override void Deactivate ()
	{
		base.Deactivate ();
		Debug.Log ("Pic désactivé.");
		/// Ici implémenter le comportement d'un pic quand il est désactivé.
		
		transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, init_pos.y - _distancePics, transform.position.z), Time.deltaTime * _speedPics);
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Character")
		{
			collision.GetComponent<Character>().GetDamage(30);
			Debug.Log("Degat sur player");
		}
	}
}