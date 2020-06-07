using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventSoundOverlap : MonoBehaviour
{
	//à coller avec le GameMaster

	List<GameObject> _elementsToRemove = new List<GameObject>();
	[HideInInspector] public float distance;

	[HideInInspector] public AudioSource audioSource;
    void Start()
    {
		audioSource = GetComponent<AudioSource>();

		PreventOverlap();
    }

	void PreventOverlap()
	{
		foreach (GameObject sound in SoundAssets.i.AudioLoops)
		{
			if (!sound.TryGetComponent<PreventSoundOverlap>(out PreventSoundOverlap soundToCheck))
				continue;
			else
			{
				if ((Vector3.Distance(transform.position, soundToCheck.transform.position) <= (distance + soundToCheck.distance)) && audioSource.clip == soundToCheck.audioSource.clip)
				{
					_elementsToRemove.Add(soundToCheck.gameObject);

					Vector3 newPos = (transform.position + soundToCheck.transform.position) / 2;

					//SoundManager.PlaySoundSpacialized(gameObject.name, audioSource.clip, newPos, (distance + soundToCheck.distance) / 2, (audioSource.volume + soundToCheck.audioSource.volume) / 2, true);
				}
			}
		}
		if (_elementsToRemove.Count != 0)
		{
			foreach (GameObject element in _elementsToRemove)
			{
				SoundAssets.i.AudioLoops.Remove(element);
				if (element != this)
				{
					Debug.Log("kill");
					_elementsToRemove.Remove(element);
					element.SetActive(false);
				}
			}
			gameObject.SetActive(false);

		}

	}

}
