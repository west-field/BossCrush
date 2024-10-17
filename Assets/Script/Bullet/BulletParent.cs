using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 弾の親クラス </summary>
public class BulletParent : MonoBehaviour
{
    protected float speed;//移動速度
    protected Vector3 velocity;//移動
    protected SpriteRenderer spriteRenderer;//画面内にいるか判定を取る

    protected Vector3 dir;//向きたい方向

    protected virtual void Start()
    {
        speed = 1.0f;
        velocity = Vector3.zero;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Move();

        //弾が画面外に出たら削除する
        if(!spriteRenderer.isVisible)
        {
            //Debug.Log("画面外");
            Destroy(this.gameObject);
        }
    }

    /// <summary> 移動処理 </summary>
    protected virtual void Move()
    {
        //向きたい方向に回転
        this.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
        //移動
        this.transform.position += velocity * Time.deltaTime * speed;
    }
}
