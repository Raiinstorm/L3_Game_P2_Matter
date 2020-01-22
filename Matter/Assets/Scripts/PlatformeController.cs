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
    bool m_actived;

    Transform m_thisTransform;

    void Start()
    {
        Detect = GetComponent<PlatfomeDetector>();
        m_actived = false;
    }

    public void Detected()
    {
        if (ExtrudeGround && !m_actived)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, transform.position.y + DistanceExtrude, 0), Time.deltaTime);
            m_actived = true;
        }
        else if (ExtrudeWallLeft || ExtrudeWallRight)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(-transform.position.x - DistanceExtrude, 0, 0), Time.deltaTime);
            m_actived = true;
        }
    }
}
