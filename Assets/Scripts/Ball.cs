using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using MonsterLove.StateMachine;

public class Ball : StateBehaviour {
	public float speed;
	public float realspeed;
	public Vector3 newVelocity;
	public AudioClip hitFloor;
	public AudioClip hitBrick;
	public AudioClip hitPaddle;
	public AudioClip hitWall;
	public MainCharacter player;
	public Transform particle;
	private AudioSource audioSource;
	private int hitCounter;
	public Transform explosionParticle;
	public delegate void EventHandler (Collision collision);
	public event EventHandler BallCollision;
    private new Rigidbody rigidbody;
    private new Collider collider;
    private new Renderer renderer;

	public enum States
	{

        Normal,
		Destroyed,
        Locked
	}

	// Use this for initialization
	void Awake(){
		audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider>();
        Initialize<States>();
		ChangeState (States.Locked);
        BallCollision += (Collision collision) => CollisionActions(collision);
    }

	void Normal_Enter () {
		Quaternion rotation = Quaternion.AngleAxis (Random.Range (-20, 20), Vector3.up);
		Vector3 direction = rotation*(Vector3.up + Vector3.forward);
		rigidbody.velocity = direction.normalized * speed;
        HitParticles(transform.position, Vector3.forward);
        hitCounter = 0;
	}

    void Locked_Enter()
    {
        rigidbody.velocity = Vector3.zero;
        renderer.enabled = false;
        collider.enabled = false;
        rigidbody.useGravity = false;
    }

    void Locked_Exit()
    {
        rigidbody.velocity = Vector3.zero;
        renderer.enabled = true;
        collider.enabled = true;
    }

    // Update is called once per frame
    void Normal_FixedUpdate () {
		realspeed = rigidbody.velocity.magnitude;

	}

    void OnCollisionEnter(Collision collision) {
        BallCollision(collision);
    }

    void CollisionActions(Collision collision) { 
		switch (collision.transform.tag){
		case "Door":
            DoorActions(collision);
			break;
		case "Brick":
            BrickActions();
			break;
		case "Floor":
            FloorActions(collision);
            break;
		case "Player":
            PlayerActions(collision);
			break;
		case "Wall":
            WallActions(collision);
			break;
		case "Back_Wall":
            BackWallActions(collision);
			break;
        case "Boss":
            BossActions(collision);
            break;
         default:
			break;
		}
		Vector3 direction = rigidbody.velocity.normalized;
        rigidbody.velocity = direction * speed;
	}

    void DoorActions(Collision collision){
        rigidbody.useGravity = true;
        audioSource.PlayOneShot(hitBrick);
        bool lastHit = collision.gameObject.GetComponent<Door>().DoDamage(1);
        
        HitParticles(collision);
        if (lastHit)
        {
            ChangeState(States.Locked);
        }
    }

    void BrickActions(){
        rigidbody.useGravity = true;
        audioSource.PlayOneShot(hitBrick);
    }

    void FloorActions(Collision collision){
        Vector3 velocity = rigidbody.velocity;
        //GetComponent<Rigidbody>().velocity=velocity*3f;
        audioSource.PlayOneShot(hitFloor);
        hitCounter++;
        if (hitCounter >= 3)
        {
            ChangeState(States.Destroyed);
        }
        SetColor();
        HitParticles(collision);
        Vector3 a = Vector3.up;

        rigidbody.AddForce(Vector3.up * speed * (hitCounter + 1));
        float xAngle=-Random.Range(22.5f, 67.5f);
        float yAngle=-Random.Range (-67.5f, 67.5f);
        newVelocity=Quaternion.Euler(new Vector3(xAngle,yAngle,0))*Vector3.forward*speed;
        rigidbody.useGravity = false;
        rigidbody.velocity = newVelocity;
        
    }

    void PlayerActions(Collision collision){
        collision.transform.GetComponent<MainCharacter>().Confusion();
        //Debug.Log("PLAYER");
    }

    void WallActions(Collision collision){
        audioSource.PlayOneShot(hitWall);
        HitParticles(collision);
    }

    void BackWallActions(Collision collision){
        audioSource.PlayOneShot(hitWall);
        HitParticles(collision);
    }

    void BossActions(Collision collision)
    {
        BossIA bossIA = collision.transform.GetComponent<BossIA>();
        bossIA.BallCollision();

        HitParticles(collision);
    }

    void OnTriggerEnter(Collider other){
		if (other.tag == "Paddle") {
			audioSource.PlayOneShot(hitPaddle);

			float xDiff=other.transform.position.x-transform.position.x;
			float yDiff=other.transform.position.y-transform.position.y;
			float yAngle=(xDiff/(other.transform.lossyScale.x/2))*45;
			float xAngle=(yDiff/(other.transform.lossyScale.y/2))*25-55;

			yAngle=Mathf.Min(yAngle,45);
			yAngle=Mathf.Max(yAngle,-45);
			xAngle=Mathf.Min(xAngle,0);
			xAngle=Mathf.Max(xAngle,-90);

			newVelocity=Quaternion.Euler(new Vector3(xAngle,yAngle,0))*Vector3.forward*speed;
			//Debug.Log(xDiff+" "+yDiff+"--"+yAngle+" "+xAngle);
            rigidbody.useGravity=false;
            rigidbody.velocity=newVelocity;
			hitCounter=0;
			SetColor();
		}
	}

	public void Destroyed_Enter(){
		Destroy(rigidbody);
		Animator animator = GetComponent<Animator> ();
		animator.enabled = true;
		animator.SetBool ("destroy", true);
        renderer.enabled = false;
		player.GetComponent<MainCharacter> ().Dead ();

	}

	public void Explosion(){
		Transform particleInstance = Instantiate (explosionParticle);
        var particleMain = particleInstance.GetComponent<ParticleSystem>().main;
        particleMain.startColor = GetComponent<Renderer> ().material.color;
		Invoke("Gameover", particleMain.duration*0.50f);

        particleInstance.position = transform.position;
        renderer.enabled = false;

	}

    public void Activate()
    {
        ChangeState(States.Normal);
    }

	public void Gameover(){
		Debug.Log ("gameover");
		SceneManager.LoadScene("game over");
	}

    public void HitParticles(Collision collision)
    {
        Vector3 position = collision.contacts[0].point;
        Vector3 axis = Quaternion.Euler(Vector3.forward * 90) * collision.contacts[0].normal;

        HitParticles(position, axis);
    }
    public void HitParticles(Vector3 position, Vector3 axis)
    {
        Transform particleInstance = Instantiate(particle);
        var particleMain = particleInstance.GetComponent<ParticleSystem>().main;
        particleInstance.transform.position = position;
        particleInstance.transform.rotation = Quaternion.AngleAxis(90, axis);
		//Debug.Log (collision.contacts [0].normal);
		Quaternion.Euler (Vector3.left * 90);
		particleMain.startColor = 
			GetComponent<MeshRenderer> ().material.color;
        particleInstance.GetComponent<ParticleSystem> ().Play ();
		Destroy (particleInstance.gameObject, particleMain.duration);
	}

    public void HitParticles_old(Collision collision)
    {
        Transform particleInstance = Instantiate(particle);
        var particleMain = particleInstance.GetComponent<ParticleSystem>().main;
        particleInstance.transform.position = collision.contacts[0].point;
        particleInstance.transform.rotation = Quaternion.AngleAxis(90, Quaternion.Euler(Vector3.forward * 90) * collision.contacts[0].normal);
        //Debug.Log (collision.contacts [0].normal);
        Quaternion.Euler(Vector3.left * 90);
        particleMain.startColor =
            GetComponent<MeshRenderer>().material.color;
        particleInstance.GetComponent<ParticleSystem>().Play();
        Destroy(particleInstance.gameObject, particleMain.duration);
    }

    public void SetColor(){
		switch (hitCounter) {
		case 0:
			GetComponent<MeshRenderer>().material.color=Color.white;
			break;
		case 1:
			GetComponent<MeshRenderer>().material.color=Color.yellow;
			break;
		default:
			GetComponent<MeshRenderer>().material.color=Color.red;
			break;
		}
        var particleMain = GetComponent<ParticleSystem>().main;
        particleMain.startColor = GetComponent<MeshRenderer> ().material.color;
	}

    public Vector3 GetVelocity()
    {
        if ((States)GetState() == States.Normal)
        {
            return rigidbody.velocity;
        }else
        {
            return Vector3.zero;
        }
    }

    public void SetVelocity(Vector3 newVelocity)
    {
        if ((States)GetState() == States.Normal)
        {
            rigidbody.velocity = newVelocity;
        }
    }
}