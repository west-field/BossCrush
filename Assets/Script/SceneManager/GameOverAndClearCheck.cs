using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverAndClearCheck : MonoBehaviour
{
    private bool isClear;//�N���A����
    private bool isGameOver;//�Q�[���I�[�o�[����

    /// <summary> �N���A���� </summary>
    public void Clear()
    {
        isClear = true;
    }
    /// <summary> �N���A���肩�ǂ��� </summary>
    /// <returns>true:�N���A false:�܂�</returns>
    public bool IsClear()
    {
        return isClear;
    }

    /// <summary> �Q�[���I�[�o�[���� </summary>
    public void GameOver()
    {
        isGameOver = true;
    }
    /// <summary> �Q�[���I�[�o�[���肩�ǂ��� </summary>
    /// <returns>true:�Q�[���I�[�o�[ false:�܂�</returns>
    public bool IsGameOver()
    {
        return isGameOver;
    }
}
