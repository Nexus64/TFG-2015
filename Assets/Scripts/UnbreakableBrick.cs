using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbreakableBrick : Brick
{
    protected new void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ball")
        {
            particleSystem.Emit(30);
        }
    }
}
