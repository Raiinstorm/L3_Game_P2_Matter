using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerV3AnimationEventHandler : MonoBehaviour
{
	Animator _animator;
	PlayerControllerV3 _playerController;
	public bool _canMove;
	private void Start()
	{
		_animator = GetComponentInChildren<Animator>();
		_playerController = GetComponent<PlayerControllerV3>();
		_canMove = true;
	}
	private void Update()
	{
		if(_canMove)
		{
			GetInput();
		}
	}

	void GetInput()
	{
		_animator.SetFloat("xSpeed", new Vector2(_playerController.InputX, _playerController.InputZ).magnitude);

		if (Input.GetButtonDown("Jump") && _playerController.IsGround() || Input.GetButtonDown("Jump") && _playerController.CanStillJump)
		{
			_playerController.CanStillJump = false;
			_animator.SetBool("jump", true);
			_playerController.IsJumping = true;
		}
	}

	public void IsJump()
	{
		_animator.SetBool("jump", false);
		_playerController.IsJumping = false;
	}

}