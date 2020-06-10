using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{

	static GameObject _oneShotGameObject;
	static AudioSource _oneShotAudioSource;

	public static float MusicIntensity = 1;
	public static float SoundIntensity = 1;


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
		TitleMusic,

		UIAppear,
		UIMove,
		UIDisappear,
		UIValidate,
		UICancel,

		RockSliding,
		DustFalling,

		PlayerJump,
		PlayerLand,
		PlayerPropulsed,

		PlayerPower,
		Genial,
		feetBack,

		TitleMusicFirst,
		Button,
		EarthQuake,
	}

	public static void PlayLoop(string name, Sound sound, float volume = 1f, bool bypassFilter = false, bool isMusic = false)
	{
		GameObject loop = new GameObject(name);
		AudioSource sourceLoop = loop.AddComponent<AudioSource>();

		float volumeOptions;

		if (isMusic)
		{
			volumeOptions = MusicIntensity;
		}
		else
		{
			volumeOptions = SoundIntensity;
		}

		sourceLoop.loop = true;
		sourceLoop.volume = volume * volumeOptions;
		sourceLoop.clip = GetAudioClip(sound);
		if (bypassFilter)
		{
			sourceLoop.bypassEffects = true;
		}

		SoundAssets.i.AudioLoops.Add(loop);

		sourceLoop.Play();
	}


	public static void PlaySound(Sound sound, float volume = 1f,bool bypassFilter = false,float pitch = 1,bool isMusic = false) //not Spacialized
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

		float volumeOptions;

		if(isMusic)
		{
			volumeOptions = MusicIntensity;
		}
		else
		{
			volumeOptions = SoundIntensity;
		}

		_oneShotAudioSource.PlayOneShot(GetAudioClip(sound), volume * volumeOptions);
	}

	public static void PlaySoundSpacialized(string name,Sound sound, Vector3 position,float maxDistance = 50,float volume = 1f, bool loop = false, bool bypassFilter = false, bool objectParent = false, Transform parent = null, AudioClip audioClip = null)
	{
		GameObject soundGameObject = new GameObject(name);
		soundGameObject.transform.position = position;

		if(objectParent && parent != null)
		{
			soundGameObject.transform.parent = parent;
		}

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

	public static void PlaySoundSpacialized(string name, AudioClip audioClip, Vector3 position, float maxDistance = 50, float volume = 1f, bool loop = false, bool bypassFilter = false, bool objectParent = false, Transform parent = null)
	{
		GameObject soundGameObject = new GameObject(name);
		soundGameObject.transform.position = position;

		if (objectParent && parent != null)
		{
			soundGameObject.transform.parent = parent;
		}

		AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();


		audioSource.clip = audioClip;
		audioSource.maxDistance = maxDistance;
		audioSource.volume = volume;
		audioSource.spatialBlend = 1f;
		audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, SoundAssets.i.SpacializedSoundCurve);
		audioSource.rolloffMode = AudioRolloffMode.Custom;
		audioSource.dopplerLevel = 0f;

		if (loop)
		{
			audioSource.loop = true;
			SoundAssets.i.AudioLoops.Add(soundGameObject);
		}
		if (bypassFilter)
		{
			audioSource.bypassEffects = true;
		}

		audioSource.Play();

		if (!loop)
		{
			Object.Destroy(soundGameObject, audioSource.clip.length);
		}
	}

	public static AudioClip GetAudioClip(Sound sound)
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
