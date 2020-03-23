using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InputMechanics : MonoBehaviour
{
    private static ElementType returnMode;
    ModeSelection myMode = new ModeSelection();

    void Update()
    {
        Debug.Log("Mon mode de sélection est : " + myMode.Selection());
        returnMode = myMode.Selection();
        if (Input.GetAxisRaw("TypesRoulette") > 0)
        {
            myMode.ChangeSelection();
            returnMode = myMode.Selection();
        }

        if (Input.GetButtonDown("Reset"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static ElementType ReturnMode => returnMode;
}
