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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "EnemyBullet")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
