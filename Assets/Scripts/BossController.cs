using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {
    public float moveAngle = 0;
    public float velocity = 10;
    public float accelerationTime = 0.2f;

    public float baseEnergy = 10f;
    public float energyIncrement = 1.25f;
    public float energyDecrement = 2;

    protected Rigidbody rigidBody;

    protected bool isColliding;
    protected float maxEnergy;
    protected float energy;
    protected float speedMod;
    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

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
}
