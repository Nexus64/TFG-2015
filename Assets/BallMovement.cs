using UnityEngine;
using System.Collections;

public class BallMovement : MonoBehaviour {
	public float speed;
	public float realspeed;
	public Vector3 newVelocity;
	// Use this for initialization
	void Start () {
		Vector3 direction = Vector3.up + Vector3.forward;
		GetComponent<Rigidbody> ().velocity = direction.normalized * speed;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		realspeed = GetComponent<Rigidbody> ().velocity.magnitude;

	}
	void OnCollisionEnter(Collision collision){
		switch (collision.transform.tag){
		case "Wall":
			GetComponent<Rigidbody> ().useGravity=true;
			break;
		case "Floor":
			float xAngle=-Random.Range(22.5f, 67.5f);
			float yAngle=-Random.Range (-67.5f, 67.5f);
			newVelocity=Quaternion.Euler(new Vector3(xAngle,yAngle,0))*Vector3.forward*speed;
			GetComponent<Rigidbody> ().useGravity=false;
			GetComponent<Rigidbody> ().velocity=newVelocity;
			break;
		default:

			break;
		}
		Vector3 direction = GetComponent<Rigidbody> ().velocity.normalized;
		GetComponent<Rigidbody> ().velocity = direction * speed;
	}
	void OnTriggerEnter(Collider other){
		if (other.tag == "Paddle") {
			float xDiff=other.transform.position.x-transform.position.x;
			float yDiff=other.transform.position.y-transform.position.y;
			float yAngle=(xDiff/(other.transform.lossyScale.x/2))*45;
			float xAngle=-(yDiff/(other.transform.lossyScale.y/2))*25-55;
			yAngle=Mathf.Min(yAngle,45);
			yAngle=Mathf.Max(yAngle,-45);
			xAngle=Mathf.Min(xAngle,0);
			xAngle=Mathf.Max(xAngle,-90);
			newVelocity=Quaternion.Euler(new Vector3(xAngle,yAngle,0))*Vector3.forward*speed;
			Debug.Log(xDiff+" "+yDiff+"--"+yAngle+" "+xAngle);
			GetComponent<Rigidbody> ().useGravity=false;
			GetComponent<Rigidbody> ().velocity=newVelocity;
		}
	}
}