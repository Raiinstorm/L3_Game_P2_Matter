using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowEnnemyCircles : MonoBehaviour
{
	public EnnemyDetection attachedTo;
    void Start()
    {
		var circle1 = new GameObject { name =  "CircleDetection" };
		var circle2 = new GameObject { name = "CircleAttacking" };
		var circle3 = new GameObject { name = "CircleEscaping" };
		var circle4 = new GameObject { name = "CircleMelee" };

		circle1.transform.position = attachedTo.transform.position;
		circle2.transform.position = attachedTo.transform.position;
		circle3.transform.position = attachedTo.transform.position;
		circle4.transform.position = attachedTo.transform.position;

		circle1.transform.parent = transform;
		circle2.transform.parent = transform;
		circle3.transform.parent = transform;
		circle4.transform.parent = transform;

		circle1.DrawCircle(attachedTo.detectionRange, .5f);
		circle2.DrawCircle(attachedTo.attackingRange, .5f);
		circle3.DrawCircle(attachedTo.escapeRange, .5f);
		circle4.DrawCircle(attachedTo.meleeRange, .5f);
	}
}
