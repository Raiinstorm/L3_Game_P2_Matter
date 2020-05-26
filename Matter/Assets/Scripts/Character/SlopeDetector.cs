using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeDetector : MonoBehaviour
{
    private Vector3 _underPlayerPoint;
    private Vector3 _underPlayerNormal;
    private Collider _underPlayerCollider;

    [SerializeField] private float _checkDistance;

    private Vector3 _checkingPoint;

    private Vector3 _oneNormalPoint;

    [HideInInspector] public float SlopeAngles;

    [HideInInspector] public Vector3 SlopeDirection;

    [HideInInspector] public bool IsOnSlope = false;

	[HideInInspector] public float SlopeDistance;

    public bool DebugRayCast = true;

    private void Update()
    {
		CheckForSlops();
	}

    void CheckForSlops()
    {
        RaycastHit hitUnderPlayer;
        RaycastHit hitCheckSlope;

        if (Physics.Raycast(transform.position, Vector3.down, out hitUnderPlayer))
        {
            _underPlayerPoint = hitUnderPlayer.point;
            _underPlayerNormal = hitUnderPlayer.normal;
            _underPlayerCollider = hitUnderPlayer.collider;
			SlopeDistance = hitUnderPlayer.distance;

			_oneNormalPoint = _underPlayerPoint + (_underPlayerNormal.normalized * _checkDistance);

            Physics.Raycast(transform.position, _oneNormalPoint - transform.position, out hitCheckSlope);

            if (hitCheckSlope.collider == _underPlayerCollider)
                _checkingPoint = hitCheckSlope.point;
            else
                _checkingPoint = _underPlayerPoint;

            SlopeDirection = ((_checkingPoint - _underPlayerPoint).normalized * 1);
            SlopeAngles = Mathf.Abs(Vector3.Angle(Vector3.down, SlopeDirection) - 90);

            if (SlopeAngles <= 10 || SlopeAngles == 90)
                IsOnSlope = false;
            else
                IsOnSlope = true;

            /*Debug.Log("slopeAngles == " + slopeAngles);
            Debug.Log("isOnSlope == " + isOnSlope);*/
        }
    }

    private void OnDrawGizmos()
    {
        if (DebugRayCast)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, _underPlayerPoint - transform.position);

            if (IsOnSlope)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(_underPlayerPoint, _underPlayerNormal);

                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, _oneNormalPoint - transform.position);

                Gizmos.color = Color.black;
                Gizmos.DrawRay(_underPlayerPoint, SlopeDirection);
            }
        }   
    }
}
