using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    protected int score;

    private void Start()
    {
        score = 100;
    }

    public void SetScore(int score)
    {
        this.score = score;
    }

    /// <summary> “¾“_Žæ“¾ </summary>
    /// <returns></returns>
    public int GetScore()
    {
        return score;
    }
}
