using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlagCheck : MonoBehaviour
{
    private bool isClear;//クリア判定
    private bool isGameOver;//ゲームオーバー判定

    private bool isPause;//ポーズしているかどうか

    private bool isFullScreen;//フルスクリーンモードに変更するか

    private void Start()
    {
        isClear = false;
        isGameOver = false;
        isPause = false;
        isFullScreen = Screen.fullScreen;
    }

    /// <summary> クリア判定 </summary>
    public void Clear()
    {
        isClear = true;
    }
    /// <summary> クリア判定かどうか </summary>
    /// <returns>true:クリア false:まだ</returns>
    public bool IsClear()
    {
        return isClear;
    }

    /// <summary> ゲームオーバー判定 </summary>
    public void GameOver()
    {
        isGameOver = true;
    }
    /// <summary> ゲームオーバー判定かどうか </summary>
    /// <returns>true:ゲームオーバー false:まだ</returns>
    public bool IsGameOver()
    {
        return isGameOver;
    }

    /// <summary> フルスクリーンモードを切り替える </summary>
    public void ChangeFullScreen()
    {
        isFullScreen = !isFullScreen;

        Screen.fullScreen = this.isFullScreen;
    }

    public bool IsFullScreen()
    {
        return isFullScreen;
    }

    public void Pause(bool pause)
    {
        isPause = pause;
        if(isPause)
        {
            Time.timeScale = 0;
            Instantiate(Resources.Load("PauseCanvas"));
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public bool IsPause()
    {
        return isPause;
    }
}
