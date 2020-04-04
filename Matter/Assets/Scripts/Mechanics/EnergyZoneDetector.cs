﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(InputMechanics))]
public class EnergyZoneDetector : MonoBehaviour
{
    public LayerMask FaultMask;
    public List<ZoneController> Faults = new List<ZoneController>();
    public Vector3 BoxRadius;
    [SerializeField] Vector3 _boxScale;
    [SerializeField] float _boxZPosition;
    [SerializeField] GameObject _cubeVisualizer;
    [SerializeField] ZoneController _zoneController = null;
    [SerializeField] protected PlayerController _player;

    bool _activedZone = false;
    private InputMechanics _mechanics;

    private void Update()
    {
        GetInput();
#if UNITY_EDITOR
        VisualizeBox();
#endif
    }

    void Start()
    {
        InvokeRepeating("Detect", 0f, 0.5f);
        _mechanics = GetComponent<InputMechanics>();
    }
    void GetInput()
    {
        if (Input.GetButtonDown("MainMechanic"))
        {
            if (Faults.Count == 0)
                return;

            ZoneController zoneController = GetClosestGameObject(Faults, false);
            if (zoneController != null && zoneController.ActivedZone)
                zoneController.ActivateElementOfType(_mechanics.ReturnMode);
        }
        else if (Input.GetButtonDown("MainMechanicCancel"))
        {
            ZoneController zoneController = GetClosestGameObject(Faults, true);
            if (zoneController != null)
                zoneController.Cancel();
        }
        else if (Input.GetButtonDown("InfuseEnergy"))
        {
            Debug.Log("insuffler de l'energy");
            ZoneController zoneController = GetClosestGameObject(Faults, false);
            if (zoneController != null)
            {
                if (zoneController.ChangedModeActivated())
                    _player.InfuseEnergy(-1);
                else
                    _player.InfuseEnergy();
            }
        }
    }

#if UNITY_EDITOR
    void VisualizeBox()
    {
        _cubeVisualizer.transform.position = transform.position + transform.forward * _boxZPosition;
        _cubeVisualizer.transform.rotation = transform.rotation;
        _cubeVisualizer.transform.localScale = _boxScale;
    }// Permet de visualiser la box de detection en debug
#endif

    /// <summary>
    /// Permet de detecter les plateformes entrant dans la zone et de les ajouter à une liste
    /// </summary>
    void Detect()
    {
        Faults.Clear();
        RaycastHit[] hits = Physics.BoxCastAll(transform.position + transform.forward * _boxZPosition, _boxScale/2, transform.forward, transform.rotation, _boxScale.magnitude, FaultMask);

        foreach (var hit in hits)
            Faults.Add(hit.transform.gameObject.GetComponent<ZoneController>());
    }

    /// <summary>
    /// Permet de renvoyer la faille la plus proche
    /// </summary>
    ZoneController GetClosestGameObject(List<ZoneController> faults , bool filterActivatedElement)
    {
        ZoneController bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (ZoneController Fault in faults)
        {
            if (Fault.CheckIfElementIsActive(_mechanics.ReturnMode) != filterActivatedElement)
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
