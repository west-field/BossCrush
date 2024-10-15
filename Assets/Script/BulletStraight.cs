using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �܂������i�ޒe </summary>
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
        //Debug.Log("�܂������i�ޒe��Move");
        this.transform.position += velocity * Time.deltaTime * speed;
    }
}
