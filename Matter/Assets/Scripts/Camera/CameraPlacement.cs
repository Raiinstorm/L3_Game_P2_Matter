using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlacement : MonoBehaviour
{
	CameraSnapping _snap;
	CameraFollow _cam;

	public float _rotX;
	public float _rotY;

	[SerializeField] bool _debugActivated;
	[SerializeField] bool _blockRotation;

	private void Start()
	{
		_cam = GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraFollow>();
		_snap = _cam.gameObject.GetComponent<CameraSnapping>();
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.O) && _debugActivated)
		{
			SetPose();
		}
	}

	void SetPose()
	{
		_rotX = _cam.rotX;
		_rotY = _cam.rotY;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Character")
		{
			_snap.EnableLerp = true;
			_snap.RotXTarget = _rotX;
			_snap.RotYTarget = _rotY;
			_snap.StopCoroutine();
			_snap.BlockRotation = _blockRotation;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Character")
		{
			_snap.EnableLerp = false;
		}
	}
}
