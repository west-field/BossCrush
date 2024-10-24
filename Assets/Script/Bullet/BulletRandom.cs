using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �����_���ȕ����ɂ΂�܂��e </summary>
public class BulletRandom : BulletParent
{
    protected override void Start()
    {
        base.Start();

        speed = 6.0f;

        //�����_���ȕ��������߂�
        var random = Random.Range(-30.0f, 40.0f);

        //�ړ�
        MoveDirection(true);
        velocity = Quaternion.AngleAxis(random, new Vector3(0, 0, 1)) * velocity;

        //�ړ������ɉ�]
        var nextPos = velocity * speed + this.transform.position;
        dir = nextPos - this.transform.position;

        //���_��ݒ�
        GetComponent<Score>().SetScore(350);
    }
}
