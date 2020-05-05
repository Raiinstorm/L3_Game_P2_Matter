using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerV2 : CharacterV2
{
	Vector3 _moveDirection;
	public float _gravityScale;
	float _deadZone = 0.25f;
	float _difAngle;
	int _energyPower;
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
		_energyPower = 100;
		FastRun = false;
		_rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		_rb.velocity = _moveDirection;
		MoveInput();
	}

	private void Update()
	{
		InputX = Input.GetAxis("Horizontal");
		InputZ = Input.GetAxis("Vertical");
		CamInputX = Input.GetAxis("RightStickHorizontal");
		CamInputZ = Input.GetAxis("RightStickVertical");

		Debug.LogWarning(_rb.velocity);
		//MoveInput();
	}

	void MoveInput()
	{
		if (_rb.velocity.y > 10 && IsGround())
			_rb.velocity = Vector3.zero;

		Vector2 stickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		if (stickInput.magnitude < _deadZone)
		{
			stickInput = Vector2.zero;
		}                                                         
		else                                                                                                    
		{                                                                                                       
			_difAngle = SignedAngle(transform.forward, new Vector3(_moveDirection.x, 0f, _moveDirection.z), Vector3.up);

			if (_difAngle > 4)                                                                                  
			{
				transform.Rotate(new Vector3(0f, Mathf.Min(7f, _difAngle), 0f));                        
			}                                                                                                   
			else if (_difAngle < -4)                                                                            
			{
				transform.Rotate(new Vector3(0f, Mathf.Max(-7f, _difAngle), 0f));                                
			}
		}

		//Debug.Log("dif Angle " + _difAngle);// + " stick Input " + stickInput.magnitude);
		Vector2 stickInputR = new Vector2(CamInputX,CamInputZ);
		if (stickInputR.magnitude < _deadZone)
			stickInputR = Vector2.zero;

		GetCamSettings();

		float yStored = _rb.velocity.y;
		_moveDirection = (_cameraRight.normalized * stickInput.x) + (_cameraForward.normalized * stickInput.y);
		_moveDirection *= MoveSpeed * ((180 - Mathf.Abs(_difAngle)) / 180);
		_moveDirection.y = yStored;


		#region Gravity
		if (IsGround())
		{
			_gravityScale = 1;
			_moveDirection.y = _rb.velocity.y;
		}
		else
		{
			_moveDirection.y = _rb.velocity.y - _gravityScale;
			if (_moveDirection.y < 0)
				_moveDirection.y *= 1.1f;
			if (Mathf.Abs(_moveDirection.y) > 70)
				_moveDirection.y = -71;
		}
		#endregion

	}

	void GetCamSettings()
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
}
