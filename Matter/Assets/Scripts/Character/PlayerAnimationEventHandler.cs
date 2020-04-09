using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    Animator _animator;
    PlayerController _playerController;
    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerController = GetComponent<PlayerController>();
    }
    private void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        _animator.SetFloat("xSpeed", new Vector2(_playerController.InputX, _playerController.InputZ).magnitude);

        if(Input.GetButtonDown("Jump") && _playerController.IsGround())
        {
            _animator.SetBool("jump", true);
        }
    }

}