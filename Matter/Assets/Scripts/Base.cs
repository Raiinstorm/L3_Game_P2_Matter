using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
	public int hp;
	public float radiusEnnemy;
	public Transform detectionPoint;

    // Start is called before the first frame update
    void Start()
    {
		var circle1= new GameObject { name = "CircleDetection" };
		circle1.transform.position = transform.position;
		circle1.transform.parent = transform;
		circle1.DrawCircle(radiusEnnemy, .1f);
	}
}
