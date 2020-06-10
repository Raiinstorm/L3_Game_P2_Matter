using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpInGame : MonoBehaviour
{
	[SerializeField] GameObject _helpInGameCanvas;

	private void OnTriggerEnter(Collider other)
	{
		_helpInGameCanvas.SetActive(true);
	}

	private void OnTriggerExit(Collider other)
	{
		_helpInGameCanvas.SetActive(false);
	}
}
