using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMechanics : MonoBehaviour
{
    private static ElementType returnMode;
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
    }

    public static ElementType ReturnMode => returnMode;
}
