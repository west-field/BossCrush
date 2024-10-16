using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    private MainManager mainManager;
    private GameOverAndClearCheck gameOverAndClearCheck;

    private void Start()
    {
        mainManager = GetComponent<MainManager>();

        gameOverAndClearCheck = GetComponent<GameOverAndClearCheck>();

        //変更先のシーン名を設定
        mainManager.ChangeSceneName("GameOverScene");
    }

    private void FixedUpdate()
    {
        //シーンを変更しているときは判定を行わないように
        if (mainManager.IsChangeScene()) return;

        if(gameOverAndClearCheck.IsClear())
        {
            mainManager.ChangeSceneName("GameClearScene");
            mainManager.StartChangeScene();
        }
        else if(gameOverAndClearCheck.IsGameOver())
        {
            mainManager.ChangeSceneName("GameOverScene");
            mainManager.StartChangeScene();
        }
    }
}
