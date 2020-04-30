using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected int _health;
    [SerializeField] protected int _maxHealth;
    [SerializeField] protected float _walkingSpeed;
    [SerializeField] protected float _fastRunSpeed;
    [SerializeField] protected float _jumpForce;
    [SerializeField] float _groundDistance = 0.4f;
    [SerializeField] protected float _fallAceleration;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] Transform _groundCheck;

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

    public virtual void Jump()
    {
        if (IsGround())
            _rb.AddForce(Vector3.up * Mathf.Sqrt(_jumpForce * -2f * Physics.gravity.y), ForceMode.VelocityChange);
    }

    public virtual void GetDamage(int Damage)
    {
        _health -= Damage;
    }
}
