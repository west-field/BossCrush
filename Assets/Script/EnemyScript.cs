using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �G�l�~�[ </summary>
public class EnemyScript : MonoBehaviour
{
    private HPScript hPScript;//HP

    [SerializeField] int maxHp = 50;
    [SerializeField] int score = 1000;
    private ScoreManager scoreManager;

    private AudioSource audioSource;
    [SerializeField] private AudioClip damage;

    private void Start()
    {
        hPScript = new HPScript();
        hPScript.Init(maxHp);

        scoreManager = GameObject.Find("Manager").GetComponent<ScoreManager>();

        audioSource = transform.parent.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�v���C���[�����˂����e�ɓ���������
        if (collision.transform.tag == "PlayerBullet")
        {
            Destroy(collision.gameObject);

            if (hPScript.IsDead()) return;

            hPScript.Damage();
            if(hPScript.IsDead())
            {
                //�F��ύX����
                this.GetComponent<SpriteRenderer>().color = Color.gray;
                scoreManager.AddScore(score);
                audioSource.PlayOneShot(damage);
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
