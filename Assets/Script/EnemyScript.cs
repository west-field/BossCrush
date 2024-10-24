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
            //攻撃力を取得する
            var damagePower = collision.gameObject.GetComponent<BulletParent>().AttackPower();
            Destroy(collision.gameObject);//当たった弾を消す

            if (hpScript.IsDead()) return;

            hpScript.Damage(damagePower);
            hpBar.HPToSlider(hpScript.GetHp(), maxHp);

            if (hpScript.IsDead())
            {
                ColorChange();//色を変更する
                deathEffect.Play();//エフェクトを再生
                scoreManager.AddScore(score);//スコアを渡す
                audioSource.PlayOneShot(damage);//ダメージ音を再生
                return;
            }
        }
    }

    /// <summary> 色を灰色に変える </summary>
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
