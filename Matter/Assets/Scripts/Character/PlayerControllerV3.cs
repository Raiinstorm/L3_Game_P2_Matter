using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerV3 : CharacterV3
{
	Vector3 _velocity;
	float _deadZone = 0.2f;
	float _difAngle;
	protected IEnumerator _lastFrameJump;
	public bool CanStillJump;
	public bool _canMove;

	[SerializeField] float _extraTimeToJump;
	[SerializeField] [Range(0,.5f)] float _turnAroundSpeed;
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
		_lastFrameJump = LastFrameJump();
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
				StartCoroutine(_lastFrameJump);
			}
			y = _rb.velocity.y -_fallingFactor;
		}
		else
		{
			y = _rb.velocity.y;
			_lastFrameJump = LastFrameJump();
		}
		_rb.velocity = new Vector3(_velocity.x, y, _velocity.z);
	}
	void MoveInput()
	{
		Vector2 stickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		if (stickInput.magnitude < _deadZone)
			stickInput = Vector2.zero;
		else
		{
			_difAngle = SignedAngle(transform.forward, new Vector3(_velocity.x, 0f, _velocity.z), Vector3.up);

			if(_difAngle > 5 || _difAngle < -5)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y + _difAngle, 0), _turnAroundSpeed);
			}

			/*if (_difAngle > 6)
				transform.Rotate(new Vector3(0f, Mathf.Min(7f, _difAngle), 0f));
			else if (_difAngle < -4)
				transform.Rotate(new Vector3(0f, Mathf.Max(-7f, _difAngle), 0f));*/
		}

		Vector2 stickInputR = new Vector2(CamInputX, CamInputZ);
		if (stickInputR.magnitude < _deadZone)
			stickInputR = Vector2.zero;

		CameraSetting();
		_velocity = (_cameraRight.normalized * stickInput.x) + (_cameraForward.normalized * stickInput.y);
		_velocity = _velocity.normalized * MoveSpeed;
		if(_velocity.magnitude > MoveSpeed)
		{
			_velocity -= _velocity.normalized * (_velocity.magnitude - MoveSpeed);
			//Debug.LogError(_velocity.magnitude);
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
	IEnumerator LastFrameJump()
	{
		CanStillJump = true;
		yield return new WaitForSeconds(_extraTimeToJump);
		CanStillJump = false;
	}
}
