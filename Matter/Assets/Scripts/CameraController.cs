using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;
    public Transform Pivot;
    public Vector3 Offset;

    public bool UseOffsetValues;
    public bool InvertY;


    public float clampAngle = 80.0f;
    public float RotateSpeed;
    public float finalInputX; //Add mouse + sticks
    public float finalInputZ;
    public float MaxViewAngle;
    public float MinViewAngle;

    private float m_rotY;
    private float m_rotX;
    void Start()
    {
        if(!UseOffsetValues)
            Offset = Target.position - transform.position;

        Vector3 rotation = transform.localRotation.eulerAngles;
        m_rotX = rotation.x;
        m_rotY = rotation.y;

        Pivot.transform.position = Target.transform.position;
        //Pivot.transform.parent = Target.transform;
        Pivot.transform.parent = null;

        Cursor.lockState = CursorLockMode.Locked; //bloquer le curseur de la souris 
        Cursor.visible = false; // Rend le curseur de la souris invisible
    }

    void LateUpdate()
    {
        Pivot.transform.position = Target.transform.position;
        
        // mise en place de la rotation
        float inputX = Input.GetAxis("RightStickHorizontal") * RotateSpeed; // récupération du float du joystick en horizontal
        float inputZ = Input.GetAxis("RightStickVertical") * RotateSpeed;
        float mouseX = Input.GetAxis("Mouse X") * RotateSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * RotateSpeed;

        // fusionner les entrées de la souris avec celle des sticks manettes
        finalInputX = inputX + mouseX;
        finalInputZ = inputZ + mouseY;

        //action de rotation de la caméra
        m_rotY += finalInputX * Time.deltaTime;
        m_rotX += finalInputZ * Time.deltaTime;

        //bloquer l'angle de rotation
        m_rotX = Mathf.Clamp(m_rotX, -clampAngle, clampAngle);
        Quaternion localRotation = Quaternion.Euler(m_rotX, m_rotY, 0.0f);
        transform.rotation = localRotation;

        Pivot.Rotate(0, finalInputX, 0);
        //Pivot.Rotate(finalInputZ, 0, 0);
        if(InvertY)
            Pivot.Rotate(finalInputZ, 0, 0);
        else
            Pivot.Rotate(-finalInputZ, 0, 0);
        /*
        // Limit Camera move
        if (Pivot.rotation.eulerAngles.x > MaxViewAngle && Pivot.rotation.eulerAngles.x < 180f)
            Pivot.rotation = Quaternion.Euler(MaxViewAngle, 0, 0);

        if (Pivot.rotation.eulerAngles.x > 180f && Pivot.rotation.eulerAngles.x < 360f + MinViewAngle)
            Pivot.rotation = Quaternion.Euler(360f + MinViewAngle, 0, 0);
*/
        // Move the camera
        float desiredYAgnle = Pivot.eulerAngles.y;
        float desiredXAgnle = Pivot.eulerAngles.x;

        Quaternion rotation = Quaternion.Euler(desiredXAgnle, desiredYAgnle, 0);
        transform.position = Target.position - (rotation * Offset);

        //transform.position = Target.position - Offset;
        if(transform.position.y < Target.position.y)
            transform.position = new Vector3(transform.position.x, Target.position.y, transform.position.z);

        transform.LookAt(Target.transform);
    }

}
