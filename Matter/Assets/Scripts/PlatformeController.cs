using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformeController : MonoBehaviour
{
    public PlatfomeDetector detect;

    void Start()
    {
        detect = GetComponent<PlatfomeDetector>();
    }

    public void Detected()
    {
        Debug.Log("HelloCoquinouDu59");
    }
}
