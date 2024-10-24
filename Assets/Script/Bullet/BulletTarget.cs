using UnityEngine;

/// <summary> ターゲットに向かって撃つ弾 </summary>
public class BulletTarget : BulletParent
{
    protected override void Start()
    {
        base.Start();

        speed = 6.0f;

        var player = GameObject.Find("Player");
        //ターゲットの位置から自分の位置を引く
        velocity = player.transform.position - this.transform.position;
        velocity.Normalize();

        //移動方向に回転
        var nextPos = velocity * speed + this.transform.position;
        dir = nextPos - this.transform.position;

        GetComponent<Score>().SetScore(150);
    }
}
