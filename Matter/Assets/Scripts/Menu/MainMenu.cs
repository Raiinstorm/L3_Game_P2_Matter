using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField] bool _playMusic;
	[SerializeField] float _volume;

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
		SoundManager.PlaySound(SoundManager.Sound.TitleMusicFirst, _volume,false,1,true);
		StartCoroutine(PlaySecondPart());
	}

	IEnumerator PlaySecondPart()
	{
		yield return new WaitForSeconds(SoundManager.GetAudioClip(SoundManager.Sound.TitleMusic).length);
		SoundManager.PlayLoop("Main Theme", SoundManager.Sound.TitleMusic, _volume, false, true);
	}
}
