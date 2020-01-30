using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

	bool alreadyPlaying = true;

	Transform place;

	int iIdle;

	NavMeshAgent agent;


	Vector3 direction;
	Vector3 saveDirection;


	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		detect = GetComponent<EnnemyDetection>();
		idle = IdleMove();
		StartCoroutine(idle);

		if (patternSpots.Length > 0)
		transform.position = patternSpots[0].position;
	}

	private void Update()
	{
		if (detect.canDetect || detect.canShoot || detect.canEscape || detect.canMelee)
		{
			StopCoroutine(idle);
			idle = IdleMove();
			alreadyPlaying = false;
		}
		else
		{
			if(!alreadyPlaying)
			{
				StartCoroutine(WaitBeforeIdleMove());
				agent.isStopped = true;
			}
		}
	}
	IEnumerator WaitBeforeIdleMove()
	{
		alreadyPlaying = true;
		yield return new WaitForSeconds(timeBeforePattern);
		StartCoroutine(idle);
	}

	IEnumerator IdleMove()
	{
		place = patternSpots[iIdle];
		agent.isStopped = false;
		agent.SetDestination(place.position);
		int time = Random.Range(minStayingTime, maxStayingTime + 1);
		yield return new WaitForSeconds(time);
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


}
