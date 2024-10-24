using UnityEngine;
using TMPro;

/// <summary> �X�R�A�}�l�[�W���[ </summary>
public class ScoreManager : MonoBehaviour
{
    private int score;//�X�R�A

    [SerializeField] private TextMeshProUGUI scoreText;//�X�R�A��\������e�L�X�g

    private WriteReadToCSV scoreData;//�X�R�A�f�[�^

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
            Debug.Log("�ǂݍ���");
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

            Debug.Log("�ۑ�");
            scoreData.InitializeData(1);
            //����̃X�R�A
            scoreData.SetDataAt(index, score);
            scoreData.AddData();

            //�n�C�X�R�A
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

    /// <summary> �X�R�A��ǉ����� </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        this.score += score;
        scoreText.text = this.score.ToString();
    }
}
