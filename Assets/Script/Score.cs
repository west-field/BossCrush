using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> スコアを設定する </summary>
public class Score : MonoBehaviour
{
    protected int score;//スコア

    private void Start()
    {
        score = 100;
    }

    public void SetScore(int score)
    {
        this.score = score;
    }

    /// <summary> 得点取得 </summary>
    /// <returns></returns>
    public int GetScore()
    {
        return score;
    }
}
