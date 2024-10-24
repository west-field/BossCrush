using UnityEngine;

/// <summary> �Q�[�����Ŏg�p����t���O </summary>
public class GameFlagCheck : MonoBehaviour
{
    private bool isClear;//�N���A����
    private bool isGameOver;//�Q�[���I�[�o�[����

    private bool isPause;//�|�[�Y���Ă��邩�ǂ���
    private Object pauseObject;//�|�[�Y�I�u�W�F�N�g

    private bool isFullScreen;//�t���X�N���[�����[�h�ɕύX���邩

    private void Start()
    {
        isClear = false;
        isGameOver = false;
        isPause = false;
        pauseObject = null;
        isFullScreen = Screen.fullScreen;//���̃V�[���^�C�v���擾
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

    /// <summary> �|�[�Y�{�^�����������Ƃ� </summary>
    /// <param name="pause">true:�|�[�Y���n�߂� false:�|�[�Y���I���</param>
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

    /// <summary> �|�[�Y�����Ă��邩 </summary>
    /// <returns></returns>
    public bool IsPause()
    {
        return isPause;
    }
}
