using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyIdlePattern : MonoBehaviour
{
	public Transform[] patternSpots;

	[Header ("Staying Time")]
	public int minStayingTime;
	public int maxStayingTime;

	[Header("Time Before Idle Start")]
	public int timeBeforePattern;

	EnnemyDetection detect;

	IEnumerator idle;

	bool alreadyPlaying;

	Transform place;

	int iIdle;


	private void Start()
	{
		detect = GetComponent<EnnemyDetection>();
		idle = IdleMove();
		StartCoroutine(idle);
	}

	private void Update()
	{
		if (detect.canDetect || detect.canAttack || detect.canEscape || detect.canMelee)
		{
			StopCoroutine(idle);
			idle = IdleMove();
			alreadyPlaying = false;
			place = null;
		}
		else
		{
			if(!alreadyPlaying)
			{
				StartCoroutine(WaitBeforeIdleMove());
			}
		}


		if (place != null)
		{
			transform.position = Vector3.Lerp(transform.position, place.position, Time.deltaTime);
		}
	}

	IEnumerator IdleMove()
	{
		while(!detect.canDetect || !detect.canAttack || !detect.canEscape || !detect.canMelee)
		{
			place = patternSpots[iIdle];
			int time = Random.Range(minStayingTime, maxStayingTime+1);
			yield return new WaitForSeconds(time);
			place = null;
			iIdle++;
			if(iIdle >= patternSpots.Length)
			{
				iIdle = 0;
			}
		}
	}
	IEnumerator WaitBeforeIdleMove()
	{
		alreadyPlaying = true;
		yield return new WaitForSeconds(timeBeforePattern);
		StartCoroutine(idle);
	}

}
