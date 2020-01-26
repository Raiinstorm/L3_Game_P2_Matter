using UnityEngine;

public class PlatfomeDetector : MonoBehaviour
{
    public float Distance;
    public LayerMask PlatfomeMask;
    public GameObject cam;
    public GameObject Player;

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
        /*
        if (m_callPlateform && Detector())
            m_detect.Detected();
          */ 
    }

    // Permet de détected si le Raycast est en collision avec une Plateforme
    /*
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
    
    
    public bool Detector()
    {
        RaycastHit hit;
        if (Physics.BoxCast(cam.transform.position, cam.transform.position, transform.forward, out hit ,Quaternion.Euler(0, 0, 0),Distance, PlatfomeMask))
        {
            hit.transform.gameObject.TryGetComponent(out m_detect);
            Debug.Log("ok");
            return true;
        }
        return false;
    }
    */
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Plateforme"))
        {
            if (m_callPlateform)
            {
                other.transform.gameObject.TryGetComponent(out m_detect);
                m_detect.Detected();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(cam.transform.position, cam.transform.position + transform.forward * Distance);
        Gizmos.DrawWireCube(Player.transform.position, Player.transform.position* Distance);
    }
}
