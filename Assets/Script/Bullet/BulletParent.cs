using UnityEngine;

/// <summary> 弾の親クラス </summary>
public class BulletParent : MonoBehaviour
{
    protected float speed;//移動速度
    protected Vector3 velocity;//移動
    protected SpriteRenderer spriteRenderer;//画面内にいるか判定を取る

    protected Vector3 dir;//回転方向

    protected int attackPower;//攻撃力

    protected virtual void Start()
    {
        speed = 1.0f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        dir = Vector3.zero;
        attackPower = 1;
    }

    private void FixedUpdate()
    {
        Move();

        //弾が画面外に出たら削除する
        if(!spriteRenderer.isVisible)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary> 移動処理 </summary>
    protected virtual void Move()
    {
        //進行方向へ回転
        this.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
        //移動
        this.transform.position += velocity * Time.deltaTime * speed;
    }

    /// <summary> 攻撃力 </summary>
    public int AttackPower()
    {
        return attackPower;
    }

    /// <summary> 移動方向を決める </summary>
    /// <param name="isLeft">true:左方向へ移動する false:右方向へ移動する</param>
    public void MoveDirection(bool isLeft)
    {
        if(isLeft)
        {
            velocity = new Vector3(-1.0f, 0.0f, 0.0f);

            var nextPos = velocity * speed + this.transform.position;
            dir = nextPos - this.transform.position;
        }
        else
        {
            velocity = new Vector3(1.0f, 0.0f, 0.0f);
        }
    }
}
