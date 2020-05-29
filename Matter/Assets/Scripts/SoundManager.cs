using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{

	static GameObject _oneShotGameObject;
	static AudioSource _oneShotAudioSource;


	public enum Sound
	{
		PlayerWalk,
		PlayerRun,

		AmbientInGameLoop,
		
		Energy,

		Extrude,
		ExtrudeEnd,
		BigExtrude,

		MusicLoop,
	}

	public static void PlayLoop(string name, Sound sound, float volume = 1f,bool bypassFilter = false)
	{
		GameObject loop = new GameObject(name);
		AudioSource sourceLoop = loop.AddComponent<AudioSource>();

		sourceLoop.loop = true;
		sourceLoop.volume = volume;
		sourceLoop.clip = GetAudioClip(sound);
		if (bypassFilter)
		{
			sourceLoop.bypassEffects = true;
		}

		SoundAssets.i.AudioLoops.Add(loop);

		sourceLoop.Play();
	}


	public static void PlaySound(Sound sound, float volume = 1f,bool bypassFilter = false) //not Spacialized
	{
		if (_oneShotGameObject == null)
		{
			_oneShotGameObject = new GameObject("OneShotSound");
			_oneShotAudioSource = _oneShotGameObject.AddComponent<AudioSource>();
		}

		_oneShotAudioSource.bypassEffects = false;
		if (bypassFilter)
		{
			_oneShotAudioSource.bypassEffects = true;
		}

		_oneShotAudioSource.PlayOneShot(GetAudioClip(sound), volume);
	}

	public static void PlaySoundSpacialized(string name,Sound sound, Vector3 position,float maxDistance = 50,float volume = 1f, bool loop = false, bool bypassFilter = false)
	{
		GameObject soundGameObject = new GameObject(name);
		soundGameObject.transform.position = position;
		AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
		audioSource.clip = GetAudioClip(sound);
		audioSource.maxDistance = maxDistance;
		audioSource.volume = volume;
		audioSource.spatialBlend = 1f;
		audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff,SoundAssets.i.SpacializedSoundCurve);
		audioSource.rolloffMode = AudioRolloffMode.Custom;
		audioSource.dopplerLevel = 0f;

		if(loop)
		{
			audioSource.loop = true;
			SoundAssets.i.AudioLoops.Add(soundGameObject);
		}
		if (bypassFilter)
		{
			audioSource.bypassEffects = true;
		}

		audioSource.Play();

		if(!loop)
		{
			Object.Destroy(soundGameObject, audioSource.clip.length);
		}
	}

	static AudioClip GetAudioClip(Sound sound)
	{
		List<AudioClip> audioClips = new List<AudioClip>();
		foreach(SoundAssets.SoundAudioClip soundAudioClip in SoundAssets.i.soundAudioClips)
		{
			if(soundAudioClip.sound == sound)
			{
				audioClips.Add(soundAudioClip.audioClip);
			}
		}
		return audioClips[Random.Range(0,audioClips.Count)];
	}


}
