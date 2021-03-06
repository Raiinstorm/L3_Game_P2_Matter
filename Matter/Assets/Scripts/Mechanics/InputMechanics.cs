﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InputMechanics : MonoBehaviour
{
    private ElementType returnMode;
    ModeSelection myMode = new ModeSelection();

    void Update()
    {
        //Debug.Log("Mon mode de sélection est : " + myMode.Selection());
        returnMode = myMode.Selection();
        if (Input.GetAxisRaw("TypesRoulette") > 0)
        {
            myMode.ChangeSelection();
            returnMode = myMode.Selection();
        }

        //if (Input.GetButtonDown("Reset")) // Commentaire add V1.07
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Commentaire add V1.07
    }

    public ElementType ReturnMode => returnMode;
}
