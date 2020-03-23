﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOLD : Character
{
    enum Direction {Walk};
    
    public float MoveSpeed;
    public float JumpForce;
    public float GravityScale;
    public float RotateSpeed;
    public Transform Pivot;
    public GameObject PlayerModel;

    Vector3 m_moveDirection; 
    CharacterController m_controller;
    
    float m_inputX;
    float m_inputZ;
    void Start()
    {
        m_controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move(Direction.Walk);
    }


    Direction Move ( Direction dir)
    {
        m_inputX = Input.GetAxis("Horizontal");
        m_inputZ = Input.GetAxis("Vertical");

        if (dir == Direction.Walk)
        {
            float yStore = m_moveDirection.y;

            m_moveDirection = (transform.forward * m_inputZ) + (transform.right * m_inputX);
            m_moveDirection = m_moveDirection.normalized * MoveSpeed;
            m_moveDirection.y = yStore;

            if (m_controller.isGrounded)
            {
                m_moveDirection.y = 0f;
                if (Input.GetButtonDown("Jump"))
                    m_moveDirection.y = JumpForce;
            }

            m_moveDirection.y = m_moveDirection.y + (Physics.gravity.y * GravityScale * Time.deltaTime);
            m_controller.Move(m_moveDirection * Time.deltaTime);

            //Move the player in different directions based on camera look direction
            if (m_inputX != 0 || m_inputZ != 0)
            {
                transform.rotation = Quaternion.Euler(0f, Pivot.rotation.eulerAngles.y, 0f);
                Quaternion newRotation = Quaternion.LookRotation(new Vector3(m_moveDirection.x, 0f, m_moveDirection.z));
                PlayerModel.transform.rotation = Quaternion.Slerp(PlayerModel.transform.rotation, newRotation, RotateSpeed * Time.deltaTime);
            }
        }
        return dir;
    }


}
