using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardian : MonoBehaviour
{
	EnnemyDetection detect;

	[Header ("Materials")]
	public Material standardMaterial;
	public Material vulnerableMaterial;
	public Material attackingMaterial;

	[Header ("Stats")]
	public int hp = 1;

	MeshRenderer rendering;

	float distance;


	public Transform player;
	BasePlayer playerScript;
	Transform thisTransform;

	public int maxCharge;

	bool chargeAttack;
	bool attacking;
	bool invincible = true;
	bool alreadyPlaying;

	void Start()
    {
		rendering = GetComponent<MeshRenderer>();
		rendering.material = standardMaterial;
		thisTransform = GetComponent<Transform>();
		playerScript = player.gameObject.GetComponent<BasePlayer>();
		detect = GetComponent<EnnemyDetection>();
	}

    // Update is called once per frame
    void Update()
    {
        if(detect.canDetect)
		{
			distance = Vector3.Distance(thisTransform.position, player.position);

			if (distance >= playerScript.radiusEnnemy && !chargeAttack)
			{
				distance = Vector3.Distance(thisTransform.position, player.position);
				//Quaternion target = 
				//transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * .2f);
				thisTransform.position = Vector3.Lerp(thisTransform.position, player.position, Time.deltaTime); //To do : mettre une distance minimale entre l'ennemy et le player
			}
			if (distance <= playerScript.radiusEnnemy && !alreadyPlaying)
			{
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
			//anim charge
			yield return new WaitForSeconds(1f);
			invincible = false;
			yield return new WaitForSeconds(0.5f);
			invincible = true;
			attacking = false;
			chargeAttack = false;
		}
		if(attackType == 2) // normal
		{
			//charge
			yield return new WaitForSeconds(1);
			attacking = false;
			chargeAttack = false;
		}
		
	}
}
