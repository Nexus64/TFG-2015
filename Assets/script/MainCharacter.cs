using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class MainCharacter : StateBehaviour {
	public float maxSpeed;
	public float force;
	public Transform paddlePref;
	private Transform model;
	public enum States
	{
		Stand, 
		Move, 
		Paddle,
		Confused,
		Dead,
	}
	// Use this for initialization

	void Awake(){
		Debug.Log ("Awake");
		model = transform.GetChild(0);
		Initialize<States>();
		ChangeState(States.Stand);
	}

	void Start () {
	
	}

	// Update is called once per frame
	void Stand_Update () {
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.UpArrow)
			|| Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.DownArrow)) {
			ChangeState (States.Move, StateTransition.Safe);
		} else if (Input.GetKeyDown (KeyCode.A)) {
			ChangeState (States.Paddle, StateTransition.Safe);
		}
	}

	void Move_Update(){
		if (!Input.GetKey (KeyCode.RightArrow) && !Input.GetKey (KeyCode.UpArrow)
			&& !Input.GetKey (KeyCode.LeftArrow) && !Input.GetKey (KeyCode.DownArrow)) {
			ChangeState (States.Stand, StateTransition.Safe);
		}  else if (Input.GetKeyDown (KeyCode.A)) {
			ChangeState (States.Paddle, StateTransition.Safe);
		}else {
			model.eulerAngles = calculateRotation ();
			float forceX = force * Mathf.Cos (Mathf.Deg2Rad * model.rotation.eulerAngles.y);
			float forceZ = -force * Mathf.Sin (Mathf.Deg2Rad * model.rotation.eulerAngles.y);

			GetComponent<Rigidbody> ().AddForce (new Vector3 (Mathf.Round (forceX), 0, Mathf.Round (forceZ)));

		}
	}
	void Move_FixedUpdate() {
		if (GetComponent<Rigidbody> ().velocity.magnitude > maxSpeed) {
			GetComponent<Rigidbody> ().velocity=GetComponent<Rigidbody> ().velocity.normalized*maxSpeed;
		}


	}

	void Paddle_Enter(){
		float time = 0.6f;
		Transform paddle=Instantiate(paddlePref);
		paddle.parent = transform;
		paddle.localPosition = paddle.position;
		Destroy (paddle.gameObject, time-time*0.2f);
		GetComponent<Rigidbody> ().drag *= 0.40f;
		Invoke ("Paddle_End", time);
	}
	void Paddle_End(){
		GetComponent<Rigidbody> ().drag *= 1/0.40f;

		ChangeState (States.Stand, StateTransition.Safe);
	}
	void Confused_Enter(){
		GetComponent<AudioSource> ().Play ();
		Invoke ("Confused_End", 1);
	}
	void Confused_Update(){
		transform.Rotate (Vector3.up * 10);
	}
	void Confused_End(){
		GetComponent<AudioSource> ().Stop ();
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
		ChangeState (States.Stand, StateTransition.Overwrite);
	}
	public void confusion(){
		ChangeState (States.Confused, StateTransition.Safe);
	}
	public void dead(){
		ChangeState (States.Dead, StateTransition.Overwrite);
	}
	Vector3 calculateRotation (){
		float newRotation;
		float input1 = -1;
		float input2 = -1;

		if (Input.GetKey (KeyCode.RightArrow)) {
			if (input1==-1){
				input1=0;
			}else {
				input2=0;
			}
		}
		if (Input.GetKey (KeyCode.UpArrow)) {
			if (input1==-1){
				input1=270;
			}else {
				input2=270;
			}
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			if (input1==-1){
				input1=180;
			}else {
				input2=180;
			}
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			if (input1==-1){
				input1=90;
			}else {
				input2=90;
			}
		}
		if (input1 == -1) {
			newRotation = input2;
		} else if (input2 == -1) {
			newRotation = input1;
		} else {
			if (input1 == 0 && input2 == 270) {
				input1 = 360;
			} else if (input2 == 0 && input1 == 270) {
				input2 = 360;
			}
			newRotation=(input1+input2)/2;
		}

		Vector3 direction = -(Quaternion.Inverse (Quaternion.AngleAxis(newRotation,Vector3.up)) * Vector3.left);
		Vector3 cross = Vector3.Cross (direction, -Vector3.up);
		Vector3 leftPoint = transform.position + cross * 0.5f;
		Vector3 rigthPoint = transform.position + cross * -0.5f;

		RaycastHit leftHit;
		RaycastHit rigthHit;

		Debug.DrawRay (transform.position, cross*10);
		Debug.DrawRay (leftPoint, direction*3);
		Debug.DrawRay (rigthPoint, direction*3);

		Physics.Raycast (leftPoint, direction*16, out leftHit,3f);
		Physics.Raycast (rigthPoint, direction*16, out rigthHit,3f);

		if (Mathf.Abs (Mathf.Abs (leftHit.distance - rigthHit.distance) - 1) < 0.5) {
			Quaternion rotation;
			if(leftHit.distance < rigthHit.distance){
				newRotation+=45;
			}else{
				newRotation-=45;
			}
		}
		return new Vector3 (0, newRotation, 0);
	}

}
