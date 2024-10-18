using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private GameOverAndClearCheck gameOverCheck;
    private CSharpEventExample example;//ボタンの判定

    /*移動*/
    private float speed;//移動スピード
    private Vector3 velocity;//移動量
    private Vector3 screenLeftBottom;
    private Vector3 screenRightTop;
    /*弾*/
    [SerializeField] private GameObject bulletPrefab;//真っ直ぐ動く弾のプレハブ
    private bool isShot;//生成できるかどうか
    private float shotElapsedTime;//攻撃した後の経過時間
    private const float shotMaxTime = 5.0f;//次攻撃ができるまでの時間

    /*HP*/
    private HPScript hpScript;//HP

    /*ボム*/
    private const int bombMaxNum = 3;
    private int bombNum;
    private ScoreManager score;

    /*サウンド*/
    private AudioSource audioSource;
    [SerializeField] private AudioClip shot;
    [SerializeField] private AudioClip damage;
    [SerializeField] private AudioClip bomb;

    /*もう一度左から出てくる*/
    private bool isInvincible;//無敵時間中かどうか
    private float invincibleElapsedTime;//無敵経過時間
    private const float invincibleMaxTime = 30.0f;//無敵時間
    private Renderer myRenderer;//自身の画像を見えなくする
    private Renderer shield;//シールドの画像を見えなくする
    [SerializeField] private ParticleSystem effector;

    private void Start()
    {
        gameOverCheck = GameObject.Find("Manager").GetComponent<GameOverAndClearCheck>();
        example = GameObject.Find("Manager").GetComponent<CSharpEventExample>();

        speed = 4.0f;
        velocity = Vector2.zero;

        isShot = true;
        shotElapsedTime = 0.0f;

        hpScript = new HPScript();
        hpScript.Init(3);

        bombNum = bombMaxNum;
        score = GameObject.Find("Manager").GetComponent<ScoreManager>();

        audioSource = GetComponent<AudioSource>();

        isInvincible = true;
        invincibleElapsedTime = invincibleMaxTime;

        myRenderer = GetComponent<Renderer>();
        myRenderer.enabled = false;
        shield = transform.GetChild(0).gameObject.GetComponent<Renderer>();

        screenLeftBottom = Camera.main.ScreenToWorldPoint(Vector3.zero);//左下
        screenRightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));//左下
    }

    private void Update()
    {
        //ボムを使うと画面内の敵弾をすべて消すことができる
        if (example.IsBomb())
        {
            Bomb();
        }
    }

    private void FixedUpdate()
    {
        if(hpScript.IsDead())
        {
            Debug.Log("死んだ");
            return;
        }

        if(isInvincible)
        {
            StartAgain();
            return;
        }

        if (example.IsMove())
        {
            velocity = example.GetVelocity() * Time.deltaTime * speed;
            this.transform.position = new Vector3(Mathf.Clamp(transform.position.x + velocity.x, screenLeftBottom.x, screenRightTop.x), Mathf.Clamp(transform.position.y + velocity.y, screenLeftBottom.y, screenRightTop.y), 0.0f);
        }

        //低速になる
        if (example.IsSlow())
        {
            //this.transform.position -= example.GetVelocity() * Time.deltaTime * speed * 0.5f;

            velocity = example.GetVelocity() * Time.deltaTime * speed * 0.5f;
            this.transform.position = new Vector3(Mathf.Clamp(transform.position.x - velocity.x, screenLeftBottom.x, screenRightTop.x), Mathf.Clamp(transform.position.y - velocity.y, screenLeftBottom.y, screenRightTop.y), 0.0f);
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
                //Debug.Log("弾を生成");
                Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
                isShot = false;
                audioSource.PlayOneShot(shot);
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
        if (bombNum > 0)
        {
            bombNum--;
            audioSource.PlayOneShot(bomb);

            //敵が生成した弾のゲームオブジェクトをすべて取得する
            var objs = GameObject.FindGameObjectsWithTag("EnemyBullet");

            foreach (var obj in objs)
            {
                //得点を取得する
                score.AddScore(obj.GetComponent<Score>().GetScore());

                Destroy(obj);
            }
        }
    }

    /// <summary> 復活 </summary>
    private void StartAgain()
    {
        if(!myRenderer.enabled)
        {
            myRenderer.enabled = true;
            shield.enabled = true;
            this.transform.position = new Vector3(-10.0f, 0.0f, 0.0f);
        }

        invincibleElapsedTime--;

        //画面左から右へ移動する
        this.transform.position += new Vector3(0.1f, 0.0f, 0.0f);

        //経過時間が 0 になったら
        if (invincibleElapsedTime <= 0.0f)
        {
            invincibleElapsedTime = invincibleMaxTime;
            isInvincible = false;
            shield.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //エネミーが発射した弾に当たった時
        if(collision.transform.tag == "EnemyBullet")
        {
            Destroy(collision.gameObject);

            //無敵時間の時は攻撃を受けないように
            if (isInvincible) return;

            Damage();
        }

        //無敵時間の時は攻撃を受けないように
        if (isInvincible) return;

        //エネミーに接触した時
        if (collision.transform.tag == "Enemy")
        {
            Damage();
        }
    }

    /// <summary> ダメージを受けたときの処理 </summary>
    private void Damage()
    {
        //エフェクトを作成
        Instantiate(effector, this.transform.position, Quaternion.identity);
        audioSource.PlayOneShot(damage);

        hpScript.Damage();

        //自分の弾を削除する
        var objs = GameObject.FindGameObjectsWithTag("PlayerBullet");
        foreach (var obj in objs)
        {
            Destroy(obj);
        }

        //敵の弾を削除する
        //敵が生成した弾のゲームオブジェクトをすべて取得する
        objs = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (var obj in objs)
        {
            Destroy(obj);
        }

        isInvincible = true;
        myRenderer.enabled = false;

        if (hpScript.IsDead())
        {
            gameOverCheck.GameOver();
        }
    }
}
