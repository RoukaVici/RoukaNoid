using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControls : MonoBehaviour {

	[SerializeField] float speed = 1f;
	[SerializeField] private Vector3 positiveOffsetLimit, negativeOffsetLimit;

	[SerializeField] private float lerpMovement = 0.5f;

	[SerializeField]
	private Animator anim;
	private Vector3 originalPosition;
	private Vector3 targetPosition;
	private bool isGrabbing = false;
	private GameObject grabbedObject;
	private Transform grabbedObjectParent;

	void Start ()
	{
		originalPosition = transform.position;
		targetPosition = originalPosition;
	}

	void MoveHands()
	{
		float x = Input.GetAxis("Mouse X") * Time.deltaTime * speed;
		float y = 0;
		float z = Input.GetAxis("Mouse Y") * Time.deltaTime * speed;

		if (Input.GetAxis("Fire2") == 1)
		{
			y = z;
			z = 0;
		}

		Vector3 translate = new Vector3(x, y, z);

		targetPosition += translate;

		targetPosition.x = Mathf.Clamp(targetPosition.x, originalPosition.x - negativeOffsetLimit.x, originalPosition.x + positiveOffsetLimit.x);
		targetPosition.y = Mathf.Clamp(targetPosition.y, originalPosition.y - negativeOffsetLimit.y, originalPosition.y + positiveOffsetLimit.y);
		targetPosition.z = Mathf.Clamp(targetPosition.z, originalPosition.z - negativeOffsetLimit.z, originalPosition.z + positiveOffsetLimit.z);

		transform.position = Vector3.Lerp(transform.position, targetPosition, lerpMovement);
	}

	void AnimateHand()
	{
		if (Input.GetAxis("Fire1") > 0)
			anim.SetBool("isGrabbing", true);
		else
		{
			anim.SetBool("isGrabbing", false);
			isGrabbing = false;
			if (grabbedObject)
			{
				grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
				grabbedObject.transform.SetParent(grabbedObjectParent);
				grabbedObject = null;
			}
		}
	}

	void GrabObject()
	{

	}
	
	void FixedUpdate ()
	{
		MoveHands();
		AnimateHand();
		GrabObject();
	}

	void OnCollisionEnter(Collision other)
	{
		if (!isGrabbing && other.gameObject.tag == "Grabable"
		&& anim.GetCurrentAnimatorStateInfo(0).IsName("Grab") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
		{
			other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			grabbedObjectParent = other.transform.parent;
			grabbedObject = other.gameObject;
			other.transform.SetParent(transform);
			isGrabbing = true;
		}
	}

}
