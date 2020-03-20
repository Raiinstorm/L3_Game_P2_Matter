using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyZoneDetector : MonoBehaviour
{
    public LayerMask PlatformMask;

    GeneriqueElement m_detect;

    public List<GameObject> Faults = new List<GameObject>();

    public Vector3 boxRadius;

    [SerializeField] Vector3 boxScale;
    [SerializeField] float boxZPosition;
    [SerializeField] GameObject CubeVisualizer;

    private void Update()
    {
        GetInput();
#if UNITY_EDITOR
        VisualizeBox();
#endif
    }

    void GetInput()
    {
        if (Input.GetButtonDown("MainMechanic"))
        {
            Detect();
            if (Faults.Count == 0)
                return;

            if (GetClosestGameObject(Faults) != null)
                GetClosestGameObject(Faults).GetComponent<GeneriqueElement>().Detected();
        }

    }

#if UNITY_EDITOR
    void VisualizeBox()
    {
        CubeVisualizer.transform.position = transform.position + transform.forward * boxZPosition;
        CubeVisualizer.transform.rotation = transform.rotation;
        CubeVisualizer.transform.localScale = boxScale;
    }// Permet de visualiser la box de detection en debug
#endif

    void Detect() // Permet de detecter les plateformes entrant dans la zone et de les ajouter à une liste
    {
        Faults.Clear();
        RaycastHit[] hits = Physics.BoxCastAll(transform.position + transform.forward * boxZPosition, boxScale, transform.forward, transform.rotation, Mathf.Infinity, PlatformMask);

        foreach (var hit in hits)
            Faults.Add(hit.transform.gameObject);
    }

    GameObject GetClosestGameObject(List<GameObject> platforms) // renvoi la plateforme la plus proche
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject Fault in Faults)
        {
            if (Fault.GetComponent<GeneriqueElement>().m_activated)
                continue;

            Vector3 directionToTarget = Fault.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = Fault;
            }
        }

        return bestTarget;
    }
}
