using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerV3 : CharacterV3
{
	#region Variables
	[HideInInspector] public Vector3 _velocity;
	Vector3 _rotation;
	float _deadZone = 0.2f;
	float _difAngle;
	protected IEnumerator _jumpBuffer;
	[HideInInspector] public bool CanStillJump;
	[HideInInspector] public bool _canMove;

	bool _gameIsPaused;
	Vector3 _velocityMemory;
	Vector3 _rbMemory;
	bool _antispamJumpImpulse;
	Vector3 _jumpImpulse;
	float _currentJumpImpulse;
	float _refJumpImpulse;

	[Header("Velocity")]
	[SerializeField] [Range(0, 1f)] float _smoothDamping;
	float _smoothInput;
	float _refSmoothInput;
	float _smoothInputTarget;
	public float MoveSpeed;
	[SerializeField] [Range(1, 3)] float _runMultiplicator;
	float _moveSpeedRun;
	bool _nerfVelocity;
	[SerializeField] float _extraTimeToJump;
	[SerializeField] [Range(0, .5f)] float _turnAroundSpeed;
	float _powerInput;
	[SerializeField] float _fallMultiplicator;
	[SerializeField] [Range(0,1)] float _jumpImpulseIntensity;

	[Header("")]
	[SerializeField] int _damageInfuseEnergy;
	[SerializeField] PhysicMaterial _physicMaterial;


	[HideInInspector] public bool IsJumping;

	[Header("Slope")]
	[SerializeField] float _maxSlopeAngle;
	[SerializeField] float _maxSpeedOnSlope;
	[SerializeField] float _speedIncreasingOnSlope;
	bool _onSlope;
	Vector3 _onSlopeDirection;
	Vector3 _onSlopeVelocity;
	float _onSlopeSpeed;
	[SerializeField] SlopeDetector _slopeDetector;


	[Header("Propulsion")]
	public Vector3 PropulsionVector;

	[SerializeField] AnimationCurve _propulsionCurve;
	[SerializeField] float _propulsionTime;
	[SerializeField] float _propulsionPower;
	public float _propulsionIntensity;
	float _lerpValue;
	float _propulsionInterpolator;

	[HideInInspector] public bool _propulsed;
	[HideInInspector] public bool AntiSpamPropulsion;
	float _targetRotationPropulsion;

	[SerializeField] float _rotationPropulsionTime;
	bool _rotationPropulsion;
	float _rotationPropulsionInterpolator;
	float _rotX;
	float _rotY;
	float _cameraRotX;
	float _cameraRotY;

	public float InputX { get; private set; }
	public float InputZ { get; private set; }
	public float CamInputX { get; private set; }
	public float CamInputZ { get; private set; }
	public bool FastRun { get; private set; }

	[Header("Camera")]
	[SerializeField] Camera _mainCamera;
	[SerializeField] CameraFollow _cameraFollow;

	public bool _dead;
	bool _canRespawn;

	Vector3 _cameraForward;      // vector forward "normalisé" de la cam
	Vector3 _cameraRight;        // vector right "normalisé" de la cam
	Vector3 _cameraUp;

	[SerializeField] bool _testSounds;

	#endregion

	private void Start()
	{
		GameMaster.i.Hello();
		GameMaster.i.PlaySounds();

		_canRespawn = true;
		RespawnPosition = transform.position;
		_maxHealth = 100;
		_health = 100;
		FastRun = false;
		_rb = GetComponent<Rigidbody>();
		_jumpBuffer = JumpBuffer();
		_canMove = true;
		_moveSpeedRun = MoveSpeed * _runMultiplicator;
		_thisTransform = GetComponent<Transform>();
		_physicMaterial = GetComponent<Collider>().material;
	}
	private void FixedUpdate()
	{
		if (_canMove)
		{
			ApplyVelocity();
			MoveInput();
		}
		else
		{
			PauseState();
		}
	}
	private void Update()
	{

		InputX = Input.GetAxis("Horizontal");
		InputZ = Input.GetAxis("Vertical");
		CamInputX = Input.GetAxis("RightStickHorizontal");
		CamInputZ = Input.GetAxis("RightStickVertical");

		if (Input.GetButtonDown("Run") && IsGround())
		{
			IsRunning = true;
		}

		if(_propulsed)
		{
			Propulsion();
		}
		else
		{
			AntiSpamPropulsion = false;
		}

		if(_dead)
		{
			Death();
		}

		if(IsJumping)
		{
			JumpingImpulse();
		}
		else
		{
			_antispamJumpImpulse = false;
			_currentJumpImpulse = 0;
		}
		if(!_onLand && IsGround() && !IsJumping)
		{
			_onLand = true;
			LandingSound();
		}

		if(_testSounds)
		{
			_testSounds = false;
			StartCoroutine(SoundMenu.i.TestSoundMenu());
		}

	}
	void ApplyVelocity()
	{
		if (_gameIsPaused)
		{
			_gameIsPaused = false;
			_rb.velocity = _rbMemory;
			_velocity = _velocityMemory;
			_rb.useGravity = true;
		}

		float y = 0;
		if (!IsGround())
		{
			_onLand = false;

			if (!CanStillJump)
			{
				StartCoroutine(_jumpBuffer);
			}
		}
		else
		{
			_jumpBuffer = JumpBuffer();
		}

		if(_onSlope)
		{
			SlopeVelocity();
		}
		else
		{
			_onSlopeSpeed = 0;
			_onSlopeVelocity = Vector3.zero;
		}

		y = _rb.velocity.y;


		if(y <0) //la ligne de code magique
		{
			y += Vector3.up.y * Physics.gravity.y * (_fallMultiplicator - 1) * Time.deltaTime;
		}

		if (_nerfVelocity && y > 0 && !IsJumping && !_propulsed)
		{
			_rb.velocity = new Vector3(_velocity.x, y/2 , _velocity.z);
		}
		else
		{

			if(_propulsed)
			{
				_onSlopeVelocity = Vector3.zero;
			}

			//Debug.Log("velocity :" + _velocity.z);

			_rb.velocity = new Vector3(_velocity.x , y, _velocity.z) + (PropulsionVector*_propulsionIntensity) + (_onSlopeVelocity) + (_jumpImpulse * _currentJumpImpulse);

			//Debug.Log("velocity : " + new Vector3(_velocity.x, y, _velocity.z) + " | Propulsion : " + (PropulsionVector * _propulsionIntensity));

			if(PropulsionVector.y >0)
			{
				PropulsionVector.y -= _propulsionIntensity;

				if(PropulsionVector.y<0)
				{
					PropulsionVector.y = 0;
				}
			}
		}

		FrictionMaterial();
	}

	#region Input
	void MoveInput()
	{
		Vector2 stickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		if (stickInput.magnitude < _deadZone)
		{
			stickInput = Vector2.zero;
			_smoothInputTarget = 0;
			IsRunning = false;
		}
		else
		{
			CheckSlope();

			_difAngle = SignedAngle(transform.forward, new Vector3(_rotation.x, 0f, _rotation.z), Vector3.up);

			//Debug.Log("difangle : " +_difAngle + " | rotation : " + transform.rotation.eulerAngles.y);
			if (Mathf.Abs(_difAngle)>5)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y + _difAngle, 0), _turnAroundSpeed);
			}
		}

		Vector2 stickInputR = new Vector2(CamInputX, CamInputZ);
		if (stickInputR.magnitude < _deadZone)
			stickInputR = Vector2.zero;

		CameraSetting();

		_velocity = (_cameraRight.normalized * stickInput.x) + (_cameraForward.normalized * stickInput.y); // get orientation
		_rotation = _velocity;

		_powerInput = PowerInput(_velocity);

		if (!_isJumpingOnSpot)
		{
			_smoothInput = Mathf.SmoothDamp(_smoothInput, _smoothInputTarget, ref _refSmoothInput, _smoothDamping); //augmentation progressive de la vitesse

			_velocity = _velocity.normalized * MoveSpeed * _smoothInput * _powerInput;

			if (_velocity.magnitude > MoveSpeed && !IsRunning)
			{
				_velocity -= _velocity.normalized * (_velocity.magnitude - MoveSpeed);
			}
			if (IsRunning && _velocity.magnitude > _moveSpeedRun)
			{
				_velocity -= _velocity.normalized * (_velocity.magnitude - _moveSpeedRun);
			}
		}
		else
		{
			_velocity = Vector3.zero;
		}
	}

	float PowerInput(Vector3 velocity)
	{
		float stickPower = Mathf.Max(Mathf.Abs(velocity.x),Mathf.Abs(velocity.z));

		if(IsRunning || stickPower > 1)
		{
			return 1;
		}
		else
		{
			//Debug.Log("StickPower : " + stickPower);
			return stickPower;
		}
	}

	float SmoothInput()
	{
		if(_nerfVelocity)
		{
			return 0;
		}
		if (IsRunning)
		{
			return _runMultiplicator;
		}
		else
		{
			return 1;
		}
	}

	void FrictionMaterial()
	{
		_physicMaterial.staticFriction = _rb.velocity.magnitude < .5f ? 1 : 0;
		//Debug.Log(_physicMaterial.staticFriction);
	}

	void JumpingImpulse()
	{
		if (!_antispamJumpImpulse)
		{
			_antispamJumpImpulse = true;
			_jumpImpulse = new Vector3(_velocity.x, 0, _velocity.z);
			_currentJumpImpulse = _jumpImpulseIntensity;
		}

		_currentJumpImpulse = Mathf.SmoothDamp(_currentJumpImpulse, 0, ref _refJumpImpulse, .5f);
		//Debug.Log("jumpImpulse :" + _jumpImpulse + " | intensity :" + _currentJumpImpulse);

	}

	#endregion

	#region Propulsion

	void Propulsion()
	{

		if (!AntiSpamPropulsion)
		{
			AntiSpamPropulsion = true;

			if (_rb.velocity.y < 0)
			{
				_rb.velocity = Vector3.zero;

			}
			else
			{
				_rb.velocity = new Vector3(0, _rb.velocity.y, 0);
			}

			SoundManager.PlaySound(SoundManager.Sound.PlayerPropulsed);

			CheckRotationPropulsion();
		}


		if (_rotationPropulsion)
		{
			RotationPropulsionLerp();
		}


		_lerpValue = Mathf.Lerp(0, 1, _propulsionInterpolator);
		_propulsionIntensity = _propulsionCurve.Evaluate(_lerpValue) * _propulsionPower;

		_propulsionInterpolator += Time.deltaTime / _propulsionTime;

		if (_propulsionIntensity <= .01f)
		{
			ResetPropulsion();
		}
	}

	public void ResetPropulsion()
	{
		_propulsed = false;
		_propulsionIntensity = 0;
		_lerpValue = 0;
		_propulsionInterpolator = 0;
	}

	void CheckRotationPropulsion()
	{
		_difAngle = SignedAngle(transform.forward, new Vector3(PropulsionVector.x, 0, PropulsionVector.z), Vector3.up);

		//Debug.Log(PropulsionVector);
		//Debug.Log(transform.rotation.eulerAngles.y);

		if ((_difAngle <= -75 || _difAngle >= 75) && PropulsionVector.y < .90f)
		{
			float target = transform.rotation.eulerAngles.y + _difAngle;

			if(target > -5 && target < 5)
			{
				target = 0;
			}

			if(target > 360)
			{
				target -= 360;
			}
			if(target <0)
			{
				target += 360;
			}

			_targetRotationPropulsion = target;

			//Debug.Log("cam : " + _cameraFollow.rotY + " | target : " + target +  " | dif : " + (_cameraFollow.rotY - target) + " | dif + 360 : " + (_cameraFollow.rotY - (target + 360)));

			if((_cameraFollow.rotY - target) < 45 && (_cameraFollow.rotY - target) > -45 || (_cameraFollow.rotY - (target + 360)) < 45 && (_cameraFollow.rotY - (target+360)) >-45) { }
			else
			{
				_cameraFollow.rotY = transform.rotation.eulerAngles.y;
			}

			if(_cameraFollow.rotY >180 && target == 0)
			{
				_targetRotationPropulsion = 360;
			}
			else if(_cameraFollow.rotY < 180 && target == 360)
			{
				_targetRotationPropulsion = 0;
			}

			_rotX = PropulsionVector.x;
			_rotY = _targetRotationPropulsion;
			_cameraRotX = _cameraFollow.rotX;
			_cameraRotY = _cameraFollow.rotY;


			StartCoroutine(RotationPropulsionCooldown());
		}
	}

	IEnumerator RotationPropulsionCooldown()
	{
		_rotationPropulsionInterpolator = 0;
		_rotationPropulsion = true;
		yield return new WaitForSeconds(_rotationPropulsionTime);
		_rotationPropulsion = false;
	}

	void RotationPropulsionLerp()
	{
		_cameraFollow.rotX = Mathf.Lerp(_cameraRotX, _rotX, _rotationPropulsionInterpolator);
		_cameraFollow.rotY = Mathf.Lerp(_cameraRotY, _rotY, _rotationPropulsionInterpolator);
		transform.rotation = Quaternion.Slerp(transform.rotation, (Quaternion.Euler(0,_targetRotationPropulsion, 0)),_rotationPropulsionInterpolator);

		_rotationPropulsionInterpolator += Time.deltaTime / _rotationPropulsionTime;
	}

	#endregion

	#region Slope
	void CheckSlope()
	{
		if (_slopeDetector.IsOnSlope && _slopeDetector.SlopeAngles >= _maxSlopeAngle && _slopeDetector.SlopeDistance <= 3f)
		{
			_nerfVelocity = true;
			_onSlope = true;
			_onSlopeDirection = _slopeDetector.SlopeDirection;
		}
		else
		{
			_nerfVelocity = false;
			_onSlope = false;
		}

		_smoothInputTarget = SmoothInput();
	}

	void SlopeVelocity()
	{
		_onSlopeVelocity = _onSlopeDirection * _onSlopeSpeed;

		_onSlopeSpeed = _onSlopeSpeed > _maxSpeedOnSlope ? _maxSpeedOnSlope : _onSlopeSpeed + _speedIncreasingOnSlope * Time.deltaTime; ;
	}
	#endregion

	void CameraSetting()
	{
		_cameraForward = _mainCamera.transform.forward;
		_cameraForward.y = 0;
		_cameraRight = _mainCamera.transform.right;
		_cameraRight.y = 0;
		_cameraUp = _mainCamera.transform.up;
		_cameraUp.y = 0;
	}
	public static float SignedAngle(Vector3 from, Vector3 to, Vector3 normal)
	{
		float angle = Vector3.Angle(from, to);
		float sign = Mathf.Sign(Vector3.Dot(normal, Vector3.Cross(from, to)));
		return (angle * sign);
	}
	public void InfuseEnergy(int enable = 1)
	{
		Health += (_damageInfuseEnergy * enable);
		Debug.Log("vie du joueur à : " + Health);

	}
	IEnumerator JumpBuffer()
	{
		CanStillJump = true;
		yield return new WaitForSeconds(_extraTimeToJump);
		CanStillJump = false;
	}

	public void CheckJumpingOnSpot()
	{
		if(_velocity == Vector3.zero)
		_isJumpingOnSpot = true;
		else
		{
			_isJumpingOnSpot = false;
		}
	}

	void PauseState()
	{
		if (!_gameIsPaused)
		{
			_gameIsPaused = true;
			_velocityMemory = _velocity;
			_rbMemory = _rb.velocity;
			_rb.useGravity = false;
		}
		_velocity = Vector3.zero;
		_rb.velocity = Vector3.zero;
	}

	void Death()
	{
		_dead = false;

		if (_canRespawn)
		{
			Respawn();
		}
		else
		{

		}
	}

	void Respawn()
	{
		transform.position = RespawnPosition;
	}


	void LandingSound()
	{
		float volume = .5f;
		SoundManager.PlaySound(SoundManager.Sound.PlayerLand, volume);
	}

}
