using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyIdlePattern : MonoBehaviour
{
	public Transform[] patternSpots;

	[Header ("Staying Time")]
	public int minStayingTime;
	public int maxStayingTime;

	EnnemyDetection detect;

	IEnumerator idle;

	bool alreadyPlaying;


	private void Start()
	{
		detect = GetComponent<EnnemyDetection>();
		idle = IdleMove();
		StartCoroutine(idle);
	}

	private void Update()
	{
		if(detect.canDetect || detect.canAttack || detect.canEscape || detect.canMelee)
		{
			StopCoroutine(idle);
			idle = IdleMove();
			alreadyPlaying = false;
		}
		else
		{
			if(!alreadyPlaying)
			{
				StartCoroutine(idle);
			}
		}
	}

	IEnumerator IdleMove()
	{
		alreadyPlaying = true;
		while(!detect.canDetect || !detect.canAttack || !detect.canEscape || !detect.canMelee)
		{
			foreach(Transform spot in patternSpots)
			{
				float time = Random.Range(minStayingTime, maxStayingTime);
				yield return new WaitForSeconds(time);
				transform.position = spot.position;
			}
		}
	}

}
