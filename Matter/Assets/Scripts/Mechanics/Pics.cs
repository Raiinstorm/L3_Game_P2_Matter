using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pics : GeneriqueElement
{
    public override void apply(float enable = 1.0f)
    {
        transformY(DistanceExtrude * enable);
    }

    public override ElementType GetElementType()
    {
        return ElementType.Pics;
    }
}
