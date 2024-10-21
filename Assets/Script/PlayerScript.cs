using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private GameOverAndClearCheck gameOverCheck;
    private UpdateExample updateExample;//ボタンの判定

    private bool isPause;

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
    private const int kHpMax = 3;
    [SerializeField] private GameObject hpCanvas;
    private GameObject[] hpSprite;

    /*ボム*/
    private const int bombMaxNum = 3;
    private int bombNum;
    private ScoreManager score;
    [SerializeField] private Image bombFade;
    private const float bombFadeMaxTime = 20.0f;
    private float bombFadeAlpha;
    private const float bombFadeAlphaMax = 0.7f;
    private float bombFadeAlphaSpeed;
    private float bombFadeElapsedTime;//経過時間
    private bool isBombFade;

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
        //example = GameObject.Find("Manager").GetComponent<CSharpEventExample>();
        updateExample = GameObject.Find("Manager").GetComponent<UpdateExample>();

        isPause = false;

        speed = 4.0f;
        velocity = Vector2.zero;

        isShot = true;
        shotElapsedTime = 0.0f;

        hpScript = new HPScript();
        hpScript.Init(kHpMax);

        var hpCanvasChildren = hpCanvas.transform.childCount;
        hpSprite = new GameObject[hpCanvasChildren];
        for (int i = 0; i < hpCanvasChildren; i++)
        {
            var child = hpCanvas.transform.GetChild(i).transform.GetChild(0).gameObject;
            hpSprite[i] = child;
            hpSprite[i].gameObject.SetActive(false);
        }

        bombNum = bombMaxNum;
        score = GameObject.Find("Manager").GetComponent<ScoreManager>();
        bombFadeElapsedTime = bombFadeMaxTime;
        bombFadeAlpha = bombFadeAlphaMax;
        bombFadeAlphaSpeed = bombFadeAlphaMax / bombFadeMaxTime;
        bombFade.enabled = false;
        isBombFade = false;

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
        if (isPause)
        {
            if(updateExample.OnTrigger(UpdateExample.ActionType.Pause))
            {
                isPause = false;
                Time.timeScale = 1;
            }
            return;
        }
        else if(updateExample.OnTrigger(UpdateExample.ActionType.Pause))
        {
            isPause = true;
            Time.timeScale = 0;
            return;
        }

        //ボムを使うと画面内の敵弾をすべて消すことができる
        if (updateExample.OnTrigger(UpdateExample.ActionType.Bomb))
        {
            if (isBombFade) return;
            Bomb();
        }
    }

    private void FixedUpdate()
    {
        if(isBombFade)
        {
            bombFadeElapsedTime--;
            bombFadeAlpha -= bombFadeAlphaSpeed;

            var white = bombFade.color;
            white.a = bombFadeAlpha;
            bombFade.color = white;
            
            
            if(bombFadeElapsedTime <= 0)
            {
                isBombFade = false;
                bombFade.enabled = false;
            }
        }

        if(hpScript.IsDead())
        {
            Debug.Log("死んだ");
            return;
        }

        if(gameOverCheck.IsClear())
        {
            GameObject.Find("Manager").GetComponent<ScoreManager>().AddScore(hpScript.GetHp() * 10000);
            hpScript.Damage(kHpMax);
            return;
        }

        if(isInvincible)
        {
            StartAgain();
            return;
        }

        if(updateExample.OnPressed(UpdateExample.ActionType.Move))
        {
            velocity = updateExample.GetVelocity() * Time.deltaTime * speed;
            this.transform.position = new Vector3(Mathf.Clamp(transform.position.x + velocity.x, screenLeftBottom.x, screenRightTop.x), Mathf.Clamp(transform.position.y + velocity.y, screenLeftBottom.y, screenRightTop.y), 0.0f);
        }

        //低速になる
        if (updateExample.OnPressed(UpdateExample.ActionType.Slow))
        {
            velocity = updateExample.GetVelocity() * Time.deltaTime * speed * 0.5f;
            this.transform.position = new Vector3(Mathf.Clamp(transform.position.x - velocity.x, screenLeftBottom.x, screenRightTop.x), Mathf.Clamp(transform.position.y - velocity.y, screenLeftBottom.y, screenRightTop.y), 0.0f);
        }

        Shot();
    }

    /// <summary> 攻撃処理 </summary>
    private void Shot()
    {
        if (updateExample.OnPressed(UpdateExample.ActionType.Shot))
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
            bombFadeElapsedTime = bombFadeMaxTime;
            bombFadeAlpha = bombFadeAlphaMax;
            bombFade.enabled = true;
            isBombFade = true;

            bombNum--;
            audioSource.PlayOneShot(bomb);

            Debug.Log(bombNum + "個");

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
            hpCanvas.SetActive(true);
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
            Damage(collision.gameObject.GetComponent<BulletParent>().AttackPower());
            Destroy(collision.gameObject);
        }

        //無敵時間の時は攻撃を受けないように
        if (isInvincible) return;

        //エネミーに接触した時
        if (collision.transform.tag == "Enemy")
        {
            Damage(1);
        }
    }

    /// <summary> ダメージを受けたときの処理 </summary>
    private void Damage(int damagePower)
    {
        //無敵時間の時は攻撃を受けないように
        if (isInvincible) return;

        //エフェクトを作成
        Instantiate(effector, this.transform.position, Quaternion.identity);
        audioSource.PlayOneShot(damage);

        hpScript.Damage(damagePower);

        var eraseNum = kHpMax - hpScript.GetHp();
        Debug.Log(eraseNum);
        //HP表示を変える
        for (int i = 0; i < hpSprite.Length; i++)
        {
            if(i < eraseNum)
            {
                Debug.Log("消す");
                if (!hpSprite[i].gameObject.activeSelf)
                {
                    Debug.Log("消した");
                    hpSprite[i].gameObject.SetActive(true);
                }
            }
        }

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
        hpCanvas.SetActive(false);

        if (hpScript.IsDead())
        {
            gameOverCheck.GameOver();
        }
    }
}
