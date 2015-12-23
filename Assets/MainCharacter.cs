using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class MainCharacter : StateBehaviour {
	public float maxSpeed;
	public float force;

	public enum States
	{
		Stand, 
		Move, 
		Paddle, 
	}
	// Use this for initialization

	void Awake(){
		Debug.Log ("Awake");
		Initialize<States>();
		ChangeState(States.Stand);
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Stand_Update () {
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.UpArrow)
			|| Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.DownArrow)) {
			ChangeState(States.Move, StateTransition.Safe);
		}
	}

	void Move_Update(){
		if (!Input.GetKey (KeyCode.RightArrow) && !Input.GetKey (KeyCode.UpArrow)
			&& !Input.GetKey (KeyCode.LeftArrow) && !Input.GetKey (KeyCode.DownArrow)) {
			ChangeState (States.Stand, StateTransition.Safe);
		} else {
			transform.eulerAngles = calculateRotation ();
			float forceX = force * Mathf.Cos (Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
			float forceZ = force * Mathf.Sin (Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
			Debug.Log (Mathf.Round (forceX)+"  "+Mathf.Round (forceZ));
			GetComponent<Rigidbody> ().AddForce (new Vector3 (Mathf.Round (forceX), 0, Mathf.Round (forceZ)));
		}
	}
	void Move_FixedUpdate() {
		if (GetComponent<Rigidbody> ().velocity.magnitude > maxSpeed) {
			GetComponent<Rigidbody> ().velocity=GetComponent<Rigidbody> ().velocity.normalized*maxSpeed;
		}
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
				input1=90;
			}else {
				input2=90;
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
				input1=270;
			}else {
				input2=270;
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
		return new Vector3 (0, newRotation, 0);
	}

}
