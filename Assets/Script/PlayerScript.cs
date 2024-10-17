using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private GameOverAndClearCheck gameOverCheck;
    private CSharpEventExample example;//�{�^���̔���

    /*�e*/
    [SerializeField] private GameObject bulletPrefab;//�^�����������e�̃v���n�u
    private float speed;//�ړ��X�s�[�h

    private bool isShot;//�����ł��邩�ǂ���
    private float shotElapsedTime;//�U��������̌o�ߎ���
    private const float shotMaxTime = 5.0f;//���U�����ł���܂ł̎���

    /*HP*/
    private HPScript hPScript;//HP

    /*�{��*/
    private const int bombMaxNum = 3;
    private int bombNum;
    private ScoreManager score;

    /*�T�E���h*/
    private AudioSource audioSource;
    [SerializeField] private AudioClip shot;
    [SerializeField] private AudioClip damage;
    [SerializeField] private AudioClip bomb;

    /*������x������o�Ă���*/
    private bool isInvincible;//���G���Ԓ����ǂ���
    private float invincibleElapsedTime;//���G�o�ߎ���
    private const float invincibleMaxTime = 30.0f;//���G����
    private Renderer myRenderer;//���g�̉摜�������Ȃ�����
    private Renderer shield;//�V�[���h�̉摜�������Ȃ�����
    [SerializeField] private ParticleSystem effector;

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
        score = GameObject.Find("Manager").GetComponent<ScoreManager>();

        audioSource = GetComponent<AudioSource>();

        isInvincible = true;
        invincibleElapsedTime = invincibleMaxTime;

        myRenderer = GetComponent<Renderer>();
        myRenderer.enabled = false;
        shield = transform.GetChild(0).gameObject.GetComponent<Renderer>();
    }

    private void Update()
    {
        //�{�����g���Ɖ�ʓ��̓G�e�����ׂď������Ƃ��ł���
        if (example.IsBomb())
        {
            Bomb(true);
        }
    }

    private void FixedUpdate()
    {
        if(hPScript.IsDead())
        {
            Debug.Log("����");
            return;
        }

        if(isInvincible)
        {
            StartAgain();
            return;
        }

        if (example.IsMove())
        {
            this.transform.position += example.GetVelocity() * Time.deltaTime * speed;
        }

        //�ᑬ�ɂȂ�
        if (example.IsSlow())
        {
            this.transform.position -= example.GetVelocity() * Time.deltaTime * speed * 0.5f;
        }

        Shot();
    }

    /// <summary> �U������ </summary>
    private void Shot()
    {
        if (example.IsShot())
        {
            if (isShot)
            {
                Debug.Log("�e�𐶐�");
                Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
                isShot = false;
                audioSource.PlayOneShot(shot);
            }
        }

        //�܂��U���ł��Ȃ��Ƃ�
        if (!isShot)
        {
            shotElapsedTime--;

            //�o�ߎ��Ԃ� 0 �ɂȂ�����
            if (shotElapsedTime <= 0.0f)
            {
                shotElapsedTime = shotMaxTime;
                isShot = true;//�U���ł���悤��
            }
        }
    }

    /// <summary> �{��  �G�����������e�����ׂč폜���� </summary>
    private void Bomb(bool isButton)
    {
        if(isButton)
        {
            if (bombNum > 0)
            {
                bombNum--;
                audioSource.PlayOneShot(bomb);

                //�G�����������e�̃Q�[���I�u�W�F�N�g�����ׂĎ擾����
                var objs = GameObject.FindGameObjectsWithTag("EnemyBullet");

                foreach (var obj in objs)
                {
                    //���_���擾����
                    score.AddScore(obj.GetComponent<Score>().GetScore());

                    Destroy(obj);
                }
            }
        }
        else
        {
            audioSource.PlayOneShot(bomb);
            //�G�����������e�̃Q�[���I�u�W�F�N�g�����ׂĎ擾����
            var objs = GameObject.FindGameObjectsWithTag("EnemyBullet");

            foreach (var obj in objs)
            {
                Destroy(obj);
            }
        }
    }

    /// <summary> ���� </summary>
    private void StartAgain()
    {
        if(!myRenderer.enabled)
        {
            myRenderer.enabled = true;
            shield.enabled = true;
            this.transform.position = new Vector3(-10.0f, 0.0f, 0.0f);
        }

        invincibleElapsedTime--;

        //��ʍ�����E�ֈړ�����
        this.transform.position += new Vector3(0.1f, 0.0f, 0.0f);

        //�o�ߎ��Ԃ� 0 �ɂȂ�����
        if (invincibleElapsedTime <= 0.0f)
        {
            invincibleElapsedTime = invincibleMaxTime;
            isInvincible = false;
            shield.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�G�l�~�[�����˂����e�ɓ���������
        if(collision.transform.tag == "EnemyBullet")
        {
            Destroy(collision.gameObject);

            //���G���Ԃ̎��͍U�����󂯂Ȃ��悤��
            if (isInvincible) return;

            Damage();
        }

        //���G���Ԃ̎��͍U�����󂯂Ȃ��悤��
        if (isInvincible) return;

        //�G�l�~�[�ɐڐG������
        if (collision.transform.tag == "Enemy")
        {
            Damage();
        }
    }

    /// <summary> �_���[�W���󂯂��Ƃ��̏��� </summary>
    private void Damage()
    {
        //�G�t�F�N�g���쐬
        Instantiate(effector, this.transform.position, Quaternion.identity);
        audioSource.PlayOneShot(damage);

        hPScript.Damage();

        Bomb(false);//�G�̒e���폜����
        isInvincible = true;
        myRenderer.enabled = false;

        if (hPScript.IsDead())
        {
            gameOverCheck.GameOver();
        }
    }
}
