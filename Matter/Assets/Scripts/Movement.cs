using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController Controller;
    public GameObject Camera;
    public Transform GroundCheck;
    public LayerMask GroundMask;

    public float GroundDistance = 0.4f;
    public float Speed = 12f;
    public float Gravity = -9.81f;

    //test
    public float InputX;
    public float InputZ;
    public Vector3 desiredMoveDirection;
    public bool blockRotationPlayer;
    public Camera cam;

    Vector3 m_velocity;
    bool m_isGround;

    public float allowPlayerRotation = 0.1f;

    public float JumpForce;

    void Update()
    {
        PlayerRotation();
        Move();
        Jump();


        if (Input.GetButtonDown("Reset"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        if (Input.GetButtonDown("Jump") && m_isGround)
        {
            m_velocity = new Vector3(0.0f, 9.0f, 0.0f);
        }
    }

    void PlayerRotation()
    {
        //Calculate the Input Magnitude
        float speedMagnitude;
        speedMagnitude = new Vector2(InputX, InputZ).sqrMagnitude;

        if (speedMagnitude > allowPlayerRotation)
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
}
