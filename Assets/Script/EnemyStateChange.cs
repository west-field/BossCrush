using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �G�l�~�[�̏�ԕω� </summary>
public class EnemyStateChange : MonoBehaviour
{
    /// <summary> �{�X�^�C�v </summary>
    enum BossType
    {
        Top,
        Under,
        Main,
        Max
    }

    /// <summary> 3��ނ̒e�������Ƃ��ł��� </summary>
    enum BulletType
    {
        TargetShot,
        RandomShot,
        HomingShot,
        Max
    }
    
    private GameFlagCheck gameFlagCheck;

    [SerializeField] private GameObject[] enemyBosses = new GameObject[(int)BossType.Max];
    /*�e�𔭎˂���*/
    [SerializeField] private GameObject[] bulletPrefabs = new GameObject[(int)BulletType.Max];
    [SerializeField] private Transform[] bulletStartPosition = new Transform[(int)BossType.Max];//�e�𔭎˂���ʒu

    private BulletType state;//��ԕω��ɉ����čU�����@��ύX����

    /*��Ԃɂ���ĕς��U��*/
    private bool isShot;//�����ł��邩�ǂ���
    private float shotElapsedTime;//�U��������̌o�ߎ���
    private const float shotMaxTime = 25.0f;//���U�����ł���܂ł̎���

    /*��ɔ��˂���U��*/
    private bool isAlwaysShot;//�����ł��邩�ǂ���
    private float alwaysShotElapsedTime;//�U��������̌o�ߎ���
    private const float alwaysShotMaxTime = 80.0f;//���U�����ł���܂ł̎���

    /*��*/
    private AudioSource audioSource;
    [SerializeField] private AudioClip shot;//���Ƃ��̉�

    /*�㉺�Ɉړ�*/
    private Vector3 defaultPosition;//���̈ʒu
    private float speed;//�ړ��X�s�[�h

    /*�o��*/
    private bool isEntry;//�o�ꒆ��
    private float entryElapsedTime;//�G���g���[�o�ߎ���
    private const float entryMaxTime = 80.0f;//�G���g���[����

    private void Start()
    {
        gameFlagCheck = GameObject.Find("Manager").GetComponent<GameFlagCheck>();

        state = BulletType.TargetShot;
        isShot = true;
        shotElapsedTime = shotMaxTime;

        isAlwaysShot = true;
        alwaysShotElapsedTime = alwaysShotMaxTime;

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

        if(gameFlagCheck.IsClear())
        {
            return;
        }

        //�㉺�Ɉړ�����
        this.transform.position = new Vector3(defaultPosition.x, Mathf.Sin(Time.time - entryMaxTime) * speed + defaultPosition.y, defaultPosition.z);

        CheckAlive();

        Shot();
    }

    /// <summary> �o�� </summary>
    private void Entry()
    {
        entryElapsedTime--;

        //������E�ɏo�Ă���
        this.transform.position -= new Vector3(0.1f, 0.0f, 0.0f);

        //�o�ߎ��Ԃ� 0 �ɂȂ�����
        if (entryElapsedTime <= 0.0f)
        {
            defaultPosition = this.transform.position;
            isEntry = false;
        }
    }

    /// <summary> �U�� </summary>
    private void Shot()
    {
        //��Ԃɂ���ĕς��U��
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

                //���ʂ������Ă��鎞�͒e�𐶐�
                Instantiate(bulletPrefabs[(int)state], bulletStartPosition[i].position, Quaternion.identity);
            }

            audioSource.PlayOneShot(shot);//�����Đ�
            isShot = false;//��������U�����ł��Ȃ��悤�ɂ���
        }
        else
        {
            shotElapsedTime--;
            //�o�ߎ��Ԃ�0�����������Ȃ�����
            if (shotElapsedTime <= 0)
            {
                //�U�����ł���悤��
                isShot = true;
                shotElapsedTime = shotMaxTime;
            }
        }

        //��ɔ��˂���U��
        if(isAlwaysShot)
        {
            for (int i = 0; i < enemyBosses.Length; i++)
            {
                //�e�𐶐�
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
                alwaysShotElapsedTime = alwaysShotMaxTime;
            }
        }
    }

    /// <summary> �����Ă��邩���m�F </summary>
    private void CheckAlive()
    {
        var aliveNum = 0;//�����Ă��鐔(�ő�3)

        foreach (var enemy in enemyBosses)
        {
            //����ł��Ȃ��Ƃ�
            if(!enemy.GetComponent<EnemyScript>().GetHPScript().IsDead())
            {
                aliveNum++;
            }
        }

        //�����Ă��鐔�ɂ���čU�����@��ύX����
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

        //�{�̂̃��C�t��0�ɂȂ�ƃN���A
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
