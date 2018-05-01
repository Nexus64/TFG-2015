using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {
    public bool active;
    public bool damaged;
    public bool destroy;

    public float moveAngle = 0;
    public float velocity = 10;
    public float accelerationTime = 0.2f;

    public float baseEnergy = 10f;
    public float energyIncrement = 1.25f;
    public float energyDecrement = 2;

    public AudioClip startSound;
    public AudioClip ballSound;
    public AudioClip hurtSound;
    public AudioClip defeatSound;
    public Transform explosionParticle;

    protected Rigidbody rigidBody;
    protected AudioSource audioSource;

    protected bool isColliding;
    protected float maxEnergy;
    protected float energy;
    protected float speedMod;
    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        maxEnergy = baseEnergy;
        energy = maxEnergy;
        speedMod = 1;
    }

    void OnCollisionEnter(Collision other)
    {
        isColliding = true;
    }

    void OnCollisionExit(Collision other)
    {
        isColliding = false;
    }

    public void MoveDirection(float direction)
    {
        moveAngle = direction;
        float speed = velocity * speedMod;
        float velocityX = Mathf.Cos(direction);
        float velocityY = Mathf.Sin(direction);

        Vector3 moveSpeed = (new Vector3(velocityX, velocityY, 0)) * (speed / accelerationTime * Time.deltaTime);
        rigidBody.AddForce(moveSpeed, ForceMode.VelocityChange);

        if (rigidBody.velocity.magnitude > speed)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * speed;
        }
    }

    public void Stop()
    {
        rigidBody.velocity = Vector3.zero;
    }

    public void SetSpeedMod(float newSpeedMod)
    {
        speedMod = newSpeedMod;
    }

    public bool IsColliding()
    {
        return isColliding;
    }

    public void BallCollision(Ball ball)
    {
        MainCharacter player = ball.player;
        Vector3 playerDirection = Vector3.Normalize(player.transform.position - ball.transform.position) *
                                    ball.GetVelocity().magnitude;

        ball.SetVelocity(playerDirection);
        DecrementEnergy(1);
        PlayBallSound();
    }

    public void Activate()
    {
        active = true;
    }

    public void DoDamage(int damage)
    {
        IncrementMaxEnergy(1);
        damaged = true;
        
    }

    public void Explosion()
    {
        Transform particleInstance = Instantiate(explosionParticle);
        particleInstance.position = transform.position;

        particleInstance.position = transform.position;
    }

    public void StartDestruction()
    {
        destroy = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + rigidBody.velocity);
    }

    public void DecrementEnergy(int amount)
    {
        energy -= Math.Max(0, energyDecrement * amount);
        speedMod = energy / baseEnergy;
    }

    public void IncrementMaxEnergy(int amount)
    {
        maxEnergy = maxEnergy * energyIncrement * amount;
        energy = Math.Max(0, maxEnergy);
        speedMod = energy / baseEnergy;
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
