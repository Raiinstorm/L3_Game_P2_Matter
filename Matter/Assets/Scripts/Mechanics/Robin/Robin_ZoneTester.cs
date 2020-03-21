using UnityEngine;

public class Robin_ZoneTester : MonoBehaviour
{
	[SerializeField] Robin_ZoneController _zoneController = null;
	[SerializeField] ElementType _type = ElementType.Extrude;

	void Update ()
	{
		if (Input.GetMouseButtonDown (0))
			_zoneController.ActivateElementOfType (_type);

		if (Input.GetMouseButtonDown (1))
			_zoneController.Cancel ();
	}
}