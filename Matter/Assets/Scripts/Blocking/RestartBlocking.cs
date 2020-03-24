using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartBlocking : MonoBehaviour
{
	// Start is called before the first frame update

	Transform _thisTransform;
	Vector3 _spawnPosition;
	public static List<Vector3> spawns = new List<Vector3>();
	int _navigate;

    void Start()
    {
		_thisTransform = GetComponent<Transform>();
		_spawnPosition = _thisTransform.position;
		spawns.Add(_spawnPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
		{
			_thisTransform.position = _spawnPosition;
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			if(_navigate-1<0)
			{
				_navigate = spawns.Count-1;
			}
			else
			{
				_navigate--;
			}
			Tp();
		}

		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			if (_navigate + 1 == spawns.Count)
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

	void Tp()
	{
		_thisTransform.position = spawns[_navigate];
	}
}
