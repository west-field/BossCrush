using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverAndClearCheck : MonoBehaviour
{
    private bool isClear;//クリア判定
    private bool isGameOver;//ゲームオーバー判定

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
}
