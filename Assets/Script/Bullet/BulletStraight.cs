using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �܂������i�ޒe </summary>
public class BulletStraight : BulletParent
{
    private ScoreManager score;//���_���擾���邽��

    protected override void Start()
    {
        base.Start();

        speed = 10.0f;

        MoveDirection(false);

        score = GameObject.Find("Manager").GetComponent<ScoreManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�G�l�~�[�̒e�̎�
        if(collision.transform.tag == "EnemyBullet")
        {
            //���_���擾
            score.AddScore(collision.gameObject.GetComponent<Score>().GetScore());

            //�����̒e�ƓG�e������
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
