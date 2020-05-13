﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : MonoBehaviour
{
    [SerializeField] PlayerControllerV3 _player;
    List<ZoneController> Faults = new List<ZoneController>();

    public void Lock()
    {
        Faults = _player.GetComponent<ZoneDetector>().Faults;
        do
        {
            foreach (ZoneController Fault in Faults)
            {
                var position = Fault.transform.position - _player.transform.position;
                var distance = position.magnitude;
                var direction = position / distance;

                gameObject.transform.position = new Vector3(position.x * 2, position.y * 2, position.z * 2);
                //gameObject.transform.rotation = direction;
            }
        }
        while (Input.GetAxisRaw("CameraLook") > 0 );
    }
}
