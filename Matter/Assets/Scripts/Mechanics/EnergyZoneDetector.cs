using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyZoneDetector : MonoBehaviour
{
    public LayerMask FaultMask;
    public List<ZoneController> Faults = new List<ZoneController>();
    public Vector3 BoxRadius;
    [SerializeField] Vector3 _boxScale;
    [SerializeField] float _boxZPosition;
    [SerializeField] GameObject _cubeVisualizer;
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
                GetClosestGameObject(Faults).GetComponent<ZoneController>().ActivateElementOfType(InputMechanics.ReturnMode);
        }

        if (Input.GetButtonDown("MainMechanicCancel"))
            GetClosestGameObject(Faults).GetComponent<ZoneController>().Cancel();

        /*
        if (Input.GetButtonDown("InfuseEnergy"))
        {
            Debug.Log("insuffler de l'energy");
            GetClosestGameObject(Faults).ChangedModeActivated();
        }
        */
    }

#if UNITY_EDITOR
    void VisualizeBox()
    {
        _cubeVisualizer.transform.position = transform.position + transform.forward * _boxZPosition;
        _cubeVisualizer.transform.rotation = transform.rotation;
        _cubeVisualizer.transform.localScale = _boxScale;
    }// Permet de visualiser la box de detection en debug
#endif

    void Detect() // Permet de detecter les plateformes entrant dans la zone et de les ajouter à une liste
    {
        Faults.Clear();
        RaycastHit[] hits = Physics.BoxCastAll(transform.position + transform.forward * _boxZPosition, _boxScale, transform.forward, transform.rotation, Mathf.Infinity, FaultMask);

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
            //if (Fault.GetComponent<ZoneController>().CheckIfElementIsActive(InputMechanics.ReturnMode)))
            //    continue;

            //if(Fault.GetComponent<ZoneController>().ActivedZone)
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
