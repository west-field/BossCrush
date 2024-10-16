using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private CSharpEventExample example;//�{�^���̔���
    private GameObject bulletPrefab;//�^�����������e�̃v���n�u
    private float speed;//�ړ��X�s�[�h

    private bool isShot;//�����ł��邩�ǂ���
    private float shotElapsedTime;//�U��������̌o�ߎ���
    private const float shotMaxTime = 10.0f;//���U�����ł���܂ł̎���

    private HPScript hPScript;//HP

    private void Start()
    {
        example = GameObject.Find("Manager").GetComponent<CSharpEventExample>();
        bulletPrefab = (GameObject)Resources.Load("BulletStraight");
        speed = 4.0f;
        isShot = true;
        shotElapsedTime = 0.0f;

        hPScript = new HPScript();
        hPScript.Init(3);
    }

    private void Update()
    {
        if (example.IsBomb())
        {
            //�{�����g���Ɖ�ʓ��̓G�e�����ׂď������Ƃ��ł���
            Debug.Log("�{�����g���Ɖ�ʓ��̓G�e�����ׂď������Ƃ��ł���");
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�G�l�~�[�����˂����e�ɓ���������
        if(collision.transform.tag == "EnemyBullet")
        {
            hPScript.Damage();
            Destroy(collision.gameObject);
        }

        //�G�l�~�[�ɐڐG������
        if(collision.transform.tag == "Enemy")
        {
            hPScript.Damage();
        }
    }
}
