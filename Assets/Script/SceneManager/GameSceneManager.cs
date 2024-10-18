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

        //�ύX��̃V�[������ݒ�
        mainManager.ChangeSceneName("GameOverScene");
    }

    private void FixedUpdate()
    {
        //�V�[����ύX���Ă���Ƃ��͔�����s��Ȃ��悤��
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
