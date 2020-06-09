using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaking : MonoBehaviour
{
	CameraFollow _cam;

	public bool Shaking;

	bool _first = true;

	[SerializeField] float _intensity;

	[SerializeField] AnimationCurve _intensityCurve;
	[SerializeField] float _shakingTime;
	float _curveIntensity;
	float _lerpValue;
	float _shakeInterpolator;


	float _saveIntensity;

	private void Start()
	{
		_saveIntensity = _intensity;
		_cam = GetComponent<CameraFollow>();
	}

	private void Update()
	{
		if(Shaking)
		{
			_cam.BlockRotation = true;
			LerpValue();
			CameraShake();
		}
		else
		{
			_cam.BlockRotation = false;
			_first = true;
			_intensity = _saveIntensity;
		}
	}

	void CameraShake()
	{
		_cam.rotX += (_intensity) * _curveIntensity;
		_cam.rotY -= (_intensity) * _curveIntensity;

		_intensity = -_intensity;
		if (_first)
		{
			_first = false;
			_intensity *= 2;
		}
	}

	void LerpValue()
	{
		_lerpValue = Mathf.Lerp(0, 1, _shakeInterpolator);
		_curveIntensity = _intensityCurve.Evaluate(_lerpValue);

		_shakeInterpolator += Time.deltaTime / _shakingTime;

		if (_curveIntensity <= .01f && _lerpValue > .9f)
		{
			Shaking = false;
			_curveIntensity = 0;
			_lerpValue = 0;
			_shakeInterpolator = 0;
		}
	}
}
