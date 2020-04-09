using UnityEngine;

public class Extrude : GenericElement
{
	public override ElementType Type { get { return ElementType.Extrude; } }

	public float SpeedExtrude = 15f;
	public float DistanceExtrude = 2.2f;
	protected Vector3 init_pos;

	void Start()
	{
		init_pos = transform.localPosition;
	}
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

	public override void apply(float enable = 1)
	{
		throw new System.NotImplementedException();
	}
}