using UnityEngine;

/// <summary> ランダムな方向にばらまく弾 </summary>
public class BulletRandom : BulletParent
{
    protected override void Start()
    {
        base.Start();

        speed = 6.0f;

        //ランダムな方向を決める
        var random = Random.Range(-30.0f, 40.0f);

        //移動
        MoveDirection(true);
        velocity = Quaternion.AngleAxis(random, new Vector3(0, 0, 1)) * velocity;

        //移動方向に回転
        var nextPos = velocity * speed + this.transform.position;
        dir = nextPos - this.transform.position;

        //得点を設定
        GetComponent<Score>().SetScore(350);
    }
}
