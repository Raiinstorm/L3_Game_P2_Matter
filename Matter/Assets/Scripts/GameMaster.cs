using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
	void Start()
	{
		StartCoroutine(PlayLoops());
	}

	IEnumerator PlayLoops()
	{
		yield return new WaitForSeconds(1);

		SoundManager.PlayLoop("Ambient", SoundManager.Sound.AmbientInGameLoop,.5f);
		SoundManager.PlayLoop("Music", SoundManager.Sound.MusicLoop,.5f);
	}

}
