using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyZoneDetector : MonoBehaviour
{
    public LayerMask FaultMask;

    public List<ZoneController> Faults = new List<ZoneController>();

    public Vector3 boxRadius;

    [SerializeField] Vector3 boxScale;
    [SerializeField] float boxZPosition;
    [SerializeField] GameObject CubeVisualizer;
    [SerializeField] ZoneController _zoneController = null;

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
                //GetClosestGameObject(Faults).GetComponent<EnergyZoneController>().Actived(InputMechanics.ReturnMode);
                GetClosestGameObject(Faults).GetComponent<ZoneController>().ActivateElementOfType(InputMechanics.ReturnMode);
        }

        if (Input.GetButtonDown("MainMechanicCancel"))
        {
            GetClosestGameObject(Faults).GetComponent<ZoneController>().Cancel();
            //_zoneController.Cancel();
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
        RaycastHit[] hits = Physics.BoxCastAll(transform.position + transform.forward * boxZPosition, boxScale, transform.forward, transform.rotation, Mathf.Infinity, FaultMask);

        foreach (var hit in hits)
            Faults.Add(hit.transform.gameObject.GetComponent<ZoneController>());
    }

    ZoneController GetClosestGameObject(List<ZoneController> Faults) // renvoi la faille la plus proche
    {
        ZoneController bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (ZoneController Fault in Faults)
        {
            //if (Fault.GetComponent<GeneriqueElement>().m_activated)
            // continue;

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
