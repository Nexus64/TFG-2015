using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using System;

[RequireComponent(typeof(BossController))]
[RequireComponent(typeof(Animator))]
public class BossIA : StateBehaviour {
    public string state = "Out";
    public float reactionTime = 1f;
    public float moveTime = 2f;
    public float deathTime = 2f;
    public float hurtTime = 0.75f;

    public AudioClip startSound;
    public AudioClip ballSound;
    public AudioClip hurtSound;
    public AudioClip defeatSound;
    public Transform explosionParticle;

    protected Ball ball;
    protected BossController mainController;
    protected LevelGenerator roomController;
    protected Animator animator;
    protected AudioSource audioSource;

    protected float reactionTimer;
    protected float moveTimer;
    protected float hurtTimer;
    protected float moveAngle;

    public enum States
    {
        Out,
        Intro,
        Wait,
        Move,
        Hurt,
        Death
    }

    void Start()
    {
        GameObject ballObject = GameObject.FindGameObjectsWithTag("Ball")[0];
        ball = ballObject.GetComponent<Ball>();
        mainController = GetComponent<BossController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        roomController = GameObject.FindGameObjectWithTag("Room").GetComponent<LevelGenerator>();

        Initialize<States>();
        ChangeState(States.Out);
    }

    void Intro_Enter()
    {
        state = "INTRO";
        AnimatorClipInfo[] animations = animator.GetCurrentAnimatorClipInfo(0);
        float time = 0f;
        if (animations.Length > 0)
        {
            time = animations[0].clip.length;
        }
            
        Invoke("Intro_End", time);
    }

    void Intro_End()
    {
        ChangeState(States.Wait);
        roomController.BossReady(this);
    }

    public void Activate()
    {
        animator.SetBool("Activated", true);
    }

    public void StartIntro()
    {
        ChangeState(States.Intro);
    }

    void Wait_Enter()
    {
        state = "Wait";
        //print("boss waiting");
        reactionTimer = 0;
    }

    void Wait_Update()
    {
        if (BallApproaching())
        {
            reactionTimer += Time.deltaTime;

            if (reactionTimer >= reactionTime)
            {
                ChangeState(States.Move);
            }
        }
    }

    void Move_Enter()
    {
        state = "Move";
        moveTimer = 0;
    }

    void Move_Update()
    {
        moveAngle = AngleToBall();
        moveTimer += Time.deltaTime;

        if (!mainController.IsColliding())
        {
            mainController.MoveDirection(moveAngle);
        }

        if (!BallApproaching())
        {
            ChangeState(States.Wait);
        }
    }

    void Hurt_Enter()
    {
        state = "Hurt";
        animator.SetBool("Hurt", true);
        PlayHurtSound();
    }

    void Hurt_Update()
    {
        hurtTimer += Time.deltaTime;

        if (hurtTimer >= hurtTime)
        {
            ChangeState(States.Wait);
        }
    }

    void Hurt_Exit()
    {
        animator.SetBool("Hurt", false);
    }

    void Death_Enter()
    {
        state = "Death";
        animator.SetBool("Destroy", true);
    }

    float AngleToBall()
    {
        Vector3 vectorToBall = ball.transform.position - transform.position;
        vectorToBall.z = 0;
        Vector3 flatPosition = new Vector3(1, 0, 0);
        float sign = Mathf.Sign(vectorToBall.y);

        float result = Vector3.Dot(vectorToBall, flatPosition) / (vectorToBall.magnitude * flatPosition.magnitude);

        return sign * Mathf.Acos(result);
    }

    bool BallApproaching()
    {
        return ball.GetVelocity().z > 0;
    }

    public void BallCollision()
    {
        MainCharacter player = ball.player;
        Vector3 playerDirection = Vector3.Normalize(player.transform.position - ball.transform.position) * 
                                    ball.GetVelocity().magnitude;

        ball.SetVelocity(playerDirection);
        mainController.DecrementEnergy(1);
        PlayBallSound();
    }

    public void DoDamage(int damage)
    {
        mainController.IncrementMaxEnergy(1);
        ChangeState(States.Hurt, StateTransition.Overwrite);
        
    }

    public float StartDestruction()
    {
        ChangeState(States.Death, StateTransition.Overwrite);
        return deathTime;
    }

    public void Explosion()
    {
        Transform particleInstance = Instantiate(explosionParticle);
        particleInstance.position = transform.position;

        particleInstance.position = transform.position;
    }

    public void PlayStartSound()
    {
        audioSource.PlayOneShot(startSound);
    }

    public void PlayBallSound()
    {
        audioSource.PlayOneShot(ballSound);
    }

    public void PlayHurtSound()
    {
        audioSource.PlayOneShot(hurtSound);
    }

    public void PlayDefeatSound()
    {
        audioSource.PlayOneShot(defeatSound);
    }
}
