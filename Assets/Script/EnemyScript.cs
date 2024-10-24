using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �G�l�~�[ </summary>
public class EnemyScript : MonoBehaviour
{
    /*HP*/
    private HPScript hpScript;//HP
    [SerializeField] int maxHp = 50;
    /*HPBar*/
    private HPBar hpBar;
    /*�X�R�A*/
    [SerializeField] int score = 1000;
    private ScoreManager scoreManager;
    /*�T�E���h*/
    private AudioSource audioSource;
    [SerializeField] private AudioClip damage;
    /*�G�t�F�N�g*/
    [SerializeField] private ParticleSystem deathEffect;

    private void Start()
    {
        hpScript = new HPScript();
        hpScript.Init(maxHp);

        scoreManager = GameObject.Find("Manager").GetComponent<ScoreManager>();

        audioSource = transform.parent.GetComponent<AudioSource>();

        hpBar = transform.GetComponentInChildren<HPBar>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�v���C���[�����˂����e�ɓ���������
        if (collision.transform.tag == "PlayerBullet")
        {
            //�U���͂��擾����
            var damagePower = collision.gameObject.GetComponent<BulletParent>().AttackPower();
            Destroy(collision.gameObject);//���������e������

            if (hpScript.IsDead()) return;

            hpScript.Damage(damagePower);
            hpBar.HPToSlider(hpScript.GetHp(), maxHp);

            if (hpScript.IsDead())
            {
                ColorChange();//�F��ύX����
                deathEffect.Play();//�G�t�F�N�g���Đ�
                scoreManager.AddScore(score);//�X�R�A��n��
                audioSource.PlayOneShot(damage);//�_���[�W�����Đ�
                return;
            }
        }
    }

    /// <summary> �F���D�F�ɕς��� </summary>
    public void ColorChange()
    {
        this.GetComponent<SpriteRenderer>().color = Color.gray;
    }

    /// <summary> HP�̃X�N���v�g���擾 </summary>
    /// <returns>HPScript</returns>
    public HPScript GetHPScript()
    {
        return hpScript;
    }
}
