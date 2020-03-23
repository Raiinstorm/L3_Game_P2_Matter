using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSelection : MonoBehaviour
{
    ElementType TypesSelection; 

    public ElementType Selection()
    {
        return TypesSelection;
    }

    public void ChangeSelection() // A améliorer ultérieurement avec la roue des types.
    {
        if (Input.GetAxis("TypesChangeUp") > 0)
        {
            TypesSelection = ElementType.Pics;
        }    
        if (Input.GetAxis("TypesChangeUp") < 0)
        {
            TypesSelection = ElementType.Extrude;
        }       
        if (Input.GetAxis("TypesChangeDown") > 0)
        {
            TypesSelection = ElementType.Glass;
        }
    }

}
