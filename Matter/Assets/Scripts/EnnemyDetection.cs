using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyDetection : MonoBehaviour
{
	Transform ennemyTransform;

	[Header ("Ranges")]
	public float detectionRange;
	public float longAttackRange;
	public float escapeRange;
	public float meleeRange;

	[Header ("Assign Player")]
	public GameObject player;
	Transform playerTransform;

	[HideInInspector]
	public bool canDetect;
	[HideInInspector]
	public bool canShoot;
	[HideInInspector]
	public bool canEscape;
	[HideInInspector]
	public bool canMelee;

	float distance;

	private void Start()
	{
		ennemyTransform = GetComponent<Transform>();
		playerTransform = player.GetComponent<Transform>();
	}

	private void Update()
	{
		Detection();
	}

	void Detection()
	{
		distance = Vector3.Distance(playerTransform.position, ennemyTransform.position);

		if (distance < detectionRange && distance > longAttackRange)
		{
			Desactivate();
			canDetect = true;
		}
		if (distance < longAttackRange && distance > escapeRange)
		{
			Desactivate();
			canShoot = true;
		}
		if (distance < escapeRange && distance > meleeRange)
		{
			Desactivate();
			canEscape = true;
		}
		if (distance < meleeRange)
		{
			Desactivate();
			canMelee = true;
		}
		if(distance > detectionRange)
		{
			Desactivate();
		}
	}

	void Desactivate()
	{
		canDetect = false;
		canShoot = false;
		canEscape = false;
		canMelee = false;
	}

	//To do : activer Calculing() et Detection() que lorsque le player est à proximité
}
