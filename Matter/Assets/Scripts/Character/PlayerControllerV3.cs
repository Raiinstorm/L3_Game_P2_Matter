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
	public bool CanStillJump;
	public bool _canMove;

	[SerializeField] float _extraTimeToJump;
	[SerializeField] [Range(0,.5f)] float _turnAroundSpeed;

	[SerializeField] [Range(0,1f)] float _smoothDamping;
	float _smoothInput;
	float _refSmoothInput;
	float _smoothInputTarget;
	[SerializeField] float MoveSpeed;

	[SerializeField] int _damageInfuseEnergy;

	public float InputX { get; private set; }
	public float InputZ { get; private set; }
	public float CamInputX { get; private set; }
	public float CamInputZ { get; private set; }
	public bool FastRun { get; private set; }

	[Header("Camera")]
	[SerializeField] Camera _mainCamera;

	Vector3 _cameraForward;      // vector forward "normalisé" de la cam
	Vector3 _cameraRight;        // vector right "normalisé" de la cam
	Vector3 _cameraUp;

	private void Start()
	{
		_maxHealth = 100;
		_health = 100;
		FastRun = false;
		_rb = GetComponent<Rigidbody>();
		_jumpBuffer = JumpBuffer();
		_canMove = true;
	}
	private void FixedUpdate()
	{
		ApplyVelocity();
		if(_canMove)
			MoveInput();
	}
	private void Update()
	{
		InputX = Input.GetAxis("Horizontal");
		InputZ = Input.GetAxis("Vertical");
		CamInputX = Input.GetAxis("RightStickHorizontal");
		CamInputZ = Input.GetAxis("RightStickVertical");
	}
	void ApplyVelocity()
	{
		float y = 0;
		if(!IsGround())
		{
			if(!CanStillJump)
			{
				StartCoroutine(_jumpBuffer);
			}
			y = _rb.velocity.y -_fallingFactor;
		}
		else
		{
			y = _rb.velocity.y;
			_jumpBuffer = JumpBuffer();
		}
		_rb.velocity = new Vector3(_velocity.x, y, _velocity.z);
	}
	void MoveInput()
	{
		Vector2 stickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		if (stickInput.magnitude < _deadZone)
		{
			stickInput = Vector2.zero;
			_smoothInputTarget = 0;
		}
		else
		{
			_smoothInputTarget = 1;
			_difAngle = SignedAngle(transform.forward, new Vector3(_rotation.x, 0f, _rotation.z), Vector3.up);

			if(_difAngle > 5 || _difAngle < -5)
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

		if (!_isJumpingOnSpot)
		{
			_smoothInput = Mathf.SmoothDamp(_smoothInput, _smoothInputTarget, ref _refSmoothInput, _smoothDamping); //augmentation progressive de la vitesse
			_velocity = _velocity.normalized * MoveSpeed * _smoothInput;
			if (_velocity.magnitude > MoveSpeed)
			{
				_velocity -= _velocity.normalized * (_velocity.magnitude - MoveSpeed);
			}
		}
		else
		{
			_velocity = Vector3.zero;
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
}
