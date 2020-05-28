using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propulsion : MonoBehaviour
{
	public bool _propulsionActivated;
	public Vector3 _direction;
	public Transform ClippingTransform;
	[SerializeField] float _propulsionTime;

	bool _playerOnPlatform;

	PlayerControllerV3 _player;
	bool _antispam;


	void Update()
	{
		if (_propulsionActivated && _playerOnPlatform && !_antispam)
		{
			_antispam = true;
			_player.PropulsionVector = _direction;
			_player.transform.position = ClippingTransform.position;
			_player.ResetPropulsion();
			_player._propulsed = true;
		}
	}

	private void OnTriggerEnter(Collider collision)
	{

		if(collision.gameObject.tag == "Character")
		{
			_playerOnPlatform = true;
			_player = collision.gameObject.GetComponent<PlayerControllerV3>();
		}

	}

	private void OnTriggerExit(Collider collision)
	{
		if (collision.gameObject.tag == "Character")
		{
			StartCoroutine(OnPlatformCoolDown());
		}
	}

	public IEnumerator PropulsionCooldown()
	{
		_propulsionActivated = true;
		yield return new WaitForSeconds(_propulsionTime);
		_propulsionActivated = false;
		_antispam = false;
	}

	IEnumerator OnPlatformCoolDown()
	{
		yield return new WaitForSeconds(.1f);
		_playerOnPlatform = false;
	}
}
