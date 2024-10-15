using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> まっすぐ進む弾 </summary>
public class BulletStraight : BulletParent
{
    protected override void Start()
    {
        base.Start();

        speed = 10.0f;
        velocity = new Vector3(1.0f, 0.0f, 0.0f);
    }

    protected override void Move()
    {
        //base.Move();
        //Debug.Log("まっすぐ進む弾のMove");
        this.transform.position += velocity * Time.deltaTime * speed;
    }
}
