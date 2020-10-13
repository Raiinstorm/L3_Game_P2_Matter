using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _menu;
    [SerializeField] PlayerControllerV3 _player;
    [SerializeField] GameObject _camera;
    [SerializeField] PlayerV3AnimationEventHandler _anim;
    [SerializeField] Button _btnResume;

    private bool _gameIsPause;
    private Animator _animator;

    private void Start()
    {
        _menu.SetActive(false);
        _gameIsPause = false;
        _animator = _player.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("PauseMenu"))
        {
            if (!_gameIsPause)
            {
                Pause();
                _gameIsPause = true;
            }
            else
                Resume(); 
        }
    }

    public void Resume()
    {
        _menu.SetActive(false);
        _gameIsPause = false;
        _player._canMove = true;
        _camera.GetComponent<CameraFollow>().enabled = true;
        _animator.speed = 1;
        _anim._canMove = true;
    }
    public void Pause()
    {
        _menu.SetActive(true);
        _btnResume.Select();
        _gameIsPause = true;
        _player._canMove = false;
        _camera.GetComponent<CameraFollow>().enabled = false;
        _animator.speed = 0;
        _anim._canMove = false;
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }


}
