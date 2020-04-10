using UnityEngine;

public class Extrude : GenericElement
{
	public override ElementType Type { get { return ElementType.Extrude; } }

	[SerializeField] private float _speedExtrude = 15f;
	[SerializeField] private float _distance = 20;
	protected Vector3 init_pos;

	void Start()
	{
		Init_pos = transform.localPosition;
	}
	private void Update()
	{
		if (Activated && transform.localPosition.y != Init_pos.y + _distance)
			Translate();

		if (!Activated && transform.localPosition.y != Init_pos.y)
			Translate(0);
	}

	/*
	public override void Activate()
	{
		base.Activate();
		Debug.Log("ExtrudeGround activé.");
		/// Ici implémenter le comportement d'une extrude ground quand elle est activée.

		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, init_pos.y + DistanceExtrude * 10, transform.localPosition.z), Time.deltaTime * SpeedExtrude);
	}

	public override void Deactivate()
	{
		base.Deactivate();
		Debug.Log("ExtrudeGround désactivé.");
		/// Ici implémenter le comportement d'une extrude ground quand elle est désactivée.

		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, init_pos.y - DistanceExtrude * 10, transform.localPosition.z), Time.deltaTime * SpeedExtrude);
	}
	*/
	public void Translate(float enable = 1.0f)
	{
		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, Init_pos.y + _distance * enable, transform.localPosition.z), Time.deltaTime * _speedExtrude);
	}
}