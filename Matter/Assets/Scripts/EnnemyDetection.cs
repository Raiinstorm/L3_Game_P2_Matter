using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyDetection : MonoBehaviour
{
	Transform ennemyTransform;

	[Header ("Ranges")]
	public float detectionRange;
	public float attackingRange;
	public float escapeRange;
	public float meleeRange;

	[Header ("Assign Player")]
	public GameObject player;
	Transform playerTransform;

	bool canDetect;
	bool canAttack;
	bool canEscape;
	bool canMelee;
	bool enableAction;

	float distance;

	private void Start()
	{
		ennemyTransform = GetComponent<Transform>();
		playerTransform = player.GetComponent<Transform>();
	}

	private void Update()
	{
		if (player != null)
		{
			Detection();
		}

		if (canDetect)
		{
			CanDetect();
		}
		if (canAttack)
		{
			CanAttack();
		}
		if (canEscape)
		{
			CanEscape();
		}
		if (canMelee)
		{
			CanMelee();
		}
	}

	void Detection()
	{
		distance = Vector3.Distance(playerTransform.position, ennemyTransform.position);

		if (distance < detectionRange && distance > attackingRange)
		{
			Desactivate();
			canDetect = true;
			enableAction = true;
		}
		if (distance < attackingRange && distance > escapeRange)
		{
			Desactivate();
			canAttack = true;
			enableAction = true;
		}
		if (distance < escapeRange && distance > meleeRange)
		{
			Desactivate();
			canEscape = true;
			enableAction = true;
		}
		if (distance < meleeRange)
		{
			Desactivate();
			canMelee = true;
			enableAction = true;
		}
		if(distance > detectionRange)
		{
			Desactivate();
		}
	}

	void Desactivate()
	{
		canDetect = false;
		canAttack = false;
		canEscape = false;
		canMelee = false;
	}

	void CanDetect()
	{
		Debug.Log("detecte le joueur");
	}

	void CanAttack()
	{
		Debug.Log("peut attaquer le joueur");
	}

	void CanEscape()
	{
		Debug.Log("fuis le joueur");
	}

	void CanMelee()
	{
		Debug.Log("Attaque de mélée au joueur");
	}

	//To do : activer Calculing() et Detection() que lorsque le player est à proximité
}
