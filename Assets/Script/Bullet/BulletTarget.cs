using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �^�[�Q�b�g�Ɍ������Č��e </summary>
public class BulletTarget : BulletParent
{
    protected override void Start()
    {
        base.Start();

        speed = 6.0f;

        var player = GameObject.Find("Player");
        //�^�[�Q�b�g�̈ʒu���玩���̈ʒu������
        velocity = player.transform.position - this.transform.position;
        velocity.Normalize();

        GetComponent<Score>().SetScore(200);
    }
}
