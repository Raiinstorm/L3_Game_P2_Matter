using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterV2
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
    [SerializeField] float _rotationSpeed = 5000;

    public float InputX { get; private set;}
    public float InputZ { get; private set; }
    public bool FastRun { get; private set; }
    float _deadZone = 0.05f;


    Vector3 m_moveDirection;




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
        InputX = Input.GetAxisRaw("Horizontal");
        InputZ = Input.GetAxisRaw("Vertical");

       float dot = Vector3.Dot((Camera.main.transform.position - transform.position).normalized, transform.forward);
       InputX *= dot < 0f ? 1f : -1f;
       InputZ *= dot < 0f ? 1f : -1f;
        
       Rotation();
    }

    private void FixedUpdate()
    {
       Walk();
    }
    
    protected void Walk()
    {        
        smoothInputX = Mathf.SmoothDamp(smoothInputX, InputX, ref refX, dampFactor);
        smoothInputZ = Mathf.SmoothDamp(smoothInputZ, InputZ, ref refZ, dampFactor);

        if (Mathf.Abs(smoothInputX) < 0.05f)
            smoothInputX = 0;

        _rb.velocity = new Vector3(smoothInputX * _walkingSpeed * Time.deltaTime, _rb.velocity.y, smoothInputZ * _walkingSpeed * Time.fixedDeltaTime);
    }
    protected void Rotation()
    {
        if (Mathf.Abs(InputX) < 0.01f && Mathf.Abs(InputZ) < 0.01f)
            return;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(InputX, 0, InputZ)), Time.deltaTime * _rotationSpeed);
        //transform.Rotate(Vector3.up * InputX * Time.deltaTime * _rotationSpeed);

    }
    

    public void InfuseEnergy(int enable = 1)
    {
        Health += (_damageInfuseEnergy * enable);
        Debug.Log("vie du joueur à : " + Health);

    }
}
