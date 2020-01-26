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

    Vector3 velocity;
    public bool isGrounded;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	public virtual void Move(Transform target)
    {

        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

		if (isGrounded)
		{
			rb.velocity = new Vector3(rb.velocity.x,0, rb.velocity.y);
		}
		else
		{
			rb.velocity -= new Vector3 (rb.velocity.x,Gravity * Time.deltaTime,rb.velocity.y);
		}

		Vector3 move = target.position - transform.position;

        rb.velocity = new Vector3(move.x * Speed*Time.fixedDeltaTime, rb.velocity.y,move.z*Speed*Time.fixedDeltaTime);
    }

	public virtual void Stop()
	{
		rb.velocity = new Vector3(0, 0, 0);
	}
    
}
