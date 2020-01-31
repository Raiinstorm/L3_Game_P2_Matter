using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guardian : MonoBehaviour
{
	EnnemyDetection detect;
	IsGrounded isGround;

	[Header ("Materials")]
	public Material standardMaterial;
	public Material vulnerableMaterial;
	public Material attackingMaterial;

	[Header ("Stats")]
	public int hp = 1;

	MeshRenderer rendering;

	float distance;

	public Transform player;
	Base playerScript;
	Transform thisTransform;

	public int maxCharge;

	bool chargeAttack;
	bool attacking;
	bool invincible = true;
	bool alreadyPlaying;

	[Header ("AttackRanges")]
	GameObject ondeDeChoc;
	public GameObject attackHitbox;

	NavMeshAgent agent;

	void Start()
    {
		agent = GetComponent<NavMeshAgent>();

		isGround = GetComponent<IsGrounded>();

		ondeDeChoc = new GameObject { name = "OndeDeChocRange" };
		ondeDeChoc.transform.position = transform.position;
		ondeDeChoc.transform.parent = transform;
		ondeDeChoc.DrawCircle(4, 0.2f);
		ondeDeChoc.SetActive(false);

		attackHitbox.SetActive(false);

		rendering = GetComponent<MeshRenderer>();
		rendering.material = standardMaterial;
		thisTransform = GetComponent<Transform>();
		playerScript = player.gameObject.GetComponent<Base>();
		detect = GetComponent<EnnemyDetection>();
	}

    // Update is called once per frame
    void Update()
    {
        if(detect.canDetect || detect.canEscape || detect.canMelee || detect.canShoot)
		{
			detect.targetLastPosition.position = player.position;

			distance = Vector3.Distance(thisTransform.position, player.position);

			if (distance >= playerScript.radiusEnnemy && !chargeAttack)
			{
				distance = Vector3.Distance(thisTransform.position, player.position);
				LookingTarget();

				agent.isStopped = false;
				agent.SetDestination(player.position);

			}
			if (distance <= playerScript.radiusEnnemy && !alreadyPlaying)
			{
				agent.isStopped = true;

				StartCoroutine(Charging());
			}

			if (!attacking && chargeAttack)
			{
				attacking = true;
				StartCoroutine(Attack());
			}

			if (invincible)
			{
				if(attacking)
				{
					rendering.material = attackingMaterial;
				}
				else
				{
					rendering.material = standardMaterial;
				}
			}
			else
			{
				rendering.material = vulnerableMaterial;
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
		int time = Random.Range(1, maxCharge+1);
		yield return new WaitForSeconds(time);
		if (distance <= playerScript.radiusEnnemy)
		{
			chargeAttack = true;
		}
		alreadyPlaying = false;
	}

	IEnumerator Attack()
	{
		int attackType = Random.Range(1, 3);
		if(attackType == 1) // onde de choc
		{
			Debug.Log("Onde de choc");
			//anim charge
			yield return new WaitForSeconds(1f);
			invincible = false;
			yield return new WaitForSeconds(0.5f);
			ondeDeChoc.SetActive(true);
			yield return new WaitForSeconds(1);
			invincible = true;
			attacking = false;
			chargeAttack = false;
			ondeDeChoc.SetActive(false);
		}
		if(attackType == 2) // normal
		{
			distance = Vector3.Distance(thisTransform.position, player.position);
			LookingTarget();
			Debug.Log("Attaque normale");
			//charge
			yield return new WaitForSeconds(1);
			attackHitbox.SetActive(true);
			yield return new WaitForSeconds(1);
			attacking = false;
			chargeAttack = false;
			attackHitbox.SetActive(false);
		}
		
	}

	void LookingTarget()
	{
		Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z); 
		transform.LookAt(targetPosition);
	}
}
