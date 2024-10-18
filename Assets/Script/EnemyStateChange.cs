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
        Under,
        Main,
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
    /*�e�𔭎˂���*/
    [SerializeField] private GameObject[] bulletPrefabs;
    [SerializeField] private Transform[] bulletStartPosition;//�e�𔭎˂���ʒu
    private StateChange state;

    private bool isShot;//�����ł��邩�ǂ���
    private float shotElapsedTime;//�U��������̌o�ߎ���
    private const float shotMaxTime = 25.0f;//���U�����ł���܂ł̎���

    private AudioSource audioSource;
    [SerializeField] private AudioClip shot;

    /*�㉺�Ɉړ�*/
    private Vector3 defaultPosition;//���̈ʒu
    private float speed;//�ړ��X�s�[�h

    /*�o��*/
    private bool isEntry;
    private float entryElapsedTime;//�G���g���[�o�ߎ���
    private const float entryMaxTime = 80.0f;//�G���g���[����

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

        //�㉺�Ɉړ�����
        this.transform.position = new Vector3(defaultPosition.x, Mathf.Sin(Time.time - entryElapsedTime) * speed + defaultPosition.y, defaultPosition.z);

        CheckAlive();

        Shot();
    }

    /// <summary> �G���g���[ </summary>
    private void Entry()
    {
        entryElapsedTime--;

        //������E�ɏo�Ă���
        this.transform.position -= new Vector3(0.1f, 0.0f, 0.0f);

        //�o�ߎ��Ԃ� 0 �ɂȂ�����
        if (entryElapsedTime <= 0.0f)
        {
            defaultPosition = this.transform.position;
            entryElapsedTime = entryMaxTime;
            isEntry = false;
        }
    }

    /// <summary> �U�� </summary>
    private void Shot()
    {
        if(isShot)
        {
            for (int i = 0; i < enemyBosses.Length; i++)
            {
                //���ʂ������Ă��Ȃ��Ƃ���
                if (enemyBosses[i].GetComponent<EnemyScript>().GetHPScript().IsDead())
                {
                    //�������Ȃ�
                    continue;
                }
                //�e�𐶐�
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
                state = StateChange.TargetShot;
                break;
        }

        //�{�̂̃��C�t��0�ɂȂ�ƃN���A
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
