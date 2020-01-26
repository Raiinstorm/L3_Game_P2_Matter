using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyIdlePattern : MonoBehaviour
{
	public Transform[] patternSpots;
	EnnemyMovement ennemyMovement;

	[Header ("Staying Time")]
	public int minStayingTime;
	public int maxStayingTime;

	[Header("Time Before Idle Start")]
	public int timeBeforePattern;

	EnnemyDetection detect;

	IEnumerator idle;

	bool alreadyPlaying = true;

	Transform place;

	int iIdle;


	private void Start()
	{
		ennemyMovement = GetComponent<EnnemyMovement>();

		detect = GetComponent<EnnemyDetection>();
		idle = IdleMove();
		StartCoroutine(idle);
		transform.position = patternSpots[0].position;
	}

	private void Update()
	{
		if (detect.canDetect || detect.canShoot || detect.canEscape || detect.canMelee)
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
			ennemyMovement.Move(place);
		}
	}

	IEnumerator IdleMove()
	{
		place = patternSpots[iIdle];
		int time = Random.Range(minStayingTime, maxStayingTime + 1);
		yield return new WaitForSeconds(time);
		place = null;
		iIdle++;
		if (iIdle >= patternSpots.Length)
		{
			iIdle = 0;
		}
		ResetIdleMove();
	}

	void ResetIdleMove()
	{
		StopCoroutine(idle);
		idle = IdleMove();
		StartCoroutine(idle);
	}

	IEnumerator WaitBeforeIdleMove()
	{
		alreadyPlaying = true;
		yield return new WaitForSeconds(timeBeforePattern);
		StartCoroutine(idle);
	}
}
