using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected int _health;
    [SerializeField] protected int _maxHealth;
    [SerializeField] protected float _walkingSpeed;
    [SerializeField] protected float _runingSpeed;
    [SerializeField] protected float _jumpPower;
    [SerializeField] float _groundDistance = 0.4f;
    [SerializeField] protected float _gravity = -9.81f;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] Transform _groundCheck;

	[SerializeField] protected bool _projection;
	float _projectionFloat;
	[SerializeField] protected GameObject _cubeProjection;

    protected Vector3 _moveDirection;
    protected Vector3 _velocity;

    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int Health { get => _health;
        set
        {
            var _oldhealth = _health;
            _health = (value > 0) ? (value < MaxHealth ? value : MaxHealth) : 0;
        }        
    }


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
		if(_projection)
		{
			_projectionFloat = 1.5f;
		}
		else
		{
			_projectionFloat = 1;
		}
        _velocity = new Vector3(0.0f, _jumpPower * _projectionFloat, 0.0f);
    }

    protected virtual void Rotation()
    {

    }

    public virtual void GetDamage(int Damage)
    {
        _health -= Damage;
    }
}
