using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private CSharpEventExample example;//�{�^���̔���
    private GameObject bulletPrefab;//�^�����������e�̃v���n�u
    private float speed;//�ړ��X�s�[�h

    private void Start()
    {
        example = GameObject.Find("Manager").GetComponent<CSharpEventExample>();
        bulletPrefab = (GameObject)Resources.Load("BulletStraight");
        speed = 4.0f;
    }

    private void FixedUpdate()
    {
        if (example.IsMove())
        {
            this.transform.position += example.GetVelocity() * Time.deltaTime * speed;
        }

        if (example.IsSlow())
        {
            this.transform.position -= example.GetVelocity() * Time.deltaTime * speed * 0.5f;
        }

        if (example.IsShot())
        {
            Debug.Log("�e�𐶐�");
            Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
        }
    }
}
