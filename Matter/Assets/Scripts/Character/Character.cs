﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected int _health;
    [SerializeField] protected int _maxHealth;
    [SerializeField] protected float _walkingSpeed;
    [SerializeField] protected float _fastRunSpeed;
    [SerializeField] protected float _jumpForce;
	[SerializeField] protected float _gravity = -9.81f;
	[SerializeField] float _groundDistance = 0.4f;
    [SerializeField] protected float _fallAceleration;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] Transform _groundCheck;
	protected Vector3 _velocity;

	protected Rigidbody _rb;

    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int Health { get => _health;
        set
        {
            var _oldhealth = _health;
            _health = (value > 0) ? (value < MaxHealth ? value : MaxHealth) : 0;
        }        
    }


    public virtual bool IsGround()
    {
        bool _isGround = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);
        if (_isGround)
            return true; 
        else
            return false; 
    }
    
    protected virtual void Walk()
    {
		if (IsGround() && _velocity.y < 0)
			_velocity.y = -2f;
	}

    public virtual void Jump()
    {
        if (IsGround())
			_velocity = new Vector3(0.0f, _jumpForce, 0.0f);
	}
    protected virtual void Rotation()
    {

    }

    public virtual void GetDamage(int Damage)
    {
        _health -= Damage;
    }
}
