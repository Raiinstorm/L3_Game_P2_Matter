using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController controller;
    public GameObject camera;

    public float speed = 12f;
    public float gravity = 9.81f;

    Vector3 velocity;
    
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Récupérer les entrées pour les transformer en déplacement (deplacement en fonction du de la caméra)
        Vector3 move = camera.transform.right * x + camera.transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
        velocity.y = gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


        //watch?v=_QajrabyTJc&t=913s
        //15
    }
}
