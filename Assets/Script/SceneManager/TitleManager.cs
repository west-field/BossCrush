using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> タイトルマネージャー </summary>
public class TitleManager : MonoBehaviour
{
    private MainManager mainManager;
    private CSharpEventExample example;

    private void Start()
    {
        mainManager = GetComponent<MainManager>();
        example = GetComponent<CSharpEventExample>();

        //変更先のシーン名を設定
        mainManager.ChangeSceneName("GameScene");
    }

    private void Update()
    {
        //シーンを変更しているときは判定を行わないように
        if (mainManager.IsChangeScene()) return;

        //決定ボタンを押したとき
        if(example.IsSubmit())
        {
            Debug.Log("シーンを変更する");
            //シーンを変更する
            mainManager.StartChangeScene();
        }

        //何かのボタンを押したとき
        if(Input.anyKey)
        {
            Debug.Log("シーンを変更する");
            //シーンを変更する
            mainManager.StartChangeScene();
        }
    }
}
