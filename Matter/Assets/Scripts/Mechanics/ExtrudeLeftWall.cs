using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrudeLeftWall : GeneriqueElement
{
    public override void apply(float enable = 1.0f)
    {
        transformX(-DistanceExtrude * enable);
    }

    public override ElementType GetElementType()
    {
        return ElementType.Extrude;
    }
}
