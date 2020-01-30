using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyMovement : MonoBehaviour
{
    public Transform GroundCheck;
    public LayerMask GroundMask;

	Rigidbody rb;

    public float GroundDistance = 0.4f;
    public float Speed = 12f;
    public float Gravity = 9.81f;

    public bool isGrounded;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

		if (!isGrounded)
		{
			rb.velocity -= new Vector3(rb.velocity.x, Gravity * Time.deltaTime, rb.velocity.y);
		}
	}

}
