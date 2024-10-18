using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> エネミー </summary>
public class EnemyScript : MonoBehaviour
{
    /*HP*/
    private HPScript hPScript;//HP
    [SerializeField] int maxHp = 50;
    /*HPBar*/
    private HPBar hpBar;
    /*スコア*/
    [SerializeField] int score = 1000;
    private ScoreManager scoreManager;
    /*サウンド*/
    private AudioSource audioSource;
    [SerializeField] private AudioClip damage;

    private void Start()
    {
        hPScript = new HPScript();
        hPScript.Init(maxHp);

        scoreManager = GameObject.Find("Manager").GetComponent<ScoreManager>();

        audioSource = transform.parent.GetComponent<AudioSource>();

        hpBar = transform.GetComponentInChildren<HPBar>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //プレイヤーが発射した弾に当たった時
        if (collision.transform.tag == "PlayerBullet")
        {
            Destroy(collision.gameObject);

            if (hPScript.IsDead()) return;

            hPScript.Damage();
            hpBar.HPToSlider(hPScript.GetHp(), maxHp);

            if (hPScript.IsDead())
            {
                //色を変更する
                this.GetComponent<SpriteRenderer>().color = Color.gray;
                scoreManager.AddScore(score);
                audioSource.PlayOneShot(damage);
                //this.GetComponent<BoxCollider2D>().enabled = false;
                return;
            }
        }
    }

    /// <summary> HPのスクリプトを取得 </summary>
    /// <returns>HPScript</returns>
    public HPScript GetHPScript()
    {
        return hPScript;
    }
}
