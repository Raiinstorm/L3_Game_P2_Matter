using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerV3AnimationEventHandler : MonoBehaviour
{
	Animator _animator;
	PlayerControllerV3 _playerController;
	public bool _canMove;
	bool soundAntispam;
	private void Start()
	{
		_animator = GetComponentInChildren<Animator>();
		_playerController = GetComponent<PlayerControllerV3>();
		_canMove = true;
	}
	private void Update()
	{
		Velocity();
		IsGround();
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
		_animator.SetFloat("ySpeed", _playerController.IsGround() ? 0 : _playerController._rb.velocity.y);
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

}