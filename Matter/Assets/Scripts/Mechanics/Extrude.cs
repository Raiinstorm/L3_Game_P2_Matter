using UnityEngine;

public class Extrude : GenericElement
{
	public override ElementType Type { get { return ElementType.Extrude; } }

	[Header ("Extrude")]
	[SerializeField] private float _timeExtrude = 15f;
	[SerializeField] private float _distance = 20;
	float _extrudeInterpolator;
	bool _switchReset;

	Vector3 _oldPos;
	Vector3 _targetPos;

	bool _CoroutineAntiSpam;

	Propulsion _propulsionScript;
	public GameObject _propulsionGameObject;

	[SerializeField] MeshRenderer[] _meshRenderers;

	void Start()
	{
		_init_pos = transform.localPosition; //doit être conservée ?

		_oldPos = transform.position;

		_propulsionScript = _propulsionGameObject.GetComponent<Propulsion>();

		_propulsionScript._direction = transform.up;

		_propulsionScript.ClippingTransform.position = transform.position + transform.up*_distance;

		SoundManager.PlaySoundSpacialized("Energie", SoundManager.Sound.Energy, transform.position, 50, .25f, true);

		foreach (MeshRenderer mesh in _meshRenderers)
		{
			mesh.enabled = false;
		}
	}
	private void Update()
	{
		if (Activated && transform.position != _oldPos + transform.up * _distance)
		{
			if(!_switchReset)
			{
				ResetInterpolator();

				SoundExtrude();

				foreach (MeshRenderer mesh in _meshRenderers)
				{
					mesh.enabled = true;
				}
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
			if (_switchReset)
			{
				ResetInterpolator();
				SoundExtrude(true);
			}

			_CoroutineAntiSpam = false;
			Translate(0);
		}

		if(_propulsionScript.IsPropulsing && Activated && transform.position == _oldPos + transform.up * _distance)
		{
			Activated = false;
			_propulsionScript.IsPropulsing = false;
		}
	}
	public void Translate(float enable = 1.0f)
	{
		_targetPos = _oldPos + transform.up * (_distance * enable);

		transform.position = Vector3.Lerp(transform.position, _targetPos, _extrudeInterpolator);

		_extrudeInterpolator += Time.deltaTime / _timeExtrude;
	}

	void ResetInterpolator()
	{
		_extrudeInterpolator = 0f;
		_switchReset = !_switchReset;
	}

	void SoundExtrude(bool end = false)
	{
		if(!end)
		{
			SoundManager.PlaySoundSpacialized("Extrude", SoundManager.Sound.Extrude, transform.position);
		}
		else
		{
			SoundManager.PlaySoundSpacialized("ExtrudeEnd", SoundManager.Sound.ExtrudeEnd, transform.position);
		}

	}


}