using UnityEngine;
using TMPro;

/// <summary> スコアマネージャー </summary>
public class ScoreManager : MonoBehaviour
{
    public static int score;//スコア

    [SerializeField] private TextMeshProUGUI scoreText;//スコアを表示するテキスト

    private void Start()
    {
        score = 0;
        scoreText.text = score.ToString();
    }

    /// <summary> スコアを追加する </summary>
    /// <param name="addScore"></param>
    public void AddScore(int addScore)
    {
        score += addScore;
        scoreText.text = score.ToString();
    }
}
