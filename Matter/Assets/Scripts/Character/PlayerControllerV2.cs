using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerV2 : MonoBehaviour
{
	Vector3 _velocity;
	float _deadZone = 0.2f;
	float _difAngle;
	int _energyPower;
	[SerializeField] float _gravity;
	[SerializeField] protected int _health;
	[SerializeField] protected int _maxHealth;
	[SerializeField] protected float _jumpForce;
	[SerializeField] float MoveSpeed;
	[SerializeField] int _damageInfuseEnergy;
	[SerializeField] LayerMask _groundMask;
	[SerializeField] Transform _groundCheck;
	[SerializeField] float _groundDistance = 0.4f;
	protected Rigidbody _rb;

	public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
	public int Health
	{
		get => _health;
		set
		{
			var _oldhealth = _health;
			_health = (value > 0) ? (value < MaxHealth ? value : MaxHealth) : 0;
		}
	}

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
		_rb.velocity = _velocity;
		
		MoveInput();
	}

	private void Update()
	{
		InputX = Input.GetAxis("Horizontal");
		InputZ = Input.GetAxis("Vertical");
		CamInputX = Input.GetAxis("RightStickHorizontal");
		CamInputZ = Input.GetAxis("RightStickVertical");

	}

	public bool IsGround()
	{
		bool _isGround = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);
		if (_isGround)
			return true;
		else
			return false;
	}
	public void Jump()
	{
		if (IsGround())
			_rb.AddForce(_jumpForce* Vector3.up *100);
	}
	public virtual void GetDamage(int Damage)
	{
		_health -= Damage;
	}

	void MoveInput()
	{
		Vector2 stickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		if (stickInput.magnitude < _deadZone)                                                                   
			stickInput = Vector2.zero;                                                                     
		else                                                                                                    
		{                                                                                                       
			_difAngle = SignedAngle(transform.forward, new Vector3(_velocity.x, 0f, _velocity.z), Vector3.up);  
			if (_difAngle > 6)                                                                                   
				transform.Rotate(new Vector3(0f, Mathf.Min(7f, _difAngle), 0f));                               
			else if (_difAngle < -4)                                                                            
				transform.Rotate(new Vector3(0f, Mathf.Max(-7f, _difAngle), 0f));                                
		}
		Vector2 stickInputR = new Vector2(CamInputX,CamInputZ);
		if (stickInputR.magnitude < _deadZone)
			stickInputR = Vector2.zero;

		CameraSetting();
		_velocity = (_cameraRight.normalized * stickInput.x) + (_cameraForward.normalized * stickInput.y);
		_velocity *= MoveSpeed * ((180 - Mathf.Abs(_difAngle)) / 180);

		if (IsGround() && _velocity.y < 0)
			_velocity.y = -2f;
		else if (!IsGround() && _velocity.y <= 0)
			_velocity.y = Physics.gravity.y;

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
}
