using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> エネミーの状態変化 </summary>
public class EnemyStateChange : MonoBehaviour
{
    private GameOverAndClearCheck clearCheck;

    /// <summary> ボスタイプ </summary>
    enum BossType
    {
        Top,
        Under,
        Main,
        Max
    }

    /// <summary> ３段階の状態変化を持つ </summary>
    enum StateChange
    {
        TargetShot,
        RandomShot,
        HomingShot,
        Max
    }

    [SerializeField] private GameObject[] enemyBosses;
    /*弾を発射する*/
    [SerializeField] private GameObject[] bulletPrefabs;
    [SerializeField] private Transform[] bulletStartPosition;//弾を発射する位置
    private StateChange state;

    private bool isShot;//生成できるかどうか
    private float shotElapsedTime;//攻撃した後の経過時間
    private const float shotMaxTime = 25.0f;//次攻撃ができるまでの時間

    private AudioSource audioSource;
    [SerializeField] private AudioClip shot;

    /*上下に移動*/
    private Vector3 defaultPosition;//元の位置
    private float speed;//移動スピード

    /*登場*/
    private bool isEntry;
    private float entryElapsedTime;//エントリー経過時間
    private const float entryMaxTime = 80.0f;//エントリー時間

    private void Start()
    {
        clearCheck = GameObject.Find("Manager").GetComponent<GameOverAndClearCheck>();

        state = StateChange.TargetShot;
        isShot = true;
        shotElapsedTime = shotMaxTime;

        audioSource = GetComponent<AudioSource>();

        speed = 1.0f;

        isEntry = true;
        entryElapsedTime = entryMaxTime;
    }

    private void FixedUpdate()
    {
        if(isEntry)
        {
            Entry();
            return;
        }

        if(clearCheck.IsClear())
        {
            return;
        }

        //上下に移動する
        this.transform.position = new Vector3(defaultPosition.x, Mathf.Sin(Time.time - entryElapsedTime) * speed + defaultPosition.y, defaultPosition.z);

        CheckAlive();

        Shot();
    }

    /// <summary> エントリー </summary>
    private void Entry()
    {
        entryElapsedTime--;

        //左から右に出てくる
        this.transform.position -= new Vector3(0.1f, 0.0f, 0.0f);

        //経過時間が 0 になったら
        if (entryElapsedTime <= 0.0f)
        {
            defaultPosition = this.transform.position;
            entryElapsedTime = entryMaxTime;
            isEntry = false;
        }
    }

    /// <summary> 攻撃 </summary>
    private void Shot()
    {
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
                //弾を生成
                Instantiate(bulletPrefabs[(int)state], bulletStartPosition[i].position, Quaternion.identity);
            }

            audioSource.PlayOneShot(shot);
            isShot = false;
        }
        else
        {
            shotElapsedTime--;
            if (shotElapsedTime <= 0)
            {
                isShot = true;
                shotElapsedTime = shotMaxTime;
            }
        }
    }

    /// <summary> 生きているかを確認 </summary>
    private void CheckAlive()
    {
        var aliveNum = 0;
        foreach (var enemy in enemyBosses)
        {
            //生きているとき
            if(!enemy.GetComponent<EnemyScript>().GetHPScript().IsDead())
            {
                aliveNum++;
            }
        }
        switch(aliveNum)
        {
            case 0:
                clearCheck.Clear();
                break;
            case 1:
                state = StateChange.HomingShot;
                break;
            case 2:
                state = StateChange.RandomShot;
                break;
            default:
                state = StateChange.TargetShot;
                break;
        }

        //本体のライフが0になるとクリア
        if(enemyBosses[(int)BossType.Main].GetComponent<EnemyScript>().GetHPScript().IsDead())
        {
            clearCheck.Clear();

            foreach (var enemy in enemyBosses)
            {
                enemyBosses[(int)BossType.Main].GetComponent<EnemyScript>().ColorChange();
            }
        }
    }
}
