using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbreakableBrick : Brick
{

    public override void DoDamage(int damage)
    {
        particleSystem.Emit(30);
    }
}
