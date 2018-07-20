using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControls : MonoBehaviour {

	[SerializeField]
	float speed = 1f;
	[SerializeField]
	private Vector3 positiveOffsetLimit, negativeOffsetLimit;

	private Vector3 originalPosition;
	private Vector3 targetPosition;

	void Start ()
	{
		originalPosition = transform.position;
		targetPosition = originalPosition;
	}
	
	public float testLerp = 0.5f;
	void FixedUpdate ()
	{
		float x = Input.GetAxis("Mouse X") * Time.deltaTime * speed;
		float y = 0;
		float z = Input.GetAxis("Mouse Y") * Time.deltaTime * speed;

		if (Input.GetAxis("Fire1") == 1)
		{
			y = z;
			z = 0;
		}

		Vector3 translate = new Vector3(x, y, z);

		targetPosition += translate;

		targetPosition.x = Mathf.Clamp(targetPosition.x, originalPosition.x - negativeOffsetLimit.x, originalPosition.x + positiveOffsetLimit.x);
		targetPosition.y = Mathf.Clamp(targetPosition.y, originalPosition.y - negativeOffsetLimit.y, originalPosition.y + positiveOffsetLimit.y);
		targetPosition.z = Mathf.Clamp(targetPosition.z, originalPosition.z - negativeOffsetLimit.z, originalPosition.z + positiveOffsetLimit.z);

		transform.position = Vector3.Lerp(transform.position, targetPosition, testLerp);
	}

}
