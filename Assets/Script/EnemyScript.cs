using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> エネミー </summary>
public class EnemyScript : MonoBehaviour
{
    private HPScript hPScript;//HP

    private Vector3 defaultPosition;//移動量
    private float speed;//移動スピード

    private void Start()
    {
        hPScript = new HPScript();
        hPScript.Init(10);

        defaultPosition = this.transform.position;
        speed = 1.5f;
    }

    private void FixedUpdate()
    {
        //上下に移動する
        this.transform.position = new Vector3(defaultPosition.x, Mathf.Sin(Time.time) * speed + defaultPosition.y, defaultPosition.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //プレイヤーが発射した弾に当たった時
        if (collision.transform.tag == "PlayerBullet")
        {
            hPScript.Damage();
            Destroy(collision.gameObject);

            if(hPScript.IsDead())
            {
                //色を変更する
                this.GetComponent<SpriteRenderer>().color = Color.gray;
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
