using System.Collections;
using System.Collections.Generic;
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

        GetComponent<Score>().SetScore(200);
    }
}
