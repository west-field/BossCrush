using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlagCheck : MonoBehaviour
{
    private bool isClear;//�N���A����
    private bool isGameOver;//�Q�[���I�[�o�[����

    private bool isPause;//�|�[�Y���Ă��邩�ǂ���

    private bool isFullScreen;//�t���X�N���[�����[�h�ɕύX���邩

    private void Start()
    {
        isClear = false;
        isGameOver = false;
        isPause = false;
        isFullScreen = Screen.fullScreen;
    }

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

    /// <summary> �t���X�N���[�����[�h��؂�ւ��� </summary>
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
