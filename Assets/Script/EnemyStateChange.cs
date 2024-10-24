using UnityEngine;

/// <summary> エネミーの状態変化 </summary>
public class EnemyStateChange : MonoBehaviour
{
    /// <summary> ボスタイプ </summary>
    enum BossType
    {
        Top,
        Under,
        Main,
        Max
    }

    /// <summary> 3種類の弾を撃つことができる </summary>
    enum BulletType
    {
        TargetShot,
        RandomShot,
        HomingShot,
        Max
    }
    
    private GameFlagCheck gameFlagCheck;

    [SerializeField] private GameObject[] enemyBosses = new GameObject[(int)BossType.Max];
    /*弾を発射する*/
    [SerializeField] private GameObject[] bulletPrefabs = new GameObject[(int)BulletType.Max];
    [SerializeField] private Transform[] bulletStartPosition = new Transform[(int)BossType.Max];//弾を発射する位置

    private BulletType state;//状態変化に応じて攻撃方法を変更する

    /*状態によって変わる攻撃*/
    private bool isShot;//生成できるかどうか
    private float shotElapsedTime;//攻撃した後の経過時間
    private const float kShotMaxTime = 25.0f;//次攻撃ができるまでの時間

    /*常に発射する攻撃*/
    private bool isAlwaysShot;//生成できるかどうか
    private float alwaysShotElapsedTime;//攻撃した後の経過時間
    private const float kAlwaysShotMaxTime = 80.0f;//次攻撃ができるまでの時間

    /*音*/
    private AudioSource audioSource;
    [SerializeField] private AudioClip shot;//撃つときの音

    /*上下に移動*/
    private Vector3 defaultPosition;//元の位置
    private float speed;//移動スピード

    /*登場*/
    private bool isEntry;//登場中か
    private float entryElapsedTime;//エントリー経過時間
    private const float kEntryMaxTime = 80.0f;//エントリー時間

    private void Start()
    {
        gameFlagCheck = GameObject.Find("Manager").GetComponent<GameFlagCheck>();

        state = BulletType.TargetShot;
        isShot = true;
        shotElapsedTime = kShotMaxTime;

        isAlwaysShot = true;
        alwaysShotElapsedTime = kAlwaysShotMaxTime;

        audioSource = GetComponent<AudioSource>();

        speed = 1.0f;

        isEntry = true;
        entryElapsedTime = kEntryMaxTime;
    }

    private void FixedUpdate()
    {
        if(isEntry)
        {
            Entry();
            return;
        }

        if(gameFlagCheck.IsClear())
        {
            return;
        }

        //上下に移動する
        this.transform.position = new Vector3(defaultPosition.x, Mathf.Sin(Time.time - kEntryMaxTime) * speed + defaultPosition.y, defaultPosition.z);

        CheckAlive();

        Shot();
    }

    /// <summary> 登場 </summary>
    private void Entry()
    {
        entryElapsedTime--;

        //左から右に出てくる
        this.transform.position -= new Vector3(0.1f, 0.0f, 0.0f);

        //経過時間が 0 になったら
        if (entryElapsedTime <= 0.0f)
        {
            defaultPosition = this.transform.position;
            isEntry = false;
        }
    }

    /// <summary> 攻撃 </summary>
    private void Shot()
    {
        //状態によって変わる攻撃
        if(isShot)
        {
            for (int i = 0; i < enemyBosses.Length; i++)
            {
                //部位が生きていないときは
                if (enemyBosses[i].GetComponent<EnemyScript>().GetHPScript().IsDead())
                {
                    //何もしない
                    continue;
                }

                //部位が生きている時は弾を生成
                Instantiate(bulletPrefabs[(int)state], bulletStartPosition[i].position, Quaternion.identity);
            }

            audioSource.PlayOneShot(shot);//音を再生
            isShot = false;//いったん攻撃ができないようにする
        }
        else
        {
            shotElapsedTime--;
            //経過時間が0よりも小さくなったら
            if (shotElapsedTime <= 0)
            {
                //攻撃ができるように
                isShot = true;
                shotElapsedTime = kShotMaxTime;
            }
        }

        //常に発射する攻撃
        if(isAlwaysShot)
        {
            for (int i = 0; i < enemyBosses.Length; i++)
            {
                //弾を生成
                Instantiate(bulletPrefabs[(int)BulletType.RandomShot], bulletStartPosition[i].position, Quaternion.identity);
            }

            audioSource.PlayOneShot(shot);
            isAlwaysShot = false;
        }
        else
        {
            alwaysShotElapsedTime--;
            if (alwaysShotElapsedTime <= 0)
            {
                isAlwaysShot = true;
                alwaysShotElapsedTime = kAlwaysShotMaxTime;
            }
        }
    }

    /// <summary> 生きているかを確認 </summary>
    private void CheckAlive()
    {
        var aliveNum = 0;//生きている数(最大3)

        foreach (var enemy in enemyBosses)
        {
            //死んでいないとき
            if(!enemy.GetComponent<EnemyScript>().GetHPScript().IsDead())
            {
                aliveNum++;
            }
        }

        //生きている数によって攻撃方法を変更する
        switch(aliveNum)
        {
            case 0:
                gameFlagCheck.Clear();
                break;
            case 1:
                state = BulletType.HomingShot;
                break;
            case 2:
                state = BulletType.RandomShot;
                break;
            default:
                break;
        }

        //本体のライフが0になるとクリア
        if(enemyBosses[(int)BossType.Main].GetComponent<EnemyScript>().GetHPScript().IsDead())
        {
            gameFlagCheck.Clear();

            foreach (var enemy in enemyBosses)
            {
                enemyBosses[(int)BossType.Main].GetComponent<EnemyScript>().ColorChange();
            }
        }
    }
}
