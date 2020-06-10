using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSnapping : MonoBehaviour
{
	CameraFollow _cam;

	public float RotXTarget;
	public float RotYTarget;

	[SerializeField] float _snappingTime;
	[SerializeField] float _waitingTime;
	float _lerpValueX;
	float _lerpValueY;
	float _snappingInterpolator;

	public bool EnableLerp;
	public bool ActivateLerp;
	bool _antispam;

	IEnumerator _waitForActivation;

	[HideInInspector] public bool BlockRotation;

	private void Start()
	{
		_cam = GetComponent<CameraFollow>();
		_waitForActivation = WaitForActivation();
	}

	private void Update()
	{
		if(EnableLerp && ActivateLerp)
		{
			if(BlockRotation)
			{
				_cam.BlockRotation = true;
			}

			LerpValue();
			_antispam = false;
			StopCoroutine();
		}
		else
		{
			_snappingInterpolator = 0;
			if(EnableLerp && !_antispam)
			{
				_antispam = true;
				StartCoroutine(_waitForActivation);
			}
		}
	}

	public void StopCoroutine()
	{
		StopCoroutine(_waitForActivation);
		_waitForActivation = WaitForActivation();
		_antispam = false;
	}

	void LerpValue()
	{
		//Debug.Log("test");
		_lerpValueX = Mathf.Lerp(_cam.rotX, RotXTarget, _snappingInterpolator);
		_lerpValueY = Mathf.Lerp(_cam.rotY, RotYTarget, _snappingInterpolator);

		_cam.rotX = _lerpValueX;
		_cam.rotY = _lerpValueY;

		_snappingInterpolator += Time.deltaTime / _snappingTime;

		if (_lerpValueX < RotXTarget +1 && _lerpValueX > RotXTarget-1 && _lerpValueY < RotYTarget + 1 && _lerpValueY > RotYTarget - 1)
		{
			_snappingInterpolator = 0;
			_lerpValueX = 0;
			_lerpValueY = 0;
			ActivateLerp = false;
			_cam.BlockRotation = false;
		}
	}

	IEnumerator WaitForActivation()
	{
		//Debug.Log("memek1");
		ActivateLerp = false;
		yield return new WaitForSeconds(_waitingTime);
		//Debug.Log("memek2");
		ActivateLerp = true;
	}

}
