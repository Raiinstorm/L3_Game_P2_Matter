using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] CharacterController _controller;
    [SerializeField] GameObject _cameraBase;
    [SerializeField] Camera _mainCamera;
    [SerializeField] bool _blockRotationPlayer;
    [SerializeField] int _damageInfuseEnergy;
    int _energyPower;
    float _allowPlayerRotation = 0.1f;
    Vector3 desiredMoveDirection;

    public float InputX { get; private set;}
    public float InputZ { get; private set; }
    public bool FastRun { get; private set; }


    private void Start()
    {
        _maxHealth = 100;
        _health = 100;
        _energyPower = 100;
        FastRun = false;
    }
    void Update()
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
        InputX = Input.GetAxisRaw("Horizontal");
        InputZ = Input.GetAxisRaw("Vertical");

        Vector3 move = _cameraBase.transform.right * InputX + _cameraBase.transform.forward * InputZ;

        _controller.Move(move * _walkingSpeed * Time.deltaTime);
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    protected override void Rotation()
    {
        //Calculate the Input Magnitude
        float speedMagnitude;
        speedMagnitude = new Vector2(InputX, InputZ).sqrMagnitude;

        if (speedMagnitude > _allowPlayerRotation)
        {
            InputX = Input.GetAxis("Horizontal");
            InputZ = Input.GetAxis("Vertical");

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
