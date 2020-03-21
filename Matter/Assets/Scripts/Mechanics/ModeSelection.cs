using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSelection : MonoBehaviour
{
    [SerializeField]
    private List<GeneriqueElement> elements = new List<GeneriqueElement>();
    private Dictionary<ElementType, GeneriqueElement> elementsDico = new Dictionary<ElementType, GeneriqueElement>();

    ElementType TypesSelection; 

    private void Awake()
    {
        elements.ForEach(e => elementsDico.Add(e.GetElementType(), e));
    }

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

    private void Update()
    {
        Debug.Log("Mon mode de sélection est : " + Selection());
    }
}
