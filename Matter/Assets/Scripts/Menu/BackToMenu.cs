using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BackToMenu : MonoBehaviour
{

	private void Update()
	{
		if (Input.GetButtonDown("Submit"))
		{
			SceneManager.LoadScene(0);
		}
	}
}
