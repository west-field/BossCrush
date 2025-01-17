using UnityEngine;

/// <summary> まっすぐ進む弾 </summary>
public class BulletStraight : BulletParent
{
    private ScoreManager score;//得点を取得するため

    protected override void Start()
    {
        base.Start();

        speed = 10.0f;

        MoveDirection(false);

        score = GameObject.Find("Manager").GetComponent<ScoreManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //エネミーの弾の時
        if(collision.transform.tag == "EnemyBullet")
        {
            //得点を取得
            score.AddScore(collision.gameObject.GetComponent<Score>().GetScore());

            //自分の弾と敵弾を消す
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
