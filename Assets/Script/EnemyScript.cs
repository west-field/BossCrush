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
            var damagePower = collision.gameObject.GetComponent<BulletParent>().AttackPower();
            Destroy(collision.gameObject);

            if (hpScript.IsDead()) return;

            hpScript.Damage(damagePower);
            hpBar.HPToSlider(hpScript.GetHp(), maxHp);

            if (hpScript.IsDead())
            {
                //�F��ύX����
                ColorChange();
                deathEffect.Play();
                scoreManager.AddScore(score);
                audioSource.PlayOneShot(damage);
                //this.GetComponent<BoxCollider2D>().enabled = false;
                return;
            }
        }
    }

    /// <summary> �F��ς��� </summary>
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
