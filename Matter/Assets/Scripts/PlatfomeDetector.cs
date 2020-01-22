using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatfomeDetector : MonoBehaviour
{
    public float Distance;
    public LayerMask PlatfomeMask;
    public GameObject cam;

    PlatformeController detect;

    private void Update()
    {
        if(Detector())
        {
            detect.Detected();
        }
    }
    
    
    // A Améliorer pour eviter plusieurs Getcomponent d'affiler.
    public bool Detector()
    {
        if (Physics.Linecast(cam.transform.position,cam.transform.position + transform.forward*Distance,out RaycastHit hitInfo,PlatfomeMask))
        {
            hitInfo.transform.gameObject.TryGetComponent(out detect);
            return true;
        }

        detect = null;
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(cam.transform.position, cam.transform.position + transform.forward * Distance);
    }
}
