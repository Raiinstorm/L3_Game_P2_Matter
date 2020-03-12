using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneriqueElement : MonoBehaviour
{
    public EnergyZoneDetector Detect;

    public float DistanceExtrude = 2.2f;
    public float TimerOfDesactived = 8f;
    public float SpeedExtrude = 15f;

    public bool m_activated;
    protected bool m_timer;
    protected float TimerOfDesactivedTemp;

    protected Vector3 init_pos;

    void Start()
    {
        Detect = GetComponent<EnergyZoneDetector>();
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
            Init();

        if (m_activated)
            apply();
    }

    public void Detected()
    {
        if (!m_activated)
        {
            m_activated = true;
            m_timer = true;
        }
    }

    public abstract void apply(float enable = 1.0f);
    public abstract ElementType GetElementType(); //Récuper le type de mon élément.


    public void transformY(float distance)
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, init_pos.y + distance, transform.position.z), Time.deltaTime * SpeedExtrude);
    }

    public void transformX(float distance)
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(init_pos.x + distance, transform.position.y, transform.position.z), Time.deltaTime * SpeedExtrude);
    }
}
