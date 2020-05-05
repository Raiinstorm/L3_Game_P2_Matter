using UnityEngine;

public class Extrude : GenericElement
{
	public override ElementType Type { get { return ElementType.Extrude; } }

	[SerializeField] private float _speedExtrude = 15f;
	[SerializeField] private float _distance = 20;
	protected Vector3 init_pos;

	void Start()
	{
		_init_pos = transform.localPosition;
	}
	private void Update()
	{
		if (Activated && transform.localPosition.y != _init_pos.y + _distance)
			Translate();

		if (!Activated && transform.localPosition.y != _init_pos.y)
			Translate(0);
	}
	public void Translate(float enable = 1.0f)
	{
		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, _init_pos.y + _distance * enable, transform.localPosition.z), Time.deltaTime * _speedExtrude);
	}
}