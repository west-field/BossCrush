using UnityEngine;

/// <summary> ゲームシーンマネージャー </summary>
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

        //変更先のシーン名を設定
        mainManager.ChangeSceneName("GameOverScene");
    }

    private void Update()
    {
        //シーンを変更しているときは判定を行わないように
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
