using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterV3 : MonoBehaviour
{
	[SerializeField] protected int _health;
	[SerializeField] protected int _maxHealth;
	[SerializeField] protected float _jumpForce;
	[SerializeField] float _groundDistance = 0.4f;
	[SerializeField] LayerMask _groundMask;
	[SerializeField] Transform _groundCheck;

	protected bool _isJumpingOnSpot;

	[SerializeField] [Range(0,.1f)] protected float _fallingFactor;

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
		_isJumpingOnSpot = false;
		_rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
		_rb.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
	}

	public virtual void GetDamage(int Damage)
	{
		_health -= Damage;
	}

}
