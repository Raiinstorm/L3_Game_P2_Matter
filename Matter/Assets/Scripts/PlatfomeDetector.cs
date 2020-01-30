using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PlatfomeDetector : MonoBehaviour
{
    public float Distance;
    public LayerMask PlatfomeMask;
    public GameObject cam;
    public GameObject Player;

    PlatformeController m_detect;
    bool m_callPlateform;
    public List<Collider> Plateformes = new List<Collider>();

    public Vector3 boxRadius;

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
        {
            Debug.Log("Test");
            Detector();
        }
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
    */
   /* 
    public bool Detector()
    {
        RaycastHit hit;
 
        if (Physics.BoxCast(Player.transform.position + new Vector3(0,0,boxRadius.z/3),boxRadius, transform.forward, out hit ,transform.rotation, PlatfomeMask))
        {
            hit.transform.gameObject.TryGetComponent(out m_detect);
            if(m_detect.m_actived == false)
            {
                Plateformes.Add(hit.collider); //Ajout de la plateform dans la list
            }
            Debug.Log("ok");
            return true;
        }
        for(int i = 0; i<= Plateformes.Count; i++)
        {
            if(!Physics.BoxCast(Player.transform.position + new Vector3(0, 0, boxRadius.z / 3), boxRadius, transform.forward, out hit, transform.rotation, PlatfomeMask))
            {
                Plateformes[i] 
            }
        }
        
        return false;
    }
*/
    public void Detector()
    {
        RaycastHit hit;
        float distance = 0f ;
        if (Physics.BoxCast(Player.transform.position + new Vector3(0, 0, boxRadius.z / 3), boxRadius, transform.forward, out hit, transform.rotation, PlatfomeMask))
        {
            hit.transform.gameObject.TryGetComponent(out m_detect);
            //if ( SI N'EST PAS ENCORE DANS LA LIST )
                Plateformes.Add(hit.collider);

            if(m_detect.m_actived == false && (Vector3.Distance(transform.position,m_detect.transform.position) < distance ))
            {
                m_detect.Detected();
            }
        }
    }

    public void Detector()
    {
        RaycastHit hit;

    }

    private void CheckList()
    {
        RaycastHit hit;
        if (!Physics.BoxCast(Player.transform.position + new Vector3(0, 0, boxRadius.z / 3), boxRadius, transform.forward, out hit, transform.rotation, PlatfomeMask))
        {
            foreach (var valeur in Plateformes)
            {
                if (valeur != hit.collider)
                {
                    RemoveList(valeur);
                }
            }
        }
    }

    private void RemoveList(Collider valeur)
    {
        Plateformes.Remove(valeur);
    }

    /*
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

public void OnTriggerEnter(Collider other)
{
   if (other.gameObject.CompareTag("Plateforme"))
   {
       Plateformes.Add(other);
   }
}

public void OnTriggerExit(Collider other)
{
   if (other.gameObject.CompareTag("Plateforme"))
   {
       Plateformes.Remove(other);
   }
}
*/
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(cam.transform.position, cam.transform.position + transform.forward * Distance);
        // Gizmos.DrawWireCube(Player.transform.position, Player.transform.position* Distance);
        Gizmos.DrawWireCube(Player.transform.position + new Vector3(0, 0, boxRadius.z / 3), boxRadius);

    }
}

public class Plateforme
{

}

