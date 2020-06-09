using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimDissolve : MonoBehaviour
{
	Animator _animator;

	private void Start()
	{
		_animator = GetComponent<Animator>();
	}

	public void Activate()
	{
		_animator.SetBool("activate", true);
	}

	public void Desactivate()
	{
		_animator.SetBool("activate", false);
	}
}
