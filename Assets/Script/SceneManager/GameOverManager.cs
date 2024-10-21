using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    private MainManager mainManager;
    private CSharpEventExample example;

    /*スコア*/
    private WriteReadToCSV scoreData;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void Start()
    {
        mainManager = GetComponent<MainManager>();
        example = GetComponent<CSharpEventExample>();

        scoreData = GetComponent<WriteReadToCSV>();
        scoreData.ReadDataToCSV();
        var data = scoreData.Data();
        var score = data[0];
        scoreText.text = score[0];
        score = data[1];
        highScoreText.text = score[0];

        //変更先のシーン名を設定
        mainManager.ChangeSceneName("TitleScene");
    }

    private void Update()
    {
        //シーンを変更しているときは判定を行わないように
        if (mainManager.IsChangeScene()) return;

        //決定ボタンを押したとき
        if (example.IsSubmit())
        {
            //シーンを変更する
            mainManager.StartChangeScene();
        }
    }
}
