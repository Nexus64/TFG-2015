using UnityEngine;
using System.Collections;

public class BallMovement : MonoBehaviour {
	public float speed;

	// Use this for initialization
	void Start () {
		Vector3 direction = Vector3.up + Vector3.forward;
		GetComponent<Rigidbody> ().velocity = direction.normalized * speed;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 direction = GetComponent<Rigidbody> ().velocity.normalized;
		GetComponent<Rigidbody> ().velocity = direction * speed;
	}
}
