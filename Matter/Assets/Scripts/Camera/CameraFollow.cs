﻿using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject cameraFollowObj; 
    public GameObject cameraObj; 

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
    [HideInInspector] public float rotX = 0.0f; // rotation cam
    [HideInInspector] public float rotY = 0.0f;
        
    void Start()
    {
        Vector3 rotation = transform.localRotation.eulerAngles; //obtenir la rotation de l'objet camera dans l'espace sur lui même
        rotX = rotation.x;
        rotY = rotation.y;
        //Cursor.lockState = CursorLockMode.Locked; //bloquer le curseur de la souris 
        //Cursor.visible = false; // Rend le curseur de la souris invisible
    }

    void Update()
    {
        // mise en place de la rotation
        float inputX = Input.GetAxis("RightStickHorizontal");
        float inputZ = Input.GetAxis("RightStickVertical"); 
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        // fusionner les entrées de la souris avec celle des sticks manettes
        finalInputX = inputX + mouseX;
        finalInputZ = inputZ + mouseY;

        //action de rotation de la caméra
        rotY += finalInputX * inputSensibility * Time.deltaTime;
        rotX += finalInputZ * inputSensibility * Time.deltaTime;

		//clamp Rotation Y
		if (rotY <= 0)
		{
			rotY = 360;
		}
		else if (rotY >= 360)
		{
			rotY = 0 ;
		}

        //bloquer l'angle de rotation
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;

		//Debug.Log("rotX : " + rotX + " | rotY : " + rotY);
    }

    void LateUpdate()
    {
        CameraUpdater ();    
    }
    
    void CameraUpdater()
    {
        // Definition du target (la cible) à suivre
        Transform target = cameraFollowObj.transform;

        //Deplacer le gameObject vers le target
        float step = cameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
} 
