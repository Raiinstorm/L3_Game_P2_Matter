using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] CharacterController _controller;
    [SerializeField] GameObject _cameraBase;
    [SerializeField] Camera _mainCamera;
    [SerializeField] bool _blockRotationPlayer;
    [SerializeField] float _sprintSpeed;

    int _energyPower;
    float _allowPlayerRotation = 0.1f;
    float _inputX;
    float _inputZ;

    Vector3 desiredMoveDirection;

    private void Start()
    {
        _lifeMax = 100;
        _energyPower = 100;
    }
    void Update()
    {
        Walk();
        Rotation();

        if (Input.GetButtonDown("Jump") && IsGround())
        {
            Debug.Log("Je saute");
            Jump();
        }
    }

    protected override void Walk()
    {
        base.Walk();

        _inputX = Input.GetAxis("Horizontal");
        _inputZ = Input.GetAxis("Vertical");

        Vector3 move = _cameraBase.transform.right * _inputX + _cameraBase.transform.forward * _inputZ;

        _controller.Move(move * _walkingSpeed * Time.deltaTime);
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    protected override void Rotation()
    {
        //Calculate the Input Magnitude
        float speedMagnitude;
        speedMagnitude = new Vector2(_inputX, _inputZ).sqrMagnitude;

        if (speedMagnitude > _allowPlayerRotation)
        {
            _inputX = Input.GetAxis("Horizontal");
            _inputZ = Input.GetAxis("Vertical");

            var forward = _cameraBase.transform.forward;
            var right = _cameraBase.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            desiredMoveDirection = forward * _inputZ + right * _inputX;

            if (_blockRotationPlayer == false)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), 0.1f);
            }
        }
    }
}
