using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDetection : MonoBehaviour
{
	MovablePlatform movablePlatform;

	private void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<MovablePlatform>() != null)
		{
			movablePlatform = other.GetComponent<MovablePlatform>();
			Debug.Log("plateforme détectée");
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<MovablePlatform>() != null)
		{
			movablePlatform = null;
			Debug.Log("plateforme plus détectée");
		}
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.O) && movablePlatform != null)
		{
			if (movablePlatform.isPushable && movablePlatform.isPullable == false)
			{
				Debug.Log("La plateforme est poussée");
				movablePlatform.Pushing();
			}
			if (movablePlatform.isPullable && movablePlatform.isPushable == false)
			{
				Debug.Log("La plateforme est tirée");
				movablePlatform.Pulling();

			}
		}
	}

	//To do : Gérer plusieurs détections en même temps
	//To do : gérer plusieurs mouvements
}
