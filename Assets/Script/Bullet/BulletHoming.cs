using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ホーミング弾 </summary>
public class BulletHoming : BulletParent
{
    private bool isHoming;//ホーミングできるか
    private float homingElapsedTime;//経過時間
    private const float homingMaxTime = 30.0f;//ホーミングできる時間

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
        velocity = new Vector3(-1.0f, 0.0f, 0.0f);

        GetComponent<Score>().SetScore(550);

        isHoming = true;
        homingElapsedTime = homingMaxTime;
    }

    protected override void Move()
    {
        if (isHoming)
        {
            homingElapsedTime--;
            if (homingElapsedTime <= 0)
            {
                isHoming = false;
                homingElapsedTime = homingMaxTime;
                return;
            }

            dir = targetPos.position - this.transform.position;
            
            var targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            // 角度差を求める
            var deltaAngle = Mathf.DeltaAngle(Direction, targetAngle);

            if (Mathf.Abs(deltaAngle) < 30.0f)
            {
                if(deltaAngle <= 0)
                {
                    deltaAngle = -30.0f;
                }
                else
                {
                    deltaAngle = 30.0f;
                }
            }

            var vx = Mathf.Cos(Mathf.Deg2Rad * deltaAngle);
            var vy = Mathf.Sin(Mathf.Deg2Rad * deltaAngle);
            velocity = new Vector3(vx, vy, 0.0f);

        }

        base.Move();
    }
}
