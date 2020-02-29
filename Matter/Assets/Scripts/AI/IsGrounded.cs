using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGrounded : MonoBehaviour
{
    public Transform GroundCheck;
    public LayerMask GroundMask;

    public float GroundDistance = 0.4f;
    public float Speed = 12f;
    public float Gravity = 9.81f;

    public bool isGrounded;

	private void Start()
	{

	}

	private void FixedUpdate()
	{
		isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);
	}

}
