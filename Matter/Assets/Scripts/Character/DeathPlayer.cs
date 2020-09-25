using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlayer : MonoBehaviour
{
    PlayerControllerV3 _player;
    [SerializeField] Vector3 _currentlyPostion;
    [HideInInspector] public bool Respawn;

    void Start()
    {
        _player = GetComponent<PlayerControllerV3>();
        _currentlyPostion = _player.transform.position;
        Respawn = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            _currentlyPostion = other.gameObject.transform.position;
            other.gameObject.SetActive(false);
            // ADD ici l'activation du canvas pour montrer que ça save
        }

        if (other.CompareTag("DeathZone"))
        {
            _player.transform.position = new Vector3(_currentlyPostion.x, _currentlyPostion.y + 0.1f,_currentlyPostion.z) ;
            Respawn = true;
        }
    }
}
