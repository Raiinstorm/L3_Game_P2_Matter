using System;
using UnityEngine;

public class Pic : GenericElement
{
	public override ElementType Type { get { return ElementType.Pics; } }

	[SerializeField] protected float _speedPics = 15f;
	[SerializeField] protected float _distancePics = 10f;
	[SerializeField] int _damage = 12;
	protected Vector3 init_pos;
	void Start()
	{
		init_pos = transform.localPosition;
	}
	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Character")
		{
			collision.GetComponent<Character>().GetDamage(30);
			Debug.Log("Degat sur player");
		}
	}
	public override void Activate()
	{
		base.Activate();
		Debug.Log("Pic activé.");
		/// Ici implémenter le comportement d'un pic quand il est activé.

		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, init_pos.y + _distancePics, transform.localPosition.z), Time.deltaTime);
	}
	public override void Deactivate()
	{
		base.Deactivate();
		Debug.Log("Pic désactivé.");
		/// Ici implémenter le comportement d'un pic quand il est désactivé.

		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, init_pos.y, transform.localPosition.z), Time.deltaTime);
	}

	 

	public override void apply(float enable = 1.0f)
	{
		transformPos(_distancePics * enable);
	}

	private void transformPos(float distancePics)
	{
		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, init_pos.y + _distancePics, transform.localPosition.z), Time.deltaTime);
	}
}