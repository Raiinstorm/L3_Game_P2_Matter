using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyDetection : MonoBehaviour
{
	Transform ennemyTransform;
	public Transform targetLastPosition;

	public LayerMask collisionsMasks;
	bool detectionObstructed;

	[Header ("Ranges")]
	public float detectionRange;
	public float longAttackRange;
	public float escapeRange;
	public float meleeRange;

	[Header ("Assign Player")]
	public GameObject player;
	Transform playerTransform;
	Base playerBase;

	[HideInInspector]
	public bool canDetect;
	[HideInInspector]
	public bool canLongAttack;
	[HideInInspector]
	public bool canEscape;
	[HideInInspector]
	public bool canMelee;

	float distance;

	[Header ("Timing")]
	public float timeSearchingTarget;

	private void Start()
	{
		ennemyTransform = GetComponent<Transform>();
		playerTransform = player.GetComponent<Transform>();
		playerBase = player.GetComponent<Base>();
	}

	private void Update()
	{
		Detection();
	}

	void Detection()
	{
		distance = Vector3.Distance(playerTransform.position, ennemyTransform.position);
		
		if(Physics.Linecast(ennemyTransform.position,playerBase.detectionPoint.position, out RaycastHit hit, collisionsMasks))
		{
			detectionObstructed = true;
		}
		else
		{
			detectionObstructed = false;
		}

		if (distance < detectionRange && distance > longAttackRange && !detectionObstructed)
		{
			Desactivate();
			canDetect = true;
		}
		if (distance < longAttackRange && distance > escapeRange && !detectionObstructed)
		{
			Desactivate();
			canLongAttack = true;
		}
		if (distance < escapeRange && distance > meleeRange && !detectionObstructed)
		{
			Desactivate();
			canEscape = true;
		}
		if (distance < meleeRange && !detectionObstructed)
		{
			Desactivate();
			canMelee = true;
		}
		if(distance > detectionRange || detectionObstructed)
		{
			Desactivate();
		}
	}

	void Desactivate()
	{
		canDetect = false;
		canLongAttack = false;
		canEscape = false;
		canMelee = false;
	}

	private void OnDrawGizmosSelected()
	{
		if(ennemyTransform != null)
		Gizmos.DrawLine(ennemyTransform.position, playerBase.detectionPoint.position);
	}

	//To do : activer Calculing() et Detection() que lorsque le player est à proximité
}
