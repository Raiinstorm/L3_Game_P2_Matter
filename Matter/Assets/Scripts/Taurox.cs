using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Taurox : MonoBehaviour
{
	EnnemyDetection detect;
	IsGrounded isGround;
	Rigidbody rb;

	[Header("Materials")]
	public Material standardMaterial;
	public Material attackingMaterial;

	[Header("Stats")]
	public int hp = 1;
	public float chargeSpeed;

	MeshRenderer rendering;

	float distance;

	public Transform player;
	Base playerScript;
	Transform thisTransform;

	public int maxCharge;

	bool chargeAttack;
	bool attacking;
	bool invincible;
	bool alreadyPlaying;

	Vector3 direction;

	NavMeshAgent agent;


	[Range(0, 0.3f)]
	public float dampFactor;
	float smoothMove;
	float refMove;

	bool stopAttack;
	bool attackPreparing;
	float moving;

	IEnumerator attack;

	void Start()
	{
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		isGround = GetComponent<IsGrounded>();
		rb = GetComponent<Rigidbody>();

		rendering = GetComponent<MeshRenderer>();
		rendering.material = standardMaterial;
		thisTransform = GetComponent<Transform>();
		playerScript = player.gameObject.GetComponent<Base>();
		detect = GetComponent<EnnemyDetection>();

		direction = player.position - thisTransform.position;
		attack = Attack(direction);
	}

	// Update is called once per frame
	void Update()
	{
		if (detect.canDetect || detect.canLongAttack)
		{
			detect.targetLastPosition.position = player.position;

			distance = Vector3.Distance(thisTransform.position, player.position);

			if (!chargeAttack)
			{
				distance = Vector3.Distance(thisTransform.position, player.position);
				LookingTarget();

				agent.isStopped = false;
				agent.SetDestination(player.position);

			}
			if (detect.canLongAttack && !alreadyPlaying)
			{
				StartCoroutine(Charging());
			}

			if (!attacking && chargeAttack)
			{
				attacking = true;
				ChargingAttack();
			}

			if (attacking)
			{
				rendering.material = attackingMaterial;
			}
			else
			{
				rendering.material = standardMaterial;
			}

			if (!invincible && Input.GetKeyDown(KeyCode.E))
			{
				hp--;
			}

			if (hp <= 0)
			{
				gameObject.SetActive(false);
			}
		}
	}

	IEnumerator Charging()
	{
		alreadyPlaying = true;
		int time = Random.Range(1, maxCharge + 1);
		yield return new WaitForSeconds(time);
		if (detect.canLongAttack)
		{
			chargeAttack = true;
		}
		alreadyPlaying = false;
	}

	void ChargingAttack()
	{
		agent.isStopped = true;
		LookingTarget();
		invincible = true;
		stopAttack = false;
		moving = 1;
		attacking = true;

		direction = (player.position - thisTransform.position) * chargeSpeed * Time.deltaTime;
		attackPreparing = false;

		ResetAttackUpdate();

	}

	IEnumerator Attack(Vector3 direction)
	{
		if(!attackPreparing)
		{
			attackPreparing = true;
			yield return new WaitForSeconds(3);
			StartCoroutine(CooldownAttack());
		}

		yield return new WaitForEndOfFrame();

		//smoothMove = Mathf.SmoothDamp(smoothMove, moving, ref refMove, dampFactor);

		Vector3 orientation = (thisTransform.position - player.position)/10;
		rb.velocity = direction.normalized * chargeSpeed;

		if (stopAttack)
		{
			rb.velocity = new Vector3(0, transform.position.y, 0);
		}

		if(rb.velocity.magnitude > 1 && !stopAttack)
		{
			ResetAttackUpdate();
		}
		else
		{
			smoothMove = 0;
			attacking = false;
			chargeAttack = false;
			invincible = false;
		}
	}

	IEnumerator CooldownAttack()
	{
		yield return new WaitForSeconds(5);
		stopAttack = true;
	}

	void ResetAttackUpdate()
	{
		StopCoroutine(attack);
		attack = Attack(direction);
		StartCoroutine(attack);
	}

	void LookingTarget()
	{
		Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
		transform.LookAt(targetPosition);
	}
}
