using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionParticle : MonoBehaviour
{
	Vector3 _position;
	public Vector3 Target;

	Transform _thisTransform;

	[SerializeField] float _positionTime;
	float _interpolator;

	private void Start()
	{
		_thisTransform = GetComponent<Transform>();
		_position = transform.position;
	}

	private void Update()
	{
		if (Target != Vector3.zero && _thisTransform.position != Target)
		{
			Lerp(Target);
		}
		else if(Target == Vector3.zero && _thisTransform.position != _position)
		{
			Lerp(_position);
		}
	}

	void Lerp(Vector3 target)
	{
		_thisTransform.position = Vector3.Lerp(_thisTransform.position, target, _interpolator);

		_interpolator += Time.deltaTime / _positionTime;

		if(_interpolator >=1)
		{
			_interpolator = 0;
		}
	}
}
