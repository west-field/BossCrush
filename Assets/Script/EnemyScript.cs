using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> エネミー </summary>
public class EnemyScript : MonoBehaviour
{
    /*HP*/
    private HPScript hpScript;//HP
    [SerializeField] int maxHp = 50;
    /*HPBar*/
    private HPBar hpBar;
    /*スコア*/
    [SerializeField] int score = 1000;
    private ScoreManager scoreManager;
    /*サウンド*/
    private AudioSource audioSource;
    [SerializeField] private AudioClip damage;
    /*エフェクト*/
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
        //プレイヤーが発射した弾に当たった時
        if (collision.transform.tag == "PlayerBullet")
        {
            var damagePower = collision.gameObject.GetComponent<BulletParent>().AttackPower();
            Destroy(collision.gameObject);

            if (hpScript.IsDead()) return;

            hpScript.Damage(damagePower);
            hpBar.HPToSlider(hpScript.GetHp(), maxHp);

            if (hpScript.IsDead())
            {
                //色を変更する
                ColorChange();
                deathEffect.Play();
                scoreManager.AddScore(score);
                audioSource.PlayOneShot(damage);
                //this.GetComponent<BoxCollider2D>().enabled = false;
                return;
            }
        }
    }

    /// <summary> 色を変える </summary>
    public void ColorChange()
    {
        this.GetComponent<SpriteRenderer>().color = Color.gray;
    }

    /// <summary> HPのスクリプトを取得 </summary>
    /// <returns>HPScript</returns>
    public HPScript GetHPScript()
    {
        return hpScript;
    }
}
