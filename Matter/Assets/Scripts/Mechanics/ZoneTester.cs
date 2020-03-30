using UnityEngine;

public class ZoneTester : MonoBehaviour
{
	[SerializeField] ZoneController _zoneController = null;
	[SerializeField] ElementType _type = ElementType.Extrude;

	void Update ()
	{
		if (Input.GetMouseButtonDown (0))
			_zoneController.ActivateElementOfType (_type);

		if (Input.GetMouseButtonDown (1))
			_zoneController.Cancel ();
	}
}