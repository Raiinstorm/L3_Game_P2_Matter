﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interruptor : MonoBehaviour
{
	[SerializeField] Extrude[] _bigExtrudes;

	bool _activated;

	bool _canBeActivated;

	[SerializeField] bool _forceActivation;

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
		foreach(Extrude extrude in _bigExtrudes)
		{
			extrude.Activated = true;
		}

		SoundManager.PlaySound(SoundManager.Sound.BigExtrude);
	}
}
