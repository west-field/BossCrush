using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> タイトルマネージャー </summary>
public class TitleManager : MonoBehaviour
{
    private MainManager mainManager;
    private UpdateExample updateExample;
    private GameFlagCheck gameFlagCheck;

    [SerializeField] private AudioSource audioSource;//決定音を再生する
    [SerializeField] private AudioClip submit;
    [SerializeField] private AudioClip pause;

    private void Start()
    {
        mainManager = GetComponent<MainManager>();
        updateExample = GetComponent<UpdateExample>();
        gameFlagCheck = GetComponent<GameFlagCheck>();

        //変更先のシーン名を設定
        mainManager.ChangeSceneName("GameScene");
    }

    private void Update()
    {
        //シーンを変更しているときは判定を行わないように
        if (mainManager.IsChangeScene()) return;

        if(gameFlagCheck.IsPause()) return;

        //決定ボタンを押したとき
        if(updateExample.OnTrigger(UpdateExample.ActionType.Submit))
        {
            audioSource.PlayOneShot(submit);
            Debug.Log("シーンを変更する");
            //シーンを変更する
            mainManager.StartChangeScene();
        }
        else if(updateExample.OnTrigger(UpdateExample.ActionType.Pause))
        {
            audioSource.PlayOneShot(pause);
            gameFlagCheck.Pause(true);
            return;
        }
    }
}
