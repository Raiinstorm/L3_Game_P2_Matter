using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianBase : MonoBehaviour
{
	public Material standardMaterial;
	public Material attackMaterial;
	MeshRenderer rendering;

	float distance;

	int hp = 1;

	public Transform player;
	BasePlayer playerScript;
	Transform thisTransform;

	public int maxCharge;
	public int charge;

	public bool chargeAttack;

	public bool attacking;

	bool invincible = true;

	bool alreadyPlaying;

    // Start is called before the first frame update
    void Start()
    {
		rendering = GetComponent<MeshRenderer>();
		rendering.material = standardMaterial;
		thisTransform = GetComponent<Transform>();
		playerScript = player.gameObject.GetComponent<BasePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
		distance = Vector3.Distance(thisTransform.position, player.position);

		if (distance >= playerScript.radiusEnnemy && !chargeAttack)
		{
			distance = Vector3.Distance(thisTransform.position, player.position);
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

		if(invincible)
		{
			rendering.material = standardMaterial;
		}
		else
		{
			rendering.material = attackMaterial;
		}

		if (!invincible && Input.GetKeyDown(KeyCode.E))
		{
			hp--;
		}

		if(hp <=0)
		{
			gameObject.SetActive(false);
		}
    }

	IEnumerator Charging()
	{
		alreadyPlaying = true;
		int time = Random.Range(1, 3);
		yield return new WaitForSeconds(time);
		if(distance <= playerScript.radiusEnnemy)
		{
			chargeAttack = true;
		}
		alreadyPlaying = false;
	}

	IEnumerator Attack()
	{
		//anim charge
		yield return new WaitForSeconds(0.8f);
		invincible = false;
		yield return new WaitForSeconds(0.5f);
		invincible = true;
		attacking = false;
		chargeAttack = false;
		charge = 0;
	}
}
