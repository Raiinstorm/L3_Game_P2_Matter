using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
	public bool isPullable;
	public bool isPushable;
	Transform thisTransform;

	private void Start()
	{
		thisTransform = GetComponent<Transform>();
	}

	public void Pushing()
	{
		thisTransform.position = (Vector2)thisTransform.position + Vector2.left;
	}

	public void Pulling()
	{
		thisTransform.position = (Vector2)thisTransform.position + Vector2.right;
	}
}
