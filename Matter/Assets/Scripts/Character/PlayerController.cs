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
    }

    protected override void Walk()
    {
        //_rb.velocity = new Vector3(InputX * _walkingSpeed * 100 * Time.deltaTime, _rb.velocity.y, InputZ * _walkingSpeed * 100 * Time.deltaTime);
               
        smoothInputX = Mathf.SmoothDamp(smoothInputX, InputX, ref refX, dampFactor);
        smoothInputZ = Mathf.SmoothDamp(smoothInputZ, InputZ, ref refZ, dampFactor);

        if (Mathf.Abs(smoothInputX) < 0.05f)
            smoothInputX = 0;

        _rb.velocity = new Vector3(smoothInputX * _walkingSpeed * Time.deltaTime, _rb.velocity.y, smoothInputZ * _walkingSpeed * Time.fixedDeltaTime);
    }
    protected override void Rotation()
    {
       
            //Calculate the Input Magnitude
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
                
                float dot = Vector3.Dot(transform.forward, (_cameraBase.transform.position - transform.position).normalized);
                if(dot < 0.3f)
                {
                    Quaternion deltaRotation = Quaternion.AngleAxis(InputX, transform.up) * Quaternion.AngleAxis(InputZ, transform.up);
                    _rb.MoveRotation(deltaRotation * transform.rotation);
                }
                //_rb.rotation = Quaternion.Slerp(_rb.rotation, Quaternion.LookRotation(desiredMoveDirection), 0.1f);
                else
                {
                    Quaternion deltaRotation = Quaternion.AngleAxis(InputX*-1, transform.up) * Quaternion.AngleAxis(InputZ*-1, transform.up);
                    _rb.MoveRotation(deltaRotation * transform.rotation);
                }
                //_rb.rotation = Quaternion.Slerp(_rb.rotation, Quaternion.LookRotation(desiredMoveDirection*-1), 0.1f);   
            }

            /*
                            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, Quaternion.LookRotation(desiredMoveDirection), 0.1f));


                            Quaternion deltaRotation = Quaternion.AngleAxis(InputX, transform.up) * Quaternion.AngleAxis(InputZ,transform.up);
                            _rb.MoveRotation(deltaRotation * transform.rotation);
              */

            //_rb.rotation = Quaternion.Euler(_rb.rotation.eulerAngles + new Vector3(0f, _rotationSpeed * InputX * InputZ, 0f));
        }
    }
    public void InfuseEnergy(int enable = 1)
    {
        Health += (_damageInfuseEnergy * enable);
        Debug.Log("vie du joueur à : " + Health);

    }
}
