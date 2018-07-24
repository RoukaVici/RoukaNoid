using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] [RequireComponent(typeof(Collider))]
public class Ball : MonoBehaviour
{

	[SerializeField] private float initialVelocity = 10f;
	private Rigidbody rb;
	private bool ballInPlay;
	private Vector3 velocity, initialPos, initialScale;
	private Transform initialParent;
	private Collider col;
	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		velocity = new Vector3(0, initialVelocity, 0);
		initialPos = transform.localPosition;
		initialParent = transform.parent;
		initialScale = transform.localScale;
		col = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (ballInPlay == false && Input.GetAxis("Jump") > 0)
		{
			transform.parent = null;
            ballInPlay = true;
            rb.isKinematic = false;
            rb.AddForce(velocity);
		}
		if (rb.velocity.y >= - 0.5 && rb.velocity.y <= 0.5f)
			rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 1.5f, rb.velocity.z);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Paddle" && rb.useGravity == true)
		{
			transform.SetParent(initialParent);
			transform.localPosition = initialPos;
			transform.localScale = initialScale;
			rb.useGravity = false;
			rb.drag = 0;
			ballInPlay = false;
			col.isTrigger = false;
		}
	}

	void prepareBallReset()
	{
		col.isTrigger = true;
		rb.isKinematic = true;
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Paddle" && rb.useGravity == false)
		{
			float leftBound = other.transform.position.x - other.collider.bounds.size.x / 2;
			float rightBound = other.transform.position.x + other.collider.bounds.size.x / 2;
			float percent = (((rightBound - other.contacts[0].point.x) / (rightBound - leftBound)) - 0.5f) * -2;
			rb.velocity = Vector3.zero;
			float speedPercent = percent * initialVelocity;
			rb.AddForce(speedPercent, initialVelocity, 0);
		}
		else if (other.gameObject.tag == "Ground")
		{
			rb.useGravity = true;
			rb.drag = 5;
			Invoke("prepareBallReset", 1);
		}
	}

	public void resetBall()
	{
		transform.SetParent(initialParent);
		transform.localPosition = initialPos;
		transform.localScale = initialScale;
		rb.useGravity = false;
		rb.isKinematic = true;
		rb.drag = 0;
		ballInPlay = false;
		col.isTrigger = false;
	}
}
