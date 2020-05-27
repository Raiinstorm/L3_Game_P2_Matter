using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerV3 : CharacterV3
{

	Vector3 _velocity;
	Vector3 _rotation;
	float _deadZone = 0.2f;
	float _difAngle;
	protected IEnumerator _jumpBuffer;
	[HideInInspector] public bool CanStillJump;
	[HideInInspector] public bool _canMove;

	bool _gameIsPaused;
	Vector3 _velocityMemory;
	Vector3 _rbMemory;

	[Header ("Velocity")]
	[SerializeField] [Range(0, 1f)] float _smoothDamping;
	float _smoothInput;
	float _refSmoothInput;
	float _smoothInputTarget;
	[SerializeField] float MoveSpeed;
	[SerializeField] [Range(1, 3)] float _runMultiplicator;
	[SerializeField] float _maxSlopeAngle;
	float _moveSpeedRun;
	bool _isRunning;
	bool _nerfVelocity;
	[SerializeField] float _extraTimeToJump;
	[SerializeField] [Range(0, .5f)] float _turnAroundSpeed;
	float _powerInput;

	[Header("")]
	[SerializeField] int _damageInfuseEnergy;
	[SerializeField] SlopeDetector _slopeDetector;

	[HideInInspector] public bool IsJumping;

	[Header ("Propulsion")]
	public Vector3 PropulsionVector;
	[SerializeField] AnimationCurve _propulsionCurve;
	[SerializeField] float _propulsionTime;
	[SerializeField] float _propulsionPower;
	float _propulsionIntensity;
	float _lerpValue;
	[HideInInspector] public bool _propulsed;
	float _propulsionInterpolator;
	bool _antiSpamPropulsion;

	[SerializeField] float _fallMultiplicator;

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

	private void Start()
	{
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

		if (Input.GetButtonDown("Run"))
		{
			_isRunning = true;
		}

		if(_propulsed)
		{
			Propulsion();

			if(!_antiSpamPropulsion)
			{
				_antiSpamPropulsion = true;
				CheckRotationPropulsion();
			}
		}
		else
		{
			_antiSpamPropulsion = false;
		}

		if(_dead)
		{
			Death();
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
			if (!CanStillJump)
			{
				StartCoroutine(_jumpBuffer);
			}
		}
		else
		{
			_jumpBuffer = JumpBuffer();
		}

		y = _rb.velocity.y;


		if(y <0) //la ligne de code magique
		{
			y += Vector3.up.y * Physics.gravity.y * (_fallMultiplicator - 1) * Time.deltaTime;
		}

		if (_nerfVelocity && y >= 10 && !IsJumping)
		{
			_rb.velocity = new Vector3(_velocity.x, y / 2, _velocity.z);
		}
		else
		{
			_rb.velocity = new Vector3(_velocity.x , y, _velocity.z) + (PropulsionVector*_propulsionIntensity);

			//Debug.Log("velocity : " + new Vector3(_velocity.x, y, _velocity.z) + " | Propulsion : " + (PropulsionVector * _propulsionIntensity));

			PropulsionVector.y *= 1/(-Physics.gravity.y*.1f);
		}
	}

	void Propulsion()
	{
		_lerpValue = Mathf.Lerp(0, 1, _propulsionInterpolator);
		_propulsionIntensity = _propulsionCurve.Evaluate(_lerpValue) * _propulsionPower;

		_propulsionInterpolator += Time.deltaTime / _propulsionTime;

		if(_propulsionIntensity <= .01f)
		{
			_propulsed = false;
			_propulsionIntensity = 0;
			_lerpValue = 0;
			_propulsionInterpolator = 0;
		}
	}

	void CheckRotationPropulsion()
	{
		float difAngle = SignedAngle(transform.forward, new Vector3(PropulsionVector.x, 0, PropulsionVector.z), Vector3.up);

		Debug.Log(PropulsionVector);

		if ((difAngle <= -90 || difAngle >= 90) && PropulsionVector.y < .90f)
		{
			transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + difAngle, 0);


			_cameraFollow.rotX = PropulsionVector.x;
			_cameraFollow.rotY = transform.rotation.eulerAngles.y;
		}
	}


	void MoveInput()
	{
		Vector2 stickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		if (stickInput.magnitude < _deadZone)
		{
			stickInput = Vector2.zero;
			_smoothInputTarget = 0;
			_isRunning = false;
		}
		else
		{
			CheckSlope();

			_difAngle = SignedAngle(transform.forward, new Vector3(_rotation.x, 0f, _rotation.z), Vector3.up);

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


			if (_nerfVelocity && !_propulsed)
			{
				_velocity = (_velocity.normalized * MoveSpeed * _smoothInput * _powerInput) / MoveSpeed;
			}
			else
			{
				_velocity = _velocity.normalized * MoveSpeed * _smoothInput * _powerInput;
			}


			if (_velocity.magnitude > MoveSpeed && !_isRunning)
			{
				_velocity -= _velocity.normalized * (_velocity.magnitude - MoveSpeed);
			}
			if (_isRunning && _velocity.magnitude > _moveSpeedRun)
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

		if(_isRunning || stickPower > 1)
		{
			return 1;
		}
		else
		{
			return stickPower;
		}
	}

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

	public void IsJumpingOnSpot()
	{
		_isJumpingOnSpot = true;
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

	void CheckSlope()
	{
		if (_slopeDetector.IsOnSlope && _slopeDetector.SlopeAngles >= _maxSlopeAngle && _slopeDetector.SlopeDistance <= 2f)
		{
			_smoothInputTarget = 0;
			_nerfVelocity = true;
		}
		else
		{
			_nerfVelocity = false;
			_smoothInputTarget = SmoothInput();
		}
	}

	float SmoothInput()
	{
		if (_isRunning)
		{
			return _runMultiplicator;
		}
		else
		{
			return 1;
		}
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
}
