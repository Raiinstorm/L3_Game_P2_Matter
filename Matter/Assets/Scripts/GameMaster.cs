using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMaster : MonoBehaviour
{
	public GenericElement Exception;

	public bool playLoops;
	[SerializeField] float volume = 1;

	static GameMaster _i;
	public static GameMaster i
	{
		get
		{
			if (_i == null)
			{
				_i = Instantiate(Resources.Load<GameMaster>("Prefabs/GameMaster"));
			}
			return _i;
		}
	}

	[Header("Extrudes")]
	public List<Extrude> AutoSwitchExtrudes;
	[SerializeField] float _switchTime = 2f;

	IEnumerator extrudesSwitch;

	public List<GenericElement> elements;

	public GameObject TrashCan;

	void Start()
	{

		if (playLoops)
		{
			StartCoroutine(PlayLoops());
		}
		if(AutoSwitchExtrudes.Count != 0)
		{
			extrudesSwitch = ExtrudesAutoSwitch();
			StartCoroutine(extrudesSwitch);
		}
	}

	IEnumerator PlayLoops()
	{
		yield return new WaitForSeconds(1);

		SoundManager.PlayLoop("Ambient", SoundManager.Sound.AmbientInGameLoop,volume,true);
		SoundManager.PlayLoop("Music", SoundManager.Sound.MusicLoop,volume,true);
	}

	IEnumerator ExtrudesAutoSwitch()
	{
		yield return new WaitForSeconds(_switchTime);

		Activate();
	}

	public void Activate()
	{

		foreach (Extrude extrude in AutoSwitchExtrudes)
		{
			if(extrude.SecondSwitching)
			{
				extrude.SecondSwitching = false;
				continue;
			}

			if(extrude != Exception)
			{
				extrude.apply();

				if(extrude.Activated)
				{
					extrude._zoneController._activatedElements.Add(extrude);
				}
				else
				{
					extrude._zoneController._activatedElements.Remove(extrude);
				}
			}
		}



		Exception = null;

		ResetRoutine();
	}

	void ResetRoutine()
	{
		StopCoroutine(extrudesSwitch);
		extrudesSwitch = ExtrudesAutoSwitch();
		StartCoroutine(extrudesSwitch);
	}
}
