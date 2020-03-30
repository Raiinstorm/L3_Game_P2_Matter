using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public float miniDistance = 0.2f;
    public float maxDistance = 2.9f;
    public float smooth = 5f;
    Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public float distance;

    void Awake ()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    void Update()
    {
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance); //Récupération de la position souhaité de la caméra
        RaycastHit hit;

       if(Physics.Linecast (transform.parent.position, desiredCameraPos, out hit))
        {
            distance = Mathf.Clamp(hit.distance* 0.9f, miniDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }

        transform.localPosition = Vector3.Lerp (transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);       
    }
}
