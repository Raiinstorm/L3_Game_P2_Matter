using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartNextScene : MonoBehaviour
{
	[SerializeField] bool isMenu;
    void Start()
    {
		if(!isMenu)
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    // Update is called once per frame
    void Update()
    {
        if(isMenu && Input.GetKeyDown(KeyCode.Space))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
    }
}
