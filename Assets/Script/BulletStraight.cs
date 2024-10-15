using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Ç‹Ç¡Ç∑ÇÆêiÇﬁíe </summary>
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
        //Debug.Log("Ç‹Ç¡Ç∑ÇÆêiÇﬁíeÇÃMove");
        this.transform.position += velocity * Time.deltaTime * speed;
    }
}
