using UnityEngine;

public abstract class Extrude : GenericElement
{
	/// Ici déclarer les variables qui serviront à tous les types d'extrude.

	public float SpeedExtrude = 15f;
	public float DistanceExtrude = 2.2f;
	protected Vector3 init_pos;

	void Start()
	{
		init_pos = transform.position;
	}



}