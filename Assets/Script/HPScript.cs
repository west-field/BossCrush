using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPScript
{
    private int hp;//���݂�HP
    private bool isDead;//���񂾂��ǂ���

    /// <summary> ������ </summary>
    public void Init(int maxHP)
    {
        hp = maxHP;
        isDead = false;
    }

    /// <summary> ���݂�HP���擾���� </summary>
    /// <returns>hp</returns>
    public int GetHp()
    {
        return hp;
    }

    /// <summary> �_���[�W���󂯂���hp�����炷 </summary>
    public void Damage()
    {
        hp--;

        if(hp <= 0)
        {
            hp = 0;
            isDead = true;
        }
    }

    /// <summary> ����ł��邩�ǂ��� </summary>
    /// <returns>true:���� false:�����Ă� </returns>
    public bool IsDead()
    {
        return isDead;
    }
}
