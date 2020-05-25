using UnityEngine;

public class Extrude : GenericElement
{
	public override ElementType Type { get { return ElementType.Extrude; } }

	[Header ("Extrude")]
	[SerializeField] private float _timeExtrude = 15f;
	[SerializeField] private float _distance = 20;
	float _extrudeInterpolator;
	bool _switchResetInterpolator;

	Vector3 _oldPos;
	Vector3 _targetPos;

	bool _CoroutineAntiSpam;

	Propulsion _propulsionScript;
	public GameObject _propulsionGameObject;

	void Start()
	{
		_init_pos = transform.localPosition; //doit être conservée ?

		_oldPos = transform.position;

		_propulsionScript = _propulsionGameObject.GetComponent<Propulsion>();

		_propulsionScript._direction = transform.up;
		_propulsionScript.ClippingTransform.position = transform.position + transform.up*_distance;
	}
	private void Update()
	{
		if (Activated && transform.position != _oldPos + transform.up * _distance)
		{
			if(!_switchResetInterpolator)
			{
				ResetInterpolator();
			}

			if(!_CoroutineAntiSpam)
			{
				_CoroutineAntiSpam = true;
				StartCoroutine(_propulsionScript.PropulsionCooldown());
			}
			Translate();
		}

		if (!Activated && transform.position != _oldPos)
		{
			if (_switchResetInterpolator)
			{
				ResetInterpolator();
			}

			_CoroutineAntiSpam = false;
			Translate(0);
		}

	}
	public void Translate(float enable = 1.0f)
	{
		_targetPos = _oldPos + transform.up * (_distance * enable);

		transform.position = Vector3.Lerp(transform.position, _targetPos, _extrudeInterpolator);

		_extrudeInterpolator += Time.deltaTime / _timeExtrude;

		/*Vector3 target = new Vector3(transform.localPosition.x, _init_pos.y + _distance * enable, transform.localPosition.z);
		transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * _speedExtrude);*/
	}

	void ResetInterpolator()
	{
		_extrudeInterpolator = 0f;
		_switchResetInterpolator = !_switchResetInterpolator;
	}
}