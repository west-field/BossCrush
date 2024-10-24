using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> プレイヤー </summary>
public class PlayerScript : MonoBehaviour
{
    private GameFlagCheck gameFlagCheck;//フラグ
    private UpdateExample updateExample;//ボタンの判定

    /*移動*/
    private float speed;//移動スピード
    private Vector3 velocity;//移動量
    //画面外に出ないように
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
    //ボムで消した弾のスコアを取得する
    private ScoreManager score;
    //ボムを使ったときに白い画像をフェードさせる
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

    /*登場*/
    private bool isEntry;//登場中かどうか
    private float entryElapsedTime;//経過時間
    private const float entryMaxTime = 30.0f;//登場完了までの時間

    private Renderer myRenderer;//自身の画像を見えなくする
    private Renderer shield;//シールドの画像を見えなくする
    
    [SerializeField] private ParticleSystem effect;//エフェクト

    private void Start()
    {
        var manager = GameObject.Find("Manager");
        gameFlagCheck = manager.GetComponent<GameFlagCheck>();
        updateExample = manager.GetComponent<UpdateExample>();

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

        hpCanvas.SetActive(false);

        bombNum = bombMaxNum;
        score = manager.GetComponent<ScoreManager>();
        bombFadeElapsedTime = bombFadeMaxTime;
        bombFadeAlpha = bombFadeAlphaMax;
        bombFadeAlphaSpeed = bombFadeAlphaMax / bombFadeMaxTime;
        bombFade.enabled = false;
        isBombFade = false;

        audioSource = GetComponent<AudioSource>();

        isEntry = true;
        entryElapsedTime = entryMaxTime;

        myRenderer = GetComponent<Renderer>();
        myRenderer.enabled = false;
        shield = transform.GetChild(0).gameObject.GetComponent<Renderer>();

        screenLeftBottom = Camera.main.ScreenToWorldPoint(Vector3.zero);//左下
        screenRightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));//左下
    }

    private void Update()
    {
        if (gameFlagCheck.IsPause())
        {
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
        //ボムのフェード
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
            return;
        }

        //ゲームクリアした時
        if(gameFlagCheck.IsClear())
        {
            //残っているライフ分得点を取得する
            GameObject.Find("Manager").GetComponent<ScoreManager>().AddScore(hpScript.GetHp() * 10000);
            hpScript.Damage(kHpMax);
            return;
        }

        if(isEntry)
        {
            StartAgain();
            return;
        }

        if(updateExample.OnPressed(UpdateExample.ActionType.Move))
        {
            //移動中はHPが見えないように
            if(hpCanvas.activeSelf)
            {
                hpCanvas.SetActive(false);
            }

            //移動
            velocity = updateExample.GetVelocity() * Time.deltaTime * speed;
            this.transform.position = new Vector3(Mathf.Clamp(transform.position.x + velocity.x, screenLeftBottom.x, screenRightTop.x), Mathf.Clamp(transform.position.y + velocity.y, screenLeftBottom.y, screenRightTop.y), 0.0f);
        }
        else
        {
            //移動していないときははHPが見えるように
            if (!hpCanvas.activeSelf)
            {
                hpCanvas.SetActive(true);
            }
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

        entryElapsedTime--;

        //画面左から右へ移動する
        this.transform.position += new Vector3(0.1f, 0.0f, 0.0f);

        //経過時間が 0 になったら
        if (entryElapsedTime <= 0.0f)
        {
            entryElapsedTime = entryMaxTime;
            isEntry = false;
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
        if (isEntry) return;

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
        if (isEntry) return;

        hpCanvas.SetActive(true);

        //エフェクトを作成
        Instantiate(effect, this.transform.position, Quaternion.identity);
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

        isEntry = true;
        myRenderer.enabled = false;

        if (hpScript.IsDead())
        {
            gameFlagCheck.GameOver();
            hpCanvas.SetActive(false);
        }
    }
}
