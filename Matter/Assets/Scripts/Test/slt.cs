using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slt : MonoBehaviour
{
	static string mot1 = "g e n i a l";

	static string mot2 = "f o r t n i t e";

	string[] code1 = mot1.Split(new char[] {' '});

	string[] code2 = mot2.Split(new char[] {' '});

	int _nbrCharacter1;
	int _nbrCharacter2;

	int i;
	int j;

	int _number;

	[SerializeField] GameObject _guest1;
	[SerializeField] GameObject _guest2;
	GameObject _player;

	[HideInInspector] public bool LockRot;

	float _rot = 45;
	bool firstRot = true;

	private void Start()
	{
		_player = GameObject.FindGameObjectWithTag("Character");

		_nbrCharacter1 = code1.Length;
		_nbrCharacter2 = code2.Length;
	}
	void Update()
	{
		Detect();

		Check();
	}

	void Detect()
	{
		if(_number == 0)
		{
			if (Input.GetKeyDown(code1[i]))
			{
				j = 0;
				i++;
				_number = 1;
			}
			if (Input.GetKeyDown(code2[i]))
			{
				j++;
				i = 0;
				_number = 2;
			}
		}
		else
		{
			if (_number == 1)
			{
				if (Input.GetKeyDown(code1[i]))
				{
					j = 0;
					i++;
					_number = 1;
				}
			}
			else if (_number == 2)
			{
				if (Input.GetKeyDown(code2[j]))
				{
					j++;
					i = 0;
					_number = 2;
				}
			}
			else
			{
				_number = 0;
				i = 0;
				j = 0;
			}
		}
	}

	void Check()
	{
		if (i == _nbrCharacter1)
		{
			i = 0;
			j = 0;
			_number = 0;
			SoundManager.PlaySound(SoundManager.Sound.Genial, .5f, true);
		}
		if (j == _nbrCharacter2)
		{
			j = 0;
			j = 0;
			_number = 0;
			StartCoroutine(Fortnite());
		}
	}

	IEnumerator Fortnite()
	{
		GameObject guest1 = Instantiate(_guest1);
		GameObject guest2 = Instantiate(_guest2);

		GameMaster.i.ResetRotation = true;

		guest1.transform.position = _player.transform.position + new Vector3(3, 0, 3);
		guest2.transform.position = _player.transform.position + new Vector3(-3, 0, 3);
		guest1.transform.LookAt(_player.transform,Vector3.up);
		guest2.transform.LookAt(_player.transform,Vector3.up);
		guest2.transform.rotation = Quaternion.Euler(guest2.transform.rotation.eulerAngles.x-90,guest2.transform.rotation.eulerAngles.y,0);

		SoundManager.PlaySound(SoundManager.Sound.feetBack, .25f, true);

		yield return new WaitForSeconds(.5f);

		guest1.transform.rotation = Quaternion.Euler(guest1.transform.rotation.eulerAngles.x - 90, 0, 0);
		guest2.transform.rotation = Quaternion.Euler(guest2.transform.rotation.eulerAngles.x - 90, guest2.transform.eulerAngles.y- 180, 0);

		yield return new WaitForSeconds(.5f);

		guest1.transform.LookAt(_player.transform, Vector3.up);
		guest2.transform.rotation = Quaternion.Euler(guest2.transform.rotation.eulerAngles.x - 90, guest2.transform.rotation.eulerAngles.y, 0);

		yield return new WaitForSeconds(.125f);

		GuestRotate(guest1,guest2);

		yield return new WaitForSeconds(.16f);

		GuestRotate(guest1, guest2);

		yield return new WaitForSeconds(.16f);

		GuestRotate(guest1, guest2);

		yield return new WaitForSeconds(.17f);

		guest1.transform.position += Vector3.down;
		guest2.transform.position += Vector3.down;

		yield return new WaitForSeconds(.38f);

		guest1.transform.position -= Vector3.down;
		guest2.transform.position -= Vector3.down;

		yield return new WaitForSeconds(.295f);

		guest1.transform.LookAt(_player.transform, Vector3.up);
		guest2.transform.LookAt(_player.transform, Vector3.up);
		guest2.transform.rotation = Quaternion.Euler(guest2.transform.rotation.eulerAngles.x - 90, guest2.transform.rotation.eulerAngles.y, 0);

		firstRot = true;
		_rot *= .5f;

		yield return new WaitForSeconds(.672f);

		guest1.transform.rotation = Quaternion.Euler(guest1.transform.rotation.eulerAngles.x - 90, 0, 0);
		guest2.transform.rotation = Quaternion.Euler(guest2.transform.rotation.eulerAngles.x - 90, guest2.transform.eulerAngles.y - 180, 0);

		yield return new WaitForSeconds(.5f);

		guest1.transform.LookAt(_player.transform, Vector3.up);
		guest2.transform.rotation = Quaternion.Euler(guest2.transform.rotation.eulerAngles.x - 90, guest2.transform.rotation.eulerAngles.y, 0);

		yield return new WaitForSeconds(.125f);

		GuestRotate(guest1, guest2);

		yield return new WaitForSeconds(.16f);

		GuestRotate(guest1, guest2);

		yield return new WaitForSeconds(.16f);

		GuestRotate(guest1, guest2);

		yield return new WaitForSeconds(.17f);

		guest1.transform.position += Vector3.down;
		guest2.transform.position += Vector3.down;

		yield return new WaitForSeconds(.38f);

		guest1.transform.position -= Vector3.down;
		guest2.transform.position -= Vector3.down;

		yield return new WaitForSeconds(.3f);

		GuestRotate(guest1, guest2);

		yield return new WaitForSeconds(.12f);

		GuestRotate(guest1, guest2);

		yield return new WaitForSeconds(.72f);

		guest1.transform.position = _player.transform.position + new Vector3(1, 0, 1);
		guest2.transform.position = _player.transform.position + new Vector3(-1, 0, 1);

		yield return new WaitForSeconds(.3f);

		Destroy(guest1);
		Destroy(guest2);
		firstRot = true;
		_rot *= .5f;

		GameMaster.i.ResetRotation = false;


	}

	void GuestRotate(GameObject guest1,GameObject guest2)
	{
		guest1.transform.rotation = Quaternion.Euler(0,guest1.transform.rotation.eulerAngles.y +  _rot, 0);
		guest2.transform.rotation = Quaternion.Euler(guest2.transform.rotation.eulerAngles.x, guest2.transform.rotation.eulerAngles.y + _rot, 0);

		if (firstRot)
		{
			firstRot = false;
			_rot *= 2;
		}

		_rot = -_rot;
	}
}
