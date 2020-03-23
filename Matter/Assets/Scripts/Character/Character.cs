﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected int _life;
    [SerializeField] protected int _lifeMax;
    [SerializeField] protected float _walkingSpeed;
    [SerializeField] protected float _runingSpeed;
    [SerializeField] protected float _jumpPower;
    [SerializeField] float _groundDistance = 0.4f;
    [SerializeField] protected float _gravity = -9.81f;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] Transform _groundCheck;

    protected Vector3 _moveDirection;
    protected Vector3 _velocity;
    enum _walkingMode { _walk, _run}


    protected virtual bool IsGround()
    {
        bool _isGround = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);
        if (_isGround)
            return true; 
        else
            return false; 
    }
    
    protected virtual void Walk()
    {
        if(IsGround() && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
    }

    protected virtual void Run()
    {

    }

    protected virtual void Jump()
    {
        _velocity = new Vector3(0.0f, _jumpPower, 0.0f);
    }

    protected virtual void Rotation()
    {

    }

    public virtual void GetDamage(int Damage)
    {
        _life -= Damage;
    }
}
