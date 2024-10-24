using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameClearManager : MonoBehaviour
{
    private MainManager mainManager;
    private UpdateExample updateExample;
    private GameFlagCheck gameFlagCheck;

    /*スコア*/
    private WriteReadToCSV scoreData;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [SerializeField] private AudioSource audioSource;//決定音を再生する

    private void Start()
    {
        mainManager = GetComponent<MainManager>();
        updateExample = GetComponent<UpdateExample>();
        gameFlagCheck = GetComponent<GameFlagCheck>();

        //スコア
        scoreData = GetComponent<WriteReadToCSV>();
        scoreData.ReadDataToCSV();
        var data = scoreData.Data();
        //今回のスコア
        var score = data[0];
        scoreText.text = score[0];
        //ハイスコア
        score = data[1];
        highScoreText.text = score[0];

        //変更先のシーン名を設定
        mainManager.ChangeSceneName("TitleScene");
    }

    private void Update()
    {
        //シーンを変更しているときは判定を行わないように
        if (mainManager.IsChangeScene()) return;

        if (gameFlagCheck.IsPause()) return;

        //決定ボタンを押したとき
        if (updateExample.OnTrigger(UpdateExample.ActionType.Submit))
        {
            audioSource.Play();
            //シーンを変更する
            mainManager.StartChangeScene();
        }
        else if (updateExample.OnTrigger(UpdateExample.ActionType.Pause))
        {
            gameFlagCheck.Pause(true);
            return;
        }
    }
}
