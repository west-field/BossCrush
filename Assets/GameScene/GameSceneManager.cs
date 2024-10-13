using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    private MainManager mainManager;
    private CSharpEventExample example;

    private void Start()
    {
        mainManager = GetComponent<MainManager>();
        example = GetComponent<CSharpEventExample>();

        //変更先のシーン名を設定
        mainManager.ChangeSceneName("GameOverScene");
    }

    private void Update()
    {
        //シーンを変更しているときは判定を行わないように
        if (mainManager.IsChangeScene()) return;


#if UNITY_EDITOR
        //決定ボタンを押したとき
        if (example.IsSubmit())
        {
            //シーンを変更する
            mainManager.StartChangeScene();
        }
#endif
    }
}
