using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicController : MonoBehaviour {
	[SerializeField]
	float speed = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
		float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

		transform.Translate(new Vector3(horizontal, 0, vertical));
	}
}
