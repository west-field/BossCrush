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
        Main,
        Under,
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
    [SerializeField] private GameObject[] bulletPrefabs;
    [SerializeField] private Transform[] bulletStartPosition;//弾を発射する位置
    private StateChange state;

    private bool isShot;//生成できるかどうか
    private float shotElapsedTime;//攻撃した後の経過時間
    private const float shotMaxTime = 25.0f;//次攻撃ができるまでの時間

    private void Start()
    {
        clearCheck = GameObject.Find("Manager").GetComponent<GameOverAndClearCheck>();

        state = StateChange.TargetShot;
        isShot = true;
        shotElapsedTime = shotMaxTime;
    }

    private void FixedUpdate()
    {
        if(clearCheck.IsClear())
        {
            return;
        }

        CheckAlive();

        Shot();
    }

    /// <summary> 攻撃 </summary>
    private void Shot()
    {
        if(isShot)
        {
            var i = 0;
            foreach (var enemy in enemyBosses)
            {
                if (enemy.GetComponent<EnemyScript>().GetHPScript().IsDead())
                {
                    continue;
                }

                Instantiate(bulletPrefabs[(int)state], bulletStartPosition[i].position, Quaternion.identity);
                i++;
            }
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
                break;
        }

        //本体のライフが0になるとクリア
        if(enemyBosses[(int)BossType.Main].GetComponent<EnemyScript>().GetHPScript().IsDead())
        {
            clearCheck.Clear();

            //削除
            foreach (var enemy in enemyBosses)
            {
                Destroy(enemy);
            }
        }
    }
}
