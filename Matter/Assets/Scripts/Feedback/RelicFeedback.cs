using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicFeedback : MonoBehaviour
{
	Animator _animator;
	[SerializeField] GameObject _trails;

	private void Start()
	{
		_animator = GetComponent<Animator>();
		Off();
	}

	public void Off()
	{
		_animator.SetBool("activation", false);
		_trails.SetActive(false);
	}

	public void On()
	{
		_animator.SetBool("activation", true);
		_trails.SetActive(true);
	}
}
