using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject cameraFollowObj; 
    public GameObject cameraObj; 
    public GameObject playerObj; 


    Vector3 FollowPos; // position
    
    public float cameraMoveSpeed = 120.0f; // speed move
    public float clampAngle = 80.0f; // angle displacement
    public float inputSensibility = 150.0f; // sensibility move camera
    public float camDistanceXToPlayer; // Distance with the player on Axis X
    public float camDistanceYToPlayer; // "" Axis Y
    public float camDistanceZToPlayer; // "" Axis Z
    public float mouseX;
    public float mouseY;
    public float finalInputX; //Add mouse + sticks
    public float finalInputZ;
    public float smoothX;
    public float smoothY;
    private float rotX = 0.0f; // rotation cam
    private float rotY = 0.0f;
        
    void Start()
    {
        Vector3 rotation = transform.localRotation.eulerAngles; //obtenir la rotation de l'objet camera dans l'espace sur lui même
        rotX = rotation.x;
        rotY = rotation.y;
        Cursor.lockState = CursorLockMode.Locked; //bloquer le curseur de la souris 
        Cursor.visible = false; // Rend le curseur de la souris invisible
    }


    void Update()
    {
        // mise en place de la rotation
        float inputX = Input.GetAxis("RightStickHorizontal"); // récupération du float du joystick en horizontal
        float inputZ = Input.GetAxis("RightStickVertical"); // "" vertical
        mouseX = Input.GetAxis("Mouse X"); // Pareil mais cette fois pour la souris 
        mouseY = Input.GetAxis("Mouse Y");

        // fusionner les entrées de la souris avec celle des sticks manettes
        finalInputX = inputX + mouseX;
        finalInputZ = inputZ + mouseY;

        //action de rotation de la caméra
        rotY += finalInputX * inputSensibility * Time.deltaTime;
        rotX += finalInputZ * inputSensibility * Time.deltaTime;

        //bloquer l'angle de rotation
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;

        //14
        //watch?v=LbDQHv9z-F0







    }
}
