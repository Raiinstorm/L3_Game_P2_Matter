using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] CharacterController _controller;
    [SerializeField] GameObject _cameraBase;
    [SerializeField] bool _blockRotationPlayer;
    [SerializeField] int _damageInfuseEnergy;
    int _energyPower;
    Vector3 desiredMoveDirection;
    float _allowPlayerRotation = 0.01f;
    
    //SmoothingMove
    float smoothInputX;
    float smoothInputZ;
    float refX;
    float refZ;
    [SerializeField] float dampFactor;
    [SerializeField] float _rotationSpeed = 60;

    public float InputX { get; private set;}
    public float InputZ { get; private set; }
    public bool FastRun { get; private set; }


    private void Start()
    {
        _maxHealth = 100;
        _health = 100;
        _energyPower = 100;
        FastRun = false;
        _rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        //Walk();
        InputX = Input.GetAxisRaw("Horizontal");
        InputZ = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Walk();
        Rotation();

		if(Input.GetAxis("Jump") == 1)
		{
			Jump();
		}
    }

    protected override void Walk()
    {
        base.Walk();
		Vector3 move = _cameraBase.transform.right * InputX + _cameraBase.transform.forward * InputZ;
		_controller.Move(move * _walkingSpeed * Time.deltaTime);
		_velocity.y += _gravity * Time.deltaTime;
		_controller.Move(_velocity * Time.deltaTime);
	}
    protected override void Rotation()
    {

		float speedMagnitude;
		speedMagnitude = new Vector2(InputX, InputZ).sqrMagnitude;

		if (speedMagnitude > _allowPlayerRotation)
		{
			var forward = _cameraBase.transform.forward;
			var right = _cameraBase.transform.right;

			forward.y = 0f;
			right.y = 0f;

			forward.Normalize();
			right.Normalize();

			desiredMoveDirection = forward * InputZ + right * InputX;

			if (_blockRotationPlayer == false)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), 0.1f);
			}
		}
	}
    public void InfuseEnergy(int enable = 1)
    {
        Health += (_damageInfuseEnergy * enable);
        Debug.Log("vie du joueur à : " + Health);

    }
}
