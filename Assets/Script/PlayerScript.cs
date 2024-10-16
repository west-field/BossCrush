using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private GameOverAndClearCheck gameOverCheck;
    private CSharpEventExample example;//ボタンの判定
    [SerializeField] private GameObject bulletPrefab;//真っ直ぐ動く弾のプレハブ
    private float speed;//移動スピード

    private bool isShot;//生成できるかどうか
    private float shotElapsedTime;//攻撃した後の経過時間
    private const float shotMaxTime = 5.0f;//次攻撃ができるまでの時間

    private HPScript hPScript;//HP

    private const int bombMaxNum = 3;
    private int bombNum;

    private void Start()
    {
        gameOverCheck = GameObject.Find("Manager").GetComponent<GameOverAndClearCheck>();
        example = GameObject.Find("Manager").GetComponent<CSharpEventExample>();
        speed = 4.0f;
        isShot = true;
        shotElapsedTime = 0.0f;

        hPScript = new HPScript();
        hPScript.Init(3);

        bombNum = bombMaxNum;
    }

    private void Update()
    {
        if (example.IsBomb())
        {
            //ボムを使うと画面内の敵弾をすべて消すことができる
            Debug.Log("ボムを使うと画面内の敵弾をすべて消すことができる");
            Bomb();
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

    /// <summary> ボム  敵が生成した弾をすべて削除する </summary>
    private void Bomb()
    {
        if(bombNum > 0)
        {
            bombNum--;
            //敵が生成した弾のゲームオブジェクトをすべて取得する
            var objs = GameObject.FindGameObjectsWithTag("EnemyBullet");

            foreach (var obj in objs)
            {
                Destroy(obj);
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
            if (hPScript.IsDead())
            {
                gameOverCheck.GameOver();
            }
        }

        //エネミーに接触した時
        if(collision.transform.tag == "Enemy")
        {
            hPScript.Damage();
            if(hPScript.IsDead())
            {
                gameOverCheck.GameOver();
            }
        }
    }
}
