using UnityEngine;

public class PlatfomeDetector : MonoBehaviour
{
    public float Distance;
    public LayerMask PlatfomeMask;
    public GameObject cam;

    PlatformeController m_detect;
    bool m_callPlateform;

    private void Update()
    {
        /*
        if(Detector())
        {
            m_detect.Detected();
        }*/
        GetInput();
    }

    public void GetInput()
    {
        m_callPlateform = Input.GetButtonDown("CallPlateform");
        
        if (m_callPlateform)
            Debug.Log("Test");

        if (m_callPlateform && Detector())
            m_detect.Detected();
    } 
    
    // Permet de détected si le Raycast est en collision avec une Plateforme
    public bool Detector()
    {
        if (Physics.Linecast(cam.transform.position,cam.transform.position + transform.forward*Distance,out RaycastHit hitInfo,PlatfomeMask))
        {
            hitInfo.transform.gameObject.TryGetComponent(out m_detect);
            return true;
        }
        m_detect = null;
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(cam.transform.position, cam.transform.position + transform.forward * Distance);
    }
}
