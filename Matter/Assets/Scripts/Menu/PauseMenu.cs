using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _menu;
    [SerializeField] PlayerControllerV3 _player;
    [SerializeField] PlayerV3AnimationEventHandler _anim;
    private bool _gameIsPause;

    private void Start()
    {
        _menu.SetActive(false);
        _gameIsPause = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("PauseMenu"))
        {
            if (!_gameIsPause)
                Pause();
            else
                Resume();
        }
    }

    public void Resume()
    {
        _menu.SetActive(false);
        _gameIsPause = false;
        _player._canMove = true;
        _anim._canMove = true;
    }
    public void Pause()
    {
        _menu.SetActive(true);
        _gameIsPause = true;
        _player._canMove = false;
        _anim._canMove = false;
    }
    public void QuitGame()
    {
        Application.Quit();
    }


}
