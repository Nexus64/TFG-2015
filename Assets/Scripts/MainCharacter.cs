using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class MainCharacter : StateBehaviour {
	public float maxSpeed;
	public float force;
    public float paddleTime = 0.6f;
    public float exitTime = 2.5f;
    public bool enterReady = false;
    public Transform paddlePref;
	private Transform model;
    protected float normalDrag;
    protected float paddleDrag;
    protected Animator animator;
    protected new Rigidbody rigidbody;
    protected new Collider collider;
    protected Room roomController;

    public enum States
	{
		Stand, 
		Move, 
		Paddle,
		Confused,
		Dead,
        Enter,
        Exit,
	}
	// Use this for initialization

	void Awake(){
		///Debug.Log ("Awake");
		model = transform.GetChild(0);
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        roomController = GameObject.FindGameObjectWithTag("Room").GetComponent<Room>();
        normalDrag = rigidbody.drag;
        paddleDrag = normalDrag * 0.4f;

        Initialize<States>();
		ChangeState(States.Enter);
	}

	void Start () {
	
	}

    void Updated()
    {
        animator.SetFloat("MoveSpeed", rigidbody.velocity.magnitude);
    }

    void Enter_Enter()
    {
        transform.eulerAngles = -Vector3.up * 90;
    }

    void Enter_Update()
    {
        if (!enterReady) {
            rigidbody.AddForce(new Vector3(0, 0, force));

            if (-transform.position.z <= 0.5)
            {
                rigidbody.velocity = Vector3.zero;
                enterReady = true;
                roomController.PlayerReady(this);
            }
        }
    }

    void Exit_Enter()
    {
        transform.eulerAngles = -Vector3.up * 90;
        collider.enabled = false;
        Invoke("ChangeLevel", exitTime);
    }

    public void Activate() {
        Ball ballMovement = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
        ballMovement.Activate();
        print("player activate");
        ChangeState(States.Stand);
    }

    void Exit_Update()
    {
        rigidbody.AddForce(new Vector3(0, 0, force));
    }

    // Update is called once per frame
    void Stand_Update () {
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.UpArrow)
			|| Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.DownArrow)) {
			ChangeState (States.Move, StateTransition.Safe);
		} else if (Input.GetKeyDown (KeyCode.Space)) {
			ChangeState (States.Paddle, StateTransition.Safe);
		}
	}

	void Move_Update(){
		if (!Input.GetKey (KeyCode.RightArrow) && !Input.GetKey (KeyCode.UpArrow)
			&& !Input.GetKey (KeyCode.LeftArrow) && !Input.GetKey (KeyCode.DownArrow)) {
			ChangeState (States.Stand, StateTransition.Safe);
		}  else if (Input.GetKeyDown (KeyCode.Space)) {
			ChangeState (States.Paddle, StateTransition.Safe);
		}else {
			transform.eulerAngles = CalculateRotation ();
			float forceX = force * Mathf.Cos (Mathf.Deg2Rad * model.rotation.eulerAngles.y);
			float forceZ = -force * Mathf.Sin (Mathf.Deg2Rad * model.rotation.eulerAngles.y);

			rigidbody.AddForce (new Vector3 (Mathf.Round (forceX), 0, Mathf.Round (forceZ)));

		}

	}
	void Move_FixedUpdate() {
		if (rigidbody.velocity.magnitude > maxSpeed) {
            rigidbody.velocity = rigidbody.velocity.normalized*maxSpeed;
		}
  	}

	void Paddle_Enter(){
		Transform paddle=Instantiate(paddlePref);
        transform.eulerAngles = Vector3.up * -90;
        paddle.parent = transform;
		paddle.position += transform.position;
		Destroy (paddle.gameObject, paddleTime - paddleTime * 0.2f);
        rigidbody.drag = paddleDrag;
		Invoke ("Paddle_End", paddleTime);
	}

	void Paddle_End(){
        if (GetState().Equals(States.Paddle))
        { 
            ChangeState(States.Stand, StateTransition.Safe);
        }
	}

    void Paddle_Exit()
    {
        rigidbody.drag = normalDrag;
    }

    void Confused_Enter(){
		GetComponent<AudioSource> ().Play ();
        animator.SetBool("Confused", true);
		Invoke ("Confused_End", 1);
	}

	void Confused_Update(){
	}

	void Confused_End(){
        if( GetState().Equals(States.Confused))
        {
            GetComponent<AudioSource>().Stop();
            animator.SetBool("Confused", false);
            ChangeState(States.Stand, StateTransition.Overwrite);
        }
	}

	public void Confusion(){
		ChangeState (States.Confused, StateTransition.Safe);
	}

	public void Dead(){
        //Debug.Log("START DEATH");
        GetComponent<Animator>().SetTrigger("Dead");
        ChangeState (States.Dead, StateTransition.Overwrite);
	}

    public void WinCinematic()
    {
        ChangeState(States.Exit, StateTransition.Safe);
    }

    void ChangeLevel()
    {
        roomController.NextStage();
    }

    Vector3 CalculateRotation (){
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
			if(leftHit.distance < rigthHit.distance){
				newRotation+=45;
			}else{
				newRotation-=45;
			}
		}
		return new Vector3 (0, newRotation, 0);
	}
}
