using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrudeGround : PlatformeController
{
    public override void apply(float enable = 1.0f)
    {
        transformY(DistanceExtrude * enable);
    }
}
