using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyZoneController : MonoBehaviour
{
    [SerializeField]
    private List<GeneriqueElement> elements = new List<GeneriqueElement>();
    private Dictionary<ElementType,GeneriqueElement> elementsDico = new Dictionary<ElementType,GeneriqueElement>();

    private Stack<GeneriqueElement> activatedElements = new Stack<GeneriqueElement>();

    private void Awake()
    {
        elements.ForEach(e => elementsDico.Add(e.GetElementType(), e));
    }

    public void Actived(ElementType myType)
    {
        var activatedElement = elementsDico[myType];
        if (activatedElement.m_activated)
            return;

        // test d'empêcher des pics au dessus de la glace
        if (activatedElements.Count > 0 &&
            activatedElements.Peek().GetElementType() == ElementType.Glass &&
            myType == ElementType.Pics)
            return;

        activatedElements.Push(activatedElement);
        activatedElement.apply();
    }

    public void Deactived()
    {
        if (activatedElements.Count == 0)
            return;

        var activatedElement = activatedElements.Pop();
        activatedElement.apply(0);
    }
}
