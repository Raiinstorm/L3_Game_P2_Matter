﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class CameraEnableDepth : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
    }
}
