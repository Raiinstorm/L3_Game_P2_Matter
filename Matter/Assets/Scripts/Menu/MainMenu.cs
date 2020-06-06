using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField] bool _playMusic;
	[SerializeField] float _volume = .1f;

	private void Start()
	{
		if(_playMusic)
		{
			PlayMusic();
		}
	}

	public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

	void PlayMusic()
	{
		SoundManager.PlayLoop("MainTheme", SoundManager.Sound.TitleMusic, _volume, true);
	}
}
