using UnityEngine;
using TMPro;

/// <summary> �X�R�A�}�l�[�W���[ </summary>
public class ScoreManager : MonoBehaviour
{
    public static int score;//�X�R�A

    [SerializeField] private TextMeshProUGUI scoreText;//�X�R�A��\������e�L�X�g

    private void Start()
    {
        score = 0;
        scoreText.text = score.ToString();
    }

    /// <summary> �X�R�A��ǉ����� </summary>
    /// <param name="addScore"></param>
    public void AddScore(int addScore)
    {
        score += addScore;
        scoreText.text = score.ToString();
    }
}
