using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlatformeController : MonoBehaviour
{
    public PlatformsDetector Detect;

    public float DistanceExtrude;

    public bool ExtrudeGround;
    public bool ExtrudeWallLeft;
    public bool ExtrudeWallRight;
    public float TimerOfDesactived = 8f;

    public bool m_activated;
    protected bool m_timer;
    protected float TimerOfDesactivedTemp;
    Transform m_thisTransform;
    protected Vector3 init_pos;

    void Start()
    {
        Detect = GetComponent<PlatformsDetector>();
        init_pos = transform.position;
        Init();
    }

    public void Init()
    {
        m_activated = false;
        m_timer = false;
        TimerOfDesactivedTemp = TimerOfDesactived;
    }

    public virtual void Update()
    {
        if (m_timer)
            TimerOfDesactivedTemp -= Time.deltaTime;

        if (!m_activated && transform.position != init_pos)
            apply(0);

        if (TimerOfDesactivedTemp <= 0 && m_activated)
        {
            Init();
        }

        if ( m_activated )
        {
            apply();
        }
        
    }

    public void Detected()
    {
        if  ( !m_activated)
        {
            m_activated = true;
            m_timer = true;
        }
    }

    public abstract void apply(float enable = 1.0f);
    

    public void transformY(float distance)
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, init_pos.y + distance, transform.position.z), Time.deltaTime);
    }

    public void transformX(float distance)
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(init_pos.x + distance, transform.position.y, transform.position.z), Time.deltaTime);
    }
}