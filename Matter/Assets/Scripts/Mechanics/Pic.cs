using UnityEngine;

public class Pic : GenericElement
{
	public override ElementType Type { get { return ElementType.Pics; } }

	/// Ici déclarer les variables utilisées pour les pics.

	[SerializeField] protected float _speedPics = 15f;
	[SerializeField] protected float _distancePics = 10f;
	[SerializeField] int _damage = 12;
	protected Vector3 init_pos;
	void Start()
	{
		init_pos = transform.position;
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