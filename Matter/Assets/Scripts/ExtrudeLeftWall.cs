using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrudeLeftWall : PlatformeController
{
    public override void apply(float enable = 1.0f)
    {
        transformX(-DistanceExtrude * enable);
    }
}
