using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private CSharpEventExample example;//ボタンの判定
    private GameObject bulletPrefab;//真っ直ぐ動く弾のプレハブ
    private float speed;//移動スピード

    private bool isShot;//生成できるかどうか
    private float shotElapsedTime;//攻撃した後の経過時間
    private const float shotMaxTime = 10.0f;//次攻撃ができるまでの時間

    private HPScript hPScript;//HP

    private void Start()
    {
        example = GameObject.Find("Manager").GetComponent<CSharpEventExample>();
        bulletPrefab = (GameObject)Resources.Load("BulletStraight");
        speed = 4.0f;
        isShot = true;
        shotElapsedTime = 0.0f;

        hPScript = new HPScript();
        hPScript.Init(3);
    }

    private void Update()
    {
        if (example.IsBomb())
        {
            //ボムを使うと画面内の敵弾をすべて消すことができる
            Debug.Log("ボムを使うと画面内の敵弾をすべて消すことができる");
        }
    }

    private void FixedUpdate()
    {
        if(hPScript.IsDead())
        {
            Debug.Log("死んだ");
            return;
        }

        if (example.IsMove())
        {
            this.transform.position += example.GetVelocity() * Time.deltaTime * speed;
        }

        //低速になる
        if (example.IsSlow())
        {
            this.transform.position -= example.GetVelocity() * Time.deltaTime * speed * 0.5f;
        }

        Shot();
    }

    /// <summary> 攻撃処理 </summary>
    private void Shot()
    {
        if (example.IsShot())
        {
            if (isShot)
            {
                Debug.Log("弾を生成");
                Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
                isShot = false;
            }
        }

        //まだ攻撃できないとき
        if (!isShot)
        {
            shotElapsedTime--;

            //経過時間が 0 になったら
            if (shotElapsedTime <= 0.0f)
            {
                shotElapsedTime = shotMaxTime;
                isShot = true;//攻撃できるように
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //エネミーが発射した弾に当たった時
        if(collision.transform.tag == "EnemyBullet")
        {
            hPScript.Damage();
            Destroy(collision.gameObject);
        }

        //エネミーに接触した時
        if(collision.transform.tag == "Enemy")
        {
            hPScript.Damage();
        }
    }
}
