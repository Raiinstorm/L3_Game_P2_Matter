﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Extrude : GenericElement
{
	public override ElementType Type { get { return ElementType.Extrude; } }
	public ZoneController _zoneController;
	[SerializeField] AnimDissolve _animDissolve;
	Transform _zoneControllerTransform;

	[Header ("Extrude")]
	[SerializeField] private float _timeExtrude = 15f;
	[SerializeField] private float _distance = 20;
	[SerializeField] Transform _directionTransform;
	float _extrudeInterpolator;
	bool _switchReset;

	Vector3 _oldPos;
	Vector3 _targetPos;

	bool _CoroutineAntiSpam;

	Propulsion _propulsionScript;
	public GameObject _propulsionGameObject;

	Vector3 _direction;

	[SerializeField] bool _minusDistance;


	[Header("Tempo")]
	[SerializeField] bool _disableExtrudeSwitch;
	[SerializeField] float _disableCooldownTime = 5;
	IEnumerator _disableCooldown;

	[Header ("AutoSwitch - Ne pas cumuler avec le disableExtrudeSwitch")]
	public bool AutoSwitch;
	public bool SecondSwitching;

	[Header("Is BigExtrude")]
	[SerializeField] bool _isBigExtrude;


	void Start()
	{
		//_init_pos = transform.localPosition; //doit être conservée ?

		if(_directionTransform != null)
		{
			_direction = _directionTransform.up;
		}
		else
		{
			_direction = transform.up;
		}

		if(_minusDistance)
		{
			transform.position -= _direction * _distance;
		}

		if(_propulsionGameObject == null)
		{
			_propulsionGameObject = new GameObject("generatedPropulsionGameObject");
			_propulsionGameObject.AddComponent<Propulsion>();
			_propulsionGameObject.transform.position = transform.position;
			_propulsionGameObject.transform.parent = GameMaster.i.TrashCan.transform;
		}

		if (_zoneController != null)
		{
			_zoneControllerTransform = _zoneController.transform;
			_oldPos = _zoneControllerTransform.position;
			GameMaster.i.Faults.Add(_zoneController);
		}
		else
			_oldPos = transform.position;
		_propulsionScript = _propulsionGameObject.GetComponent<Propulsion>();
		_propulsionScript._direction = _direction;
		_propulsionScript.ClippingTransform.position = _oldPos + _direction*_distance;

		if(!_isBigExtrude || _zoneController !=null)
		SoundManager.PlaySoundSpacialized("Energie", SoundManager.Sound.Energy, _oldPos, 50, .25f, true,false,true,_zoneControllerTransform);

		if(AutoSwitch)
		{
			GameMaster.i.AutoSwitchExtrudes.Add(this);
		}

		_disableCooldown = DisableCooldown();
	}
	private void Update()
	{

		if(_zoneControllerTransform != null)
		_oldPos = _zoneControllerTransform.position;

		//Debug.Log(_oldPos);


		if (Activated && transform.position != _oldPos + _direction * _distance)
		{
			if(!_switchReset)
			{
				ResetInterpolator();
				SoundExtrude();

				if(_animDissolve != null)
				{
					_animDissolve.Activate();
				}

				if(_disableExtrudeSwitch)
				{
					StopCoroutine(_disableCooldown);
					_disableCooldown = DisableCooldown();
					StartCoroutine(_disableCooldown);
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

				if (_animDissolve != null)
				{
					_animDissolve.Desactivate();
				}
			}

			_CoroutineAntiSpam = false;
			Translate(0);
		}

		if(_propulsionScript.IsPropulsing && Activated && transform.position == _oldPos +_direction * _distance)
		{
			_propulsionScript.IsPropulsing = false;

			if(AutoSwitch)
			{
				GameMaster.i.Activate();
			}
			else
			{
				_zoneController.Cancel();
			}
		}


	}
	public void Translate(float enable = 1.0f)
	{
		_targetPos = _oldPos +_direction * (_distance * enable);

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

	IEnumerator DisableCooldown()
	{
		yield return new WaitForSeconds(_disableCooldownTime);
		_zoneController.Cancel();

	}

}