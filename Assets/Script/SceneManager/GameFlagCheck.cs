using UnityEngine;

/// <summary> ゲーム内で使用するフラグ </summary>
public class GameFlagCheck : MonoBehaviour
{
    private bool isClear;//クリア判定
    private bool isGameOver;//ゲームオーバー判定

    private bool isPause;//ポーズしているかどうか
    private Object pauseObject;//ポーズオブジェクト

    private bool isFullScreen;//フルスクリーンモードに変更するか

    private void Start()
    {
        isClear = false;
        isGameOver = false;
        isPause = false;
        pauseObject = null;
        isFullScreen = Screen.fullScreen;//今のシーンタイプを取得
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

    /// <summary> ポーズボタンを押したとき </summary>
    /// <param name="pause">true:ポーズを始める false:ポーズを終わる</param>
    public void Pause(bool pause)
    {
        isPause = pause;
        if(isPause)
        {
            Time.timeScale = 0;
            pauseObject = Instantiate(Resources.Load("PauseCanvas"));
        }
        else
        {
            Destroy(pauseObject);
            Time.timeScale = 1.0f;
        }
    }

    /// <summary> ポーズをしているか </summary>
    /// <returns></returns>
    public bool IsPause()
    {
        return isPause;
    }
}
