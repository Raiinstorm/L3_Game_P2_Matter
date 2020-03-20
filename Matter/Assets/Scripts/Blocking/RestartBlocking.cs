using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartBlocking : MonoBehaviour
{
	// Start is called before the first frame update

	Transform _thisTransform;
	Vector3 _spawnPosition;
    void Start()
    {
		_thisTransform = GetComponent<Transform>();
		_spawnPosition = _thisTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
		{
			_thisTransform.position = _spawnPosition;
		}
    }
}
