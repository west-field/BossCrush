using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �G�l�~�[ </summary>
public class EnemyScript : MonoBehaviour
{
    private HPScript hPScript;//HP

    private Vector3 defaultPosition;//���̈ʒu
    private float speed;//�ړ��X�s�[�h

    [SerializeField] int maxHp = 50;

    private void Start()
    {
        hPScript = new HPScript();
        hPScript.Init(maxHp);

        defaultPosition = this.transform.position;
        speed = 1.5f;
    }

    private void FixedUpdate()
    {
        //�㉺�Ɉړ�����
        this.transform.position = new Vector3(defaultPosition.x, Mathf.Sin(Time.time) * speed + defaultPosition.y, defaultPosition.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�v���C���[�����˂����e�ɓ���������
        if (collision.transform.tag == "PlayerBullet")
        {
            hPScript.Damage();
            Destroy(collision.gameObject);

            if(hPScript.IsDead())
            {
                //�F��ύX����
                this.GetComponent<SpriteRenderer>().color = Color.gray;
                //this.GetComponent<BoxCollider2D>().enabled = false;
                return;
            }
        }
    }

    /// <summary> HP�̃X�N���v�g���擾 </summary>
    /// <returns>HPScript</returns>
    public HPScript GetHPScript()
    {
        return hPScript;
    }
}
