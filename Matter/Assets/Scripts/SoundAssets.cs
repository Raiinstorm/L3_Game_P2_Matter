using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAssets : MonoBehaviour
{

	static SoundAssets _i;

	public AnimationCurve SpacializedSoundCurve;


	public static SoundAssets i
	{ get
		{
			if(_i == null)
			{
				_i = Instantiate(Resources.Load<SoundAssets>("SoundAssets"));
			}
			return _i;
		}
	}

	public List<GameObject> AudioLoops;

	public string Hello()
	{
		return "Hello World";
	}
	
	[System.Serializable]
	public class SoundAudioClip
	{
		public SoundManager.Sound sound;
		public AudioClip audioClip;
	}

	public SoundAudioClip[] soundAudioClips;

}
