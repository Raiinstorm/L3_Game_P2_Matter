using System;
using UnityEngine;

public class Pic : GenericElement
{
	public override ElementType Type { get { return ElementType.Pics; } }

	[SerializeField] protected float _speedPics = 15f;
	[SerializeField] int _damage = 12;
	//public Vector3 Init_pos { get; private set; }
	[SerializeField] private float _distance;

	void Start()
	{
		Init_pos = transform.localPosition;
	}

	private void Update()
	{
		Debug.Log("MaPositionDeBase" + Init_pos.y);
		Debug.Log("DistancePic" + _distance);

		if (Activated && transform.localPosition.y != Init_pos.y + _distance)
			Translate();

		if (!Activated && transform.localPosition.y != Init_pos.y)
			Translate(0);

	}
	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Character")
		{
			collision.GetComponent<Character>().GetDamage(30);
			Debug.Log("Degat sur player");
		}
	}
	/*
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
	*/

	public void Translate(float enable = 1.0f)
	{
		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, Init_pos.y + _distance * enable, transform.localPosition.z), Time.deltaTime * _speedPics);
	}
}