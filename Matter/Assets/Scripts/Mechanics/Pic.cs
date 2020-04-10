using UnityEngine;

public class Pic : GenericElement
{
	public override ElementType Type { get { return ElementType.Pics; } }

	[SerializeField] float _speedPics = 15f;
	[SerializeField] float _distance;
	[SerializeField] int _damage = 12;

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
	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Character")
		{
			collision.GetComponent<Character>().GetDamage(30);
			Debug.Log("Degat sur player");
		}
	}
	public void Translate(float enable = 1.0f)
	{
		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, _init_pos.y + _distance * enable, transform.localPosition.z), Time.deltaTime * _speedPics);
	}
}