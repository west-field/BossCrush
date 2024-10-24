using UnityEngine;
using TMPro;

/// <summary> スコアマネージャー </summary>
public class ScoreManager : MonoBehaviour
{
    private int score;//スコア

    [SerializeField] private TextMeshProUGUI scoreText;//スコアを表示するテキスト

    private WriteReadToCSV scoreData;//スコアデータ

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
            Debug.Log("読み込み");
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

            Debug.Log("保存");
            scoreData.InitializeData(1);
            //今回のスコア
            scoreData.SetDataAt(index, score);
            scoreData.AddData();

            //ハイスコア
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

    /// <summary> スコアを追加する </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        this.score += score;
        scoreText.text = this.score.ToString();
    }
}
