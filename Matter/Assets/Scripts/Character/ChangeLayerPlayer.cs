using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLayerPlayer : MonoBehaviour
{

    [SerializeField] GameObject _playerMesh;
    private void OnTriggerEnter(Collider other)
    {
        _playerMesh.layer = 0;
    }
}
