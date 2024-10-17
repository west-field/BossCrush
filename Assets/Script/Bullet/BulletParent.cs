using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �e�̐e�N���X </summary>
public class BulletParent : MonoBehaviour
{
    protected float speed;//�ړ����x
    protected Vector3 velocity;//�ړ�
    protected SpriteRenderer spriteRenderer;//��ʓ��ɂ��邩��������

    protected Vector3 dir;//������������

    protected virtual void Start()
    {
        speed = 1.0f;
        velocity = Vector3.zero;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Move();

        //�e����ʊO�ɏo����폜����
        if(!spriteRenderer.isVisible)
        {
            //Debug.Log("��ʊO");
            Destroy(this.gameObject);
        }
    }

    /// <summary> �ړ����� </summary>
    protected virtual void Move()
    {
        //�������������ɉ�]
        this.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
        //�ړ�
        this.transform.position += velocity * Time.deltaTime * speed;
    }
}
