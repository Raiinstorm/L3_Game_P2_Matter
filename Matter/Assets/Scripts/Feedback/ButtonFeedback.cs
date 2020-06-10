using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFeedback : MonoBehaviour
{
	[SerializeField] GameObject _onPrefab;
	[SerializeField] GameObject _offPrefab;

	[SerializeField] Transform _fxPos;

	GameObject _on;
	GameObject _off;

	[SerializeField] Animator _animator;

	private void Start()
	{
		Off();
	}

	public void Off()
	{
		if(_onPrefab != null && _offPrefab != null)
		{
			_off = Instantiate(_offPrefab);
			_off.transform.position = _fxPos.position;
			StartCoroutine(DestroyObject(_on));
		}

		_animator.SetBool("activation", false);
	}

	public void On()
	{
		if (_onPrefab != null && _offPrefab != null)
		{
					_on = Instantiate(_onPrefab);
		_on.transform.position = _fxPos.position;
		StartCoroutine(DestroyObject(_off));
		}

		_animator.SetBool("activation",true);
		SoundManager.PlaySound(SoundManager.Sound.Button);
	}

	IEnumerator DestroyObject(GameObject target)
	{
		yield return new WaitForSeconds(.10f);
		if(target !=null)
		Destroy(target);
	}
}
