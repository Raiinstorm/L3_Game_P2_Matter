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

	[Header("Random Movement")]
	public float maxRandomDistance;
	float maxRandomX, maxRandomZ;




	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		detect = GetComponent<EnnemyDetection>();

		if (patternSpots.Length > 0)
		transform.position = patternSpots[0].position;

		maxRandomX = transform.position.x + maxRandomDistance;
		maxRandomZ = transform.position.z + maxRandomDistance;

		Debug.Log("maxX " + maxRandomX);
		Debug.Log("maxZ " + maxRandomZ);

		idle = IdleMove();
		StartCoroutine(idle);

		var circle1 = new GameObject { name = "Slt" };
		circle1.transform.position = transform.position;
		circle1.DrawCircle(maxRandomDistance, .1f);
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
				StartCoroutine(SearchTarget(detect.targetLastPosition));
			}
		}
	}

	IEnumerator SearchTarget(Transform lastTargetPosition)
	{
		alreadyPlaying = true;

		if(lastTargetPosition.position != transform.position)
		{
			agent.SetDestination(lastTargetPosition.position);
			yield return new WaitForSeconds(detect.timeSearchingTarget);
			lastTargetPosition.position = transform.position;
		}

		StartCoroutine(WaitBeforeIdleMove());
	}
	IEnumerator WaitBeforeIdleMove()
	{
		agent.isStopped = true;
		yield return new WaitForSeconds(timeBeforePattern);
		StartCoroutine(idle);
	}

	IEnumerator IdleMove()
	{
		agent.isStopped = false;
		int time = Random.Range(minStayingTime, maxStayingTime + 1);

		if (patternSpots.Length != 0)
		{
			place = patternSpots[iIdle];
			agent.SetDestination(place.position);
			yield return new WaitForSeconds(time);
			iIdle++;
			if (iIdle >= patternSpots.Length)
			{
				iIdle = 0;
			}
		}
		else
		{
			float xRandom = Random.Range(-10, 11);
			float zRandom = Random.Range(-10, 11);
			Vector3 randomDirection = new Vector3(transform.position.x + xRandom, transform.position.y, transform.position.z + zRandom);
			Debug.Log("old " + randomDirection);

			if (randomDirection.x > maxRandomX)
				randomDirection.x = maxRandomX;
			else if (randomDirection.x < -maxRandomX)
				randomDirection.x = -maxRandomX;

			if(randomDirection.z > maxRandomZ)
				randomDirection.z = maxRandomZ;
			else if (randomDirection.z < -maxRandomZ)
				randomDirection.z = -maxRandomZ;

			Debug.Log("x " + xRandom);
			Debug.Log("z " + zRandom);

			agent.SetDestination(randomDirection);
			Debug.Log("new "+randomDirection);
			yield return new WaitForSeconds(time);
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
