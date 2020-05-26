using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterV3 : MonoBehaviour
{
	[Header ("Stats")]
	[SerializeField] protected int _health;
	[SerializeField] protected int _maxHealth;
	[SerializeField] protected float _jumpForce;

	[Header ("Ground")]
	[SerializeField] float _groundDistance = 0.4f;
	[SerializeField] LayerMask _groundMask;
	[SerializeField] Transform _groundCheck;
	protected Transform _thisTransform;

	protected bool _isJumpingOnSpot;

	protected Rigidbody _rb;

	[HideInInspector] public Vector3 RespawnPosition;

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
		bool _isGround = Physics.CheckBox(_groundCheck.position, new Vector3(0, _groundDistance, 0),Quaternion.identity,_groundMask);
		//bool _isGround = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);
		if (_isGround)
		{
			return true;
		}
		else
		{
			return false;
		}
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

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(_groundCheck.position, _groundDistance);
	}

}
