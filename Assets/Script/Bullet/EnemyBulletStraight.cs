using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletStraight : BulletParent
{
    protected override void Start()
    {
        base.Start();

        speed = 7.0f;
        MoveDirection(true);
        GetComponent<Score>().SetScore(100);
    }
}
