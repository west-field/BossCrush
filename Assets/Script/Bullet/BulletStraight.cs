using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ‚Ü‚Á‚·‚®i‚Ş’e </summary>
public class BulletStraight : BulletParent
{
    private ScoreManager score;//“¾“_‚ğæ“¾‚·‚é‚½‚ß

    protected override void Start()
    {
        base.Start();

        speed = 10.0f;

        MoveDirection(false);

        score = GameObject.Find("Manager").GetComponent<ScoreManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //ƒGƒlƒ~[‚Ì’e‚Ì
        if(collision.transform.tag == "EnemyBullet")
        {
            //“¾“_‚ğæ“¾
            score.AddScore(collision.gameObject.GetComponent<Score>().GetScore());

            //©•ª‚Ì’e‚Æ“G’e‚ğÁ‚·
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
