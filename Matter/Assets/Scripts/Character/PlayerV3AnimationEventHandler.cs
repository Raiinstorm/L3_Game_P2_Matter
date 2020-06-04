using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerV3AnimationEventHandler : MonoBehaviour
{
	Animator _animator;
	PlayerControllerV3 _playerController;
	DeathPlayer _deathPlayer;
	public bool _canMove;
	bool soundAntispam;
	private void Start()
	{
		_animator = GetComponentInChildren<Animator>();
		_playerController = GetComponent<PlayerControllerV3>();
		_canMove = true;
		_deathPlayer = GetComponent<DeathPlayer>();
	}
	private void Update()
	{
		Velocity();
		IsGround();
		Propulsion();
		PropulsionIntensity();
		Respawn();
		if (_canMove)
			GetInput();
	}

	void GetInput()
	{
		_animator.SetFloat("xSpeed",(_playerController._velocity.magnitude/_playerController.MoveSpeed));

		if ((Input.GetButtonDown("Jump") && _playerController.IsGround() || Input.GetButtonDown("Jump") && _playerController.CanStillJump) && !_playerController._propulsed)
		{
			_playerController.CanStillJump = false;
			_animator.SetBool("jump", true);
			_playerController.IsJumping = true;
		}

		if (Input.GetButtonDown("MainMechanic") || Input.GetButtonDown("MainMechanicCancel"))
			Power();
	}

	public void IsJump()
	{
		_animator.SetBool("jump", false);
		_playerController.IsJumping = false;
	}

	public void IsGround()
	{
		//Debug.Log("Check");
		if (_playerController.IsGround())
			_animator.SetBool("isGround", true);
		else
			_animator.SetBool("isGround", false);
	}

	public void Velocity()
	{
		_animator.SetFloat("ySpeedRB", _playerController.IsGround() ? 0 : _playerController._rb.velocity.y);
		_animator.SetFloat("xSpeedRB", _playerController.IsGround() ? 0 : _playerController._rb.velocity.x);
	}

	public void Power()
	{
		_animator.SetTrigger("power");
	}

	public void MoveSound()
	{
		float volume = .5f;

		if(!soundAntispam)
		{
			if(!_playerController.IsRunning)
			{
				SoundManager.PlaySound(SoundManager.Sound.PlayerWalk,volume);
			}
			else
			{
				SoundManager.PlaySound(SoundManager.Sound.PlayerRun,volume*2);
			}
		}

		soundAntispam = !soundAntispam;
	}

	public void Propulsion()
	{
		if (_playerController._propulsed)
			_animator.SetBool("propulsed", true);
		else 
			_animator.SetBool("propulsed", false);
	}

	public void PropulsionIntensity()
	{
		_animator.SetFloat("propulsionIntensity", _playerController._propulsionIntensity);
	}

	public void Respawn()
	{
		if (_deathPlayer.Respawn)
		{
			_animator.SetTrigger("respawn");
			_deathPlayer.Respawn = false;
			_playerController._canMove = false;
		}
	}
	public void CanMove()
	{
		_playerController._canMove = true;
	}

}