using UnityEngine;

/// <summary> �Q�[���V�[���}�l�[�W���[ </summary>
public class GameSceneManager : MonoBehaviour
{
    private MainManager mainManager;
    private UpdateExample updateExample;
    private GameFlagCheck gameFlagCheck;

    private void Start()
    {
        mainManager = GetComponent<MainManager>();
        updateExample = GetComponent<UpdateExample>();
        gameFlagCheck = GetComponent<GameFlagCheck>();

        //�ύX��̃V�[������ݒ�
        mainManager.ChangeSceneName("GameOverScene");
    }

    private void Update()
    {
        //�V�[����ύX���Ă���Ƃ��͔�����s��Ȃ��悤��
        if (mainManager.IsChangeScene()) return;

        if (gameFlagCheck.IsPause()) return;

        if (updateExample.OnTrigger(UpdateExample.ActionType.Pause))
        {
            gameFlagCheck.Pause(true);
            return;
        }

        if (gameFlagCheck.IsClear())
        {
            mainManager.ChangeSceneName("GameClearScene");
            mainManager.StartChangeScene();
        }
        else if(gameFlagCheck.IsGameOver())
        {
            mainManager.ChangeSceneName("GameOverScene");
            mainManager.StartChangeScene();
        }
    }
}
