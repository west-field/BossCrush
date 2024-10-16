using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private GameOverAndClearCheck gameOverCheck;
    private CSharpEventExample example;//�{�^���̔���
    [SerializeField] private GameObject bulletPrefab;//�^�����������e�̃v���n�u
    private float speed;//�ړ��X�s�[�h

    private bool isShot;//�����ł��邩�ǂ���
    private float shotElapsedTime;//�U��������̌o�ߎ���
    private const float shotMaxTime = 5.0f;//���U�����ł���܂ł̎���

    private HPScript hPScript;//HP

    private const int bombMaxNum = 3;
    private int bombNum;

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
    }

    private void Update()
    {
        if (example.IsBomb())
        {
            //�{�����g���Ɖ�ʓ��̓G�e�����ׂď������Ƃ��ł���
            Debug.Log("�{�����g���Ɖ�ʓ��̓G�e�����ׂď������Ƃ��ł���");
            Bomb();
        }
    }

    private void FixedUpdate()
    {
        if(hPScript.IsDead())
        {
            Debug.Log("����");
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
    private void Bomb()
    {
        if(bombNum > 0)
        {
            bombNum--;
            //�G�����������e�̃Q�[���I�u�W�F�N�g�����ׂĎ擾����
            var objs = GameObject.FindGameObjectsWithTag("EnemyBullet");

            foreach (var obj in objs)
            {
                Destroy(obj);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�G�l�~�[�����˂����e�ɓ���������
        if(collision.transform.tag == "EnemyBullet")
        {
            hPScript.Damage();
            Destroy(collision.gameObject);
            if (hPScript.IsDead())
            {
                gameOverCheck.GameOver();
            }
        }

        //�G�l�~�[�ɐڐG������
        if(collision.transform.tag == "Enemy")
        {
            hPScript.Damage();
            if(hPScript.IsDead())
            {
                gameOverCheck.GameOver();
            }
        }
    }
}
