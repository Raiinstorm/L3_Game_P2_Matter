using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interruptor : MonoBehaviour
{
	[SerializeField] Extrude[] _bigExtrudes;

	bool _activated;

	bool _canBeActivated;

	[SerializeField] bool _forceActivation;

	[SerializeField] CameraShaking _cameraShaking;
	CameraSnapping _snap;

	[SerializeField] GameObject _button;
	[SerializeField] GameObject _relic;

	[SerializeField] SoundManager.Sound feedBackSound;

	[SerializeField] GameObject _canvas;
	[SerializeField] GameObject _imageCanvas;

	private void Start()
	{
		_snap = _cameraShaking.gameObject.GetComponent<CameraSnapping>();
	}


	void Update()
	{
		if((Input.GetButtonDown("Interract") && _canBeActivated || _forceActivation) && !_activated)
		{
			ActivateInterruptor();
			_forceActivation = false;
			_activated = true;
		}
	}



	private void OnTriggerEnter(Collider collision)
	{
		if(collision.gameObject.tag == "Character")
		{
			_canBeActivated = true;
		}
	}

	private void OnTriggerExit(Collider collision)
	{
		if (collision.gameObject.tag == "Character")
		{
			_canBeActivated = false;
		}
	}

	void ActivateInterruptor()
	{
		if(_bigExtrudes.Length !=0)
		{
			foreach (Extrude extrude in _bigExtrudes)
			{
				extrude.Activated = true;
			}
		}
		SoundManager.PlaySound(SoundManager.Sound.Button);
		SoundManager.PlaySound(feedBackSound);
		_cameraShaking.Shaking = true;
		_snap.ActivateLerp = true;
		_button.GetComponent<ButtonFeedback>().On();

		if (_relic != null)
		{
			_relic.GetComponent<RelicFeedback>().On();
			StartCoroutine(ThanksForPlaying());
		}
	}

	IEnumerator ThanksForPlaying()
	{
		yield return new WaitForSeconds(5);
		_canvas.SetActive(true);
		yield return new WaitForSeconds(1);
		_imageCanvas.SetActive(true);
	}
}
