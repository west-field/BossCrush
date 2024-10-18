using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private int score;
    [SerializeField] private TextMeshProUGUI scoreText;

    private WriteReadToCSV scoreData;

    private void Start()
    {
        score = 0;
        scoreText.text = this.score.ToString();

        if(GetComponent<WriteReadToCSV>() != null)
        {
            scoreData = GetComponent<WriteReadToCSV>();
        }

        if (scoreData != null)
        {
            Debug.Log("ì«Ç›çûÇ›");
            scoreData.ReadDataToCSV();
            Debug.Log(scoreData.Data());
        }
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable");
        if(scoreData != null)
        {
            var index = 0;

            Debug.Log("ï€ë∂");
            scoreData.InitializeData(1);
            scoreData.SetDataAt(index, score);
            scoreData.AddData();

            var list = scoreData.Data();
            Debug.Log(list.Count.ToString());
            if (list.Count >= 2)
            {
                var s = list[1];
                scoreData.SetDataAt(index, Mathf.Max(score, int.Parse(s[0])));
            }
            else
            {
                scoreData.SetDataAt(index, score);
            }
            scoreData.AddData();
            scoreData.WriteDataToCSV();
        }
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable");
    }

    /// <summary> ÉXÉRÉAÇí«â¡Ç∑ÇÈ </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        this.score += score;
        scoreText.text = this.score.ToString();
    }
}
