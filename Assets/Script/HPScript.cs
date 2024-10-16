using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPScript
{
    private int hp;//Œ»İ‚ÌHP
    private bool isDead;//€‚ñ‚¾‚©‚Ç‚¤‚©

    /// <summary> ‰Šú‰» </summary>
    public void Init(int maxHP)
    {
        hp = maxHP;
        isDead = false;
    }

    /// <summary> Œ»İ‚ÌHP‚ğæ“¾‚·‚é </summary>
    /// <returns>hp</returns>
    public int GetHp()
    {
        return hp;
    }

    /// <summary> ƒ_ƒ[ƒW‚ğó‚¯‚½hp‚ğŒ¸‚ç‚· </summary>
    public void Damage()
    {
        hp--;

        if(hp <= 0)
        {
            hp = 0;
            isDead = true;
        }
    }

    /// <summary> €‚ñ‚Å‚¢‚é‚©‚Ç‚¤‚© </summary>
    /// <returns>true:€‚ñ‚¾ false:¶‚«‚Ä‚é </returns>
    public bool IsDead()
    {
        return isDead;
    }
}
