using UnityEngine;

/// <summary> ホーミング弾 </summary>
public class BulletHoming : BulletParent
{
    private bool isHoming;//ホーミングできるか
    private float homingElapsedTime;//経過時間
    private const float kHomingMaxTime = 60.0f;//ホーミングできる時間

    private Transform targetPos;//ホーミングする対象

    /// <summary> 移動角度 </summary>
    float Direction
    {
        get { return Mathf.Atan2(GetComponent<Rigidbody2D>().velocity.y, GetComponent<Rigidbody2D>().velocity.x) * Mathf.Rad2Deg; }
    }

    protected override void Start()
    {
        base.Start();

        speed = 6.0f;

        targetPos = GameObject.Find("Player").transform;

        MoveDirection(true);

        //得点を設定
        GetComponent<Score>().SetScore(550);

        isHoming = true;
        homingElapsedTime = kHomingMaxTime;
    }

    protected override void Move()
    {
        if (isHoming)
        {
            homingElapsedTime--;
            if (homingElapsedTime <= 0)
            {
                isHoming = false;
                homingElapsedTime = kHomingMaxTime;
                return;
            }

            dir = targetPos.position - this.transform.position;
            
            var targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            // 角度差を求める
            var deltaAngle = Mathf.DeltaAngle(Direction, targetAngle);
            
            //30度以上曲がらないように
            if (Mathf.Abs(deltaAngle) - 180.0f > 30.0f ||
                Mathf.Abs(deltaAngle) - 180.0f < -30.0f)
            {
                if (deltaAngle <= 0)
                {
                    deltaAngle = -180.0f + 30.0f;
                }
                else
                {
                    deltaAngle = 180.0f - 30.0f;
                }
            }

            //移動
            var vx = Mathf.Cos(Mathf.Deg2Rad * deltaAngle);
            var vy = Mathf.Sin(Mathf.Deg2Rad * deltaAngle);
            velocity = new Vector3(vx, vy, 0.0f);

            //移動方向に回転
            dir = (this.transform.position + velocity) - this.transform.position;
        }

        base.Move();
    }
}
