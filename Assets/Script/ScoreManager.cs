using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private int score;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        score = 0;
        scoreText.text = this.score.ToString();
    }

    public void AddScore(int score)
    {
        this.score += score;
        scoreText.text = this.score.ToString();
    }
}
