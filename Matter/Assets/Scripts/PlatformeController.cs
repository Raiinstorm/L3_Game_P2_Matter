using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformeController : MonoBehaviour
{
    public PlatfomeDetector Detect;

    public float DistanceExtrude;

    public bool ExtrudeGround;
    public bool ExtrudeWallLeft;
    public bool ExtrudeWallRight;
    public float TimerOfDesactived =8f;
    
    bool m_actived;
    bool m_timer;
    float TimerOfDesactivedTemp;
    Transform m_thisTransform;

    void Start()
    {
        Detect = GetComponent<PlatfomeDetector>();
        m_actived = false;
        TimerOfDesactivedTemp = TimerOfDesactived;
        m_timer = false;
    }

    void Update()
    {
        if (m_timer)
           TimerOfDesactivedTemp -= Time.deltaTime;

        if(ExtrudeGround && TimerOfDesactivedTemp <= 0 && m_actived == true)
        {
            m_actived = false;
            m_timer = false;
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, transform.position.y - DistanceExtrude, 0), Time.deltaTime);
            TimerOfDesactivedTemp = TimerOfDesactived;
        }
        else if((ExtrudeWallLeft || ExtrudeWallRight) && TimerOfDesactivedTemp <= 0 && m_actived == true)
        {
            m_actived = false;
            m_timer = false;
            transform.position = Vector3.Lerp(transform.position, new Vector3(-transform.position.x + DistanceExtrude, 0, 0), Time.deltaTime);
            TimerOfDesactivedTemp = TimerOfDesactived;
        }
    }

    public void Detected()
    {
        if (ExtrudeGround && !m_actived)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, transform.position.y + DistanceExtrude, 0),Time.deltaTime);
            m_actived = true;
            m_timer = true;
        }
        else if ((ExtrudeWallLeft || ExtrudeWallRight) && !m_actived)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(-transform.position.x - DistanceExtrude, 0, 0), Time.deltaTime);
            m_actived = true;
            m_timer = true;
        }
    }
}
