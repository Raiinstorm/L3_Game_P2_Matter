using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrudeSchematic : MonoBehaviour
{
	public float distanceExtrude;
	public Vector3 direction;
	public bool activated;

	Transform thisTransform;
	Vector3 initialPos;

	public KeyCode key;

    void Start()
    {
		thisTransform = GetComponent<Transform>();
		initialPos = thisTransform.position;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(Input.GetKeyDown(key))
		{
			activated = !activated;
		}

		Activation();
    }

	void Activation()
	{
		if(activated)
		{
			thisTransform.position = Vector3.Lerp(thisTransform.position, initialPos + direction * distanceExtrude, Time.deltaTime);
		}
		else
		{
			thisTransform.position = Vector3.Lerp(thisTransform.position, initialPos, Time.deltaTime);
		}
	}
}
