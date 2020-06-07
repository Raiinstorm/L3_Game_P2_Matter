using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlayer : MonoBehaviour
{
    PlayerControllerV3 _player;
    [SerializeField] Vector3 _currentlyPostion;
    [SerializeField] float _timeCheckPos;
    [SerializeField] float _timeCheckPosTemp;
    [HideInInspector] public bool Respawn;

    void Start()
    {
        _player = GetComponent<PlayerControllerV3>();
        _currentlyPostion = _player.transform.position;
        _timeCheckPosTemp = _timeCheckPos;
        Respawn = false;
    }
    private void Update()
    {
        if(_timeCheckPosTemp >= 0)
            _timeCheckPosTemp -= Time.deltaTime;
        else
        {
            CheckPosition();
            _timeCheckPosTemp = _timeCheckPos;
        }

    }
    void CheckPosition()
    {
        if (_player.IsGround())
            _currentlyPostion = _player.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathZone"))
        {
            _player.transform.position = new Vector3(_currentlyPostion.x, _currentlyPostion.y + 0.1f,_currentlyPostion.z) ;
            Respawn = true;
        }
    }
}
