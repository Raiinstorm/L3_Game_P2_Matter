using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
	public bool WallDetected;
	Transform _thisTransform;
	[SerializeField] LayerMask _groundLayer;

	private void Start()
	{
		_thisTransform = GetComponent<Transform>();
	}
	private void Update()
	{
		WallDetected = Physics.CheckSphere(_thisTransform.position, .4f, _groundLayer);
	}
}
