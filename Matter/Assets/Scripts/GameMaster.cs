using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMaster : MonoBehaviour
{
	public GenericElement Exception;

	[SerializeField] float _volume = 1;
	[SerializeField] float _minWaitForSounds = 1;
	[SerializeField] float _maxWaitForSounds = 100;

	[HideInInspector] public bool ResetRotation;

	public List<ZoneController> Faults = new List<ZoneController>(); 
	[HideInInspector] public ZoneController FaultSelected;

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

	IEnumerator _extrudesSwitch;
	IEnumerator _rockAndDust;

	public List<GenericElement> Elements;

	public GameObject TrashCan;

	void Start()
	{
		if(AutoSwitchExtrudes.Count != 0)
		{
			_extrudesSwitch = ExtrudesAutoSwitch();
			StartCoroutine(_extrudesSwitch);
		}
	}

	public void PlaySounds()
	{
		StartCoroutine(PlayLoops());
		RockAndDust();
	}

	public void Hello()
	{
		//Debug.Log("hello");
	}

	IEnumerator PlayLoops()
	{
		yield return new WaitForSeconds(1);

		SoundManager.PlayLoop("Ambient", SoundManager.Sound.AmbientInGameLoop,_volume,true);
		SoundManager.PlayLoop("Music", SoundManager.Sound.MusicLoop,_volume,true);
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

		ResetRoutineAutoSwitch();
	}

	void ResetRoutineAutoSwitch()
	{
		StopCoroutine(_extrudesSwitch);
		_extrudesSwitch = ExtrudesAutoSwitch();
		StartCoroutine(_extrudesSwitch);
	}

	void RockAndDust()
	{
		float waitTime1 = Random.Range(_minWaitForSounds, _maxWaitForSounds);
		float waitTime2 = Random.Range(_minWaitForSounds, _maxWaitForSounds);
		float whoFirst = Random.Range(1, 2);

		_rockAndDust = RockAndDustRoutine(waitTime1,waitTime2,whoFirst);
		StartCoroutine(_rockAndDust);
	}

	IEnumerator RockAndDustRoutine(float waitTime1, float waitTime2, float whoFirst)
	{
		//Debug.Log("waitTime1 :" + waitTime1);
		//Debug.Log("waitTime2 :" + waitTime2);

		yield return new WaitForSeconds(waitTime1);
		//Debug.Log("1");

		if(whoFirst == 1)
		SoundManager.PlaySound(SoundManager.Sound.RockSliding, _volume/2);
		else
		SoundManager.PlaySound(SoundManager.Sound.DustFalling, _volume / 2);

		yield return new WaitForSeconds(waitTime2);
		//Debug.Log("2");

		if (whoFirst == 1)
			SoundManager.PlaySound(SoundManager.Sound.DustFalling, _volume / 2);
		else
			SoundManager.PlaySound(SoundManager.Sound.RockSliding, _volume / 2);

		RockAndDust();
	}

	public void CheckingFaultSelected()
	{
		foreach (ZoneController fault in Faults)
		{
			if (fault == FaultSelected)
			{
				continue;
			}
			else
			{
				fault._animator.SetBool("activation", false);
			}
		}
	}
}
