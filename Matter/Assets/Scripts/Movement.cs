using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController Controller;
    public GameObject Camera;
    public Transform GroundCheck;
    public LayerMask GroundMask;

    public float GroundDistance = 0.4f;
    public float Speed = 12f;
    public float Gravity = 9.81f;

    //test
    public float InputX;
    public float InputZ;
    public Vector3 desiredMoveDirection;
    public bool blockRotationPlayer;
    public Camera cam;

    Vector3 m_velocity;
    bool m_isGround;
    
    void Update()
    {
        Move();
        Jump();
        //InputMagnitude();

        PlayerRotation();
    }

    public virtual void Move()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        m_isGround = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        if (m_isGround && m_velocity.y < 0)
            m_velocity.y = -2f;

        //Récupérer les entrées pour les transformer en déplacement (deplacement en fonction du de la caméra)
        Vector3 move = Camera.transform.right * InputX + Camera.transform.forward * InputZ;

        Controller.Move(move * Speed * Time.deltaTime);
        m_velocity.y += Gravity * Time.deltaTime;
        Controller.Move(m_velocity * Time.deltaTime);
    }

    public virtual void Jump()
    {

    }

    void PlayerRotation()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * InputZ + right * InputX;

        if (blockRotationPlayer == false)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), 0.1f);
        }
    }
    
}
