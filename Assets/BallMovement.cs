using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class BallMovement : StateBehaviour {
	public float speed;
	public float realspeed;
	public Vector3 newVelocity;
	public AudioClip hit_floor;
	public AudioClip hit_brick;
	public AudioClip hit_paddle;
	public AudioClip hit_wall;
	public Transform player;
	public Transform particle;
	private AudioSource audio_source;
	private int hit_counter;
	public Transform explosion_particle;
	public enum States
	{
		Normal,
		Destroyed
	}
	// Use this for initialization
	void Awake(){
		audio_source = GetComponent<AudioSource>();
		Initialize<States>();
		ChangeState (States.Normal);
	}
	void Start () {
		Quaternion rotation = Quaternion.AngleAxis (Random.Range (-20, 20), Vector3.up);
		Vector3 direction = rotation*(Vector3.up + Vector3.forward);
		GetComponent<Rigidbody> ().velocity = direction.normalized * speed;
		hit_counter = 0;
	}
	
	// Update is called once per frame
	void Normal_FixedUpdate () {
		realspeed = GetComponent<Rigidbody> ().velocity.magnitude;

	}
	void OnCollisionEnter(Collision collision){
		switch (collision.transform.tag){
		case "Door":
			GetComponent<Rigidbody> ().useGravity=true;
			audio_source.PlayOneShot(hit_brick);
			collision.gameObject.GetComponent<DoorScript>().do_damage(10);
			hit_particles(collision);
			break;
		case "Brick":
			GetComponent<Rigidbody> ().useGravity=true;
			audio_source.PlayOneShot(hit_brick);

			break;
		case "Floor":
			Vector3 velocity = GetComponent<Rigidbody>().velocity;
			GetComponent<Rigidbody>().velocity=velocity*3f;
			audio_source.PlayOneShot(hit_floor);
			hit_counter++;
			if (hit_counter>=3){
				ChangeState(States.Destroyed);
			}
			hit_particles(collision);

			/*float xAngle=-Random.Range(22.5f, 67.5f);
			float yAngle=-Random.Range (-67.5f, 67.5f);
			newVelocity=Quaternion.Euler(new Vector3(xAngle,yAngle,0))*Vector3.forward*speed;
			GetComponent<Rigidbody> ().useGravity=false;
			GetComponent<Rigidbody> ().velocity=newVelocity;
			*/break;
		case "Player":
			collision.transform.GetComponent<MainCharacter>().confusion();
			//Debug.Log("PLAYER");
			break;
		case "Wall":
			audio_source.PlayOneShot(hit_wall);
			hit_particles(collision);
			break;
		default:

			break;
		}
		Vector3 direction = GetComponent<Rigidbody> ().velocity.normalized;
		GetComponent<Rigidbody> ().velocity = direction * speed;
	}
	void OnTriggerEnter(Collider other){

		if (other.tag == "Paddle") {
			audio_source.PlayOneShot(hit_paddle);

			float xDiff=other.transform.position.x-transform.position.x;
			float yDiff=other.transform.position.y-transform.position.y;
			float yAngle=-(xDiff/(other.transform.lossyScale.x/2))*45;
			float xAngle=-(yDiff/(other.transform.lossyScale.y/2))*25-55;
			yAngle=Mathf.Min(yAngle,45);
			yAngle=Mathf.Max(yAngle,-45);
			xAngle=Mathf.Min(xAngle,0);
			xAngle=Mathf.Max(xAngle,-90);
			newVelocity=Quaternion.Euler(new Vector3(xAngle,yAngle,0))*Vector3.forward*speed;
			//Debug.Log(xDiff+" "+yDiff+"--"+yAngle+" "+xAngle);
			GetComponent<Rigidbody> ().useGravity=false;
			GetComponent<Rigidbody> ().velocity=newVelocity;
			hit_counter=0;
		}
	
	}

	public void Destroyed_Enter(){
		Destroy(GetComponent<Rigidbody>());
		Animator animator = GetComponent<Animator> ();
		animator.enabled = true;
		animator.SetBool ("destroy", true);
		player.GetComponent<Animator> ().SetBool ("dead", true);
		player.GetComponent<MainCharacter> ().dead ();

	}
	public void explosion(){
		Transform particle = Instantiate (explosion_particle);
		Invoke("gameover", particle.GetComponent<ParticleSystem>().duration*0.50f);
		particle.position = transform.position;

	}
	public void gameover(){
		Debug.Log ("gameover");
		Application.LoadLevel("game over");
	}
	public void hit_particles(Collision collision){
		Transform particle_ins=Instantiate(particle);
		particle_ins.transform.position=collision.contacts[0].point;
		particle_ins.transform.rotation= Quaternion.AngleAxis(90,Quaternion.Euler (Vector3.forward * 90)*collision.contacts [0].normal);
		Debug.Log (collision.contacts [0].normal);
		Quaternion.Euler (Vector3.left * 90);
		particle_ins.GetComponent<ParticleSystem> ().Play ();
		Destroy (particle_ins.gameObject, particle.GetComponent<ParticleSystem> ().duration);
	}
}