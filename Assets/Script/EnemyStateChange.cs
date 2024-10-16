using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �G�l�~�[�̏�ԕω� </summary>
public class EnemyStateChange : MonoBehaviour
{
    private GameOverAndClearCheck clearCheck;

    /// <summary> �{�X�^�C�v </summary>
    enum BossType
    {
        Top,
        Main,
        Under,
        Max
    }

    /// <summary> �R�i�K�̏�ԕω������� </summary>
    enum StateChange
    {
        TargetShot,
        RandomShot,
        HomingShot,
        Max
    }


    [SerializeField] private GameObject[] enemyBosses;
    [SerializeField] private GameObject[] bulletPrefabs;
    [SerializeField] private Transform[] bulletStartPosition;//�e�𔭎˂���ʒu
    private StateChange state;

    private bool isShot;//�����ł��邩�ǂ���
    private float shotElapsedTime;//�U��������̌o�ߎ���
    private const float shotMaxTime = 25.0f;//���U�����ł���܂ł̎���

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

    /// <summary> �U�� </summary>
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

    /// <summary> �����Ă��邩���m�F </summary>
    private void CheckAlive()
    {
        var aliveNum = 0;
        foreach (var enemy in enemyBosses)
        {
            //�����Ă���Ƃ�
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

        //�{�̂̃��C�t��0�ɂȂ�ƃN���A
        if(enemyBosses[(int)BossType.Main].GetComponent<EnemyScript>().GetHPScript().IsDead())
        {
            clearCheck.Clear();

            //�폜
            foreach (var enemy in enemyBosses)
            {
                Destroy(enemy);
            }
        }
    }
}
