using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> まっすぐ進む弾 </summary>
public class BulletStraight : BulletParent
{
    private ScoreManager score;

    protected override void Start()
    {
        base.Start();

        speed = 10.0f;
        velocity = new Vector3(1.0f, 0.0f, 0.0f);

        score = GameObject.Find("Manager").GetComponent<ScoreManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "EnemyBullet")
        {
            score.AddScore(collision.gameObject.GetComponent<Score>().GetScore());

            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
