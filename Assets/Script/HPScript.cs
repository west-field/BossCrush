
/// <summary> HP基本情報 </summary>
public class HPScript
{
    private int hp;//現在のHP
    private bool isDead;//死んだかどうか

    /// <summary> 初期化 </summary>
    public void Init(int maxHP)
    {
        hp = maxHP;
        isDead = false;
    }

    /// <summary> 現在のHPを取得する </summary>
    /// <returns>hp</returns>
    public int GetHp()
    {
        return hp;
    }

    /// <summary> ダメージを受けた時hpを減らす </summary>
    public void Damage(int damage)
    {
        hp -= damage;

        if(hp <= 0)
        {
            hp = 0;
            isDead = true;
        }
    }

    /// <summary> 死んでいるかどうか </summary>
    /// <returns>true:死んだ false:生きてる </returns>
    public bool IsDead()
    {
        return isDead;
    }
}
