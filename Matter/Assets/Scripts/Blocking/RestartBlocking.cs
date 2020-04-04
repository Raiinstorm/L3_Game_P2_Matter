using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartBlocking : MonoBehaviour
{
	// Start is called before the first frame update

	Transform _playerTransform;
	Vector3 _spawnPosition;
	public Transform[] spawns;
	[SerializeField]
	int _navigate;

	bool _antiSpam;

	[SerializeField]
	Vector3 _position;

    void Start()
    {
		_playerTransform = GetComponent<Transform>();
		_spawnPosition = _playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
		{
			_playerTransform.position = _spawnPosition;
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			if(!_antiSpam)
			{
				_antiSpam = true;

				if (_navigate - 1 < 0)
				{
					_navigate = spawns.Length - 1;
				}
				else
				{
					_navigate--;
				}
				Tp();
			}
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			if(!_antiSpam)
			{
				_antiSpam = true;

				if (_navigate + 1 == spawns.Length)
				{
					_navigate = 0;
				}
				else
				{
					_navigate++;
				}

				Tp();
			}
		}
		else
		{
			_antiSpam = false;
		}
	}

	void Tp()
	{
		_position = spawns[_navigate].position;
		_playerTransform.position = _position;
	}
}
