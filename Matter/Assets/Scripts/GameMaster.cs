using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMaster : MonoBehaviour
{

	public bool playLoops;

	void Start()
	{
		if(playLoops)
		{
			StartCoroutine(PlayLoops());
		}
	}

	IEnumerator PlayLoops()
	{
		yield return new WaitForSeconds(1);

		SoundManager.PlayLoop("Ambient", SoundManager.Sound.AmbientInGameLoop,.5f,true);
		SoundManager.PlayLoop("Music", SoundManager.Sound.MusicLoop,.5f,true);
	}

}
