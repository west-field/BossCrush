using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary> ���C���}�l�[�W���[ </summary>
public class MainManager : MonoBehaviour
{
    /// <summary> �t�F�[�h�p�l�� </summary>
    [SerializeField] private GameObject fadePanel;

    /// <summary> �t�F�[�h�p�l���̓����x��ς��� </summary>
    private Image fadePanelImageAlpha;

    /// <summary> �t�F�[�h�p�l���̃��l </summary>
    private float fadePanelAlpha;

    /// <summary> �t�F�[�h�X�s�[�h </summary>
    private float fadeSpeed;

    /// <summary> �t�F�[�h�C���t���O </summary>
    private bool isFadeIn;
    /// <summary> �t�F�[�h�A�E�g�t���O </summary>
    private bool isFadeOut;

    /// <summary> ���̃V�[���� </summary>
    private string nextSceneName;

    private void Start()
    {
        //�C���[�W���擾
        fadePanelImageAlpha = fadePanel.GetComponent<Image>();
        //�����x��1��
        fadePanelAlpha = 1.0f;
        var color = fadePanelImageAlpha.color;
        color.a = fadePanelAlpha;
        fadePanelImageAlpha.color = color;
        //�t�F�[�h�X�s�[�h
        fadeSpeed = 0.05f;
        //�n�߂̓t�F�[�h�C��
        isFadeIn = true;
        isFadeOut = false;

        nextSceneName = "TitleScene";
    }


    private void FixedUpdate()
    {
        if (isFadeIn)
        {
            FadeIn();
        }
        else if (isFadeOut)
        {
            FadeOut();
        }
    }

    private void FadeIn()
    {
        //�����x��ύX
        fadePanelAlpha -= fadeSpeed;

        var color = fadePanelImageAlpha.color;
        color.a = fadePanelAlpha;
        fadePanelImageAlpha.color = color;

        if(fadePanelAlpha <= 0.0f)
        {
            //�t�F�[�h�C�����I���
            isFadeIn = false;
            return;
        }
    }

    private void FadeOut()
    {
        //�����x��ύX
        fadePanelAlpha += fadeSpeed;

        var color = fadePanelImageAlpha.color;
        color.a = fadePanelAlpha;
        fadePanelImageAlpha.color = color;

        if (fadePanelAlpha >= 1.0f)
        {
            //�t�F�[�h�C�����I���
            isFadeOut = false;
            SceneManager.LoadScene(nextSceneName);
            Debug.Log(nextSceneName + "�ɃV�[����ύX");
            return;
        }
    }

    /// <summary> �V�[����ύX���� </summary>
    /// <param name="name">�ύX�������V�[���̖��O</param>
    public void ChangeSceneName(string name)
    {
        Debug.Log(name + "�ɕύX");

        //�V�[������ύX
        nextSceneName = name;
    }

    /// <summary> �V�[����ύX���� </summary>
    public void StartChangeScene()
    {
        if (isFadeOut)
        {
            Debug.Log("�V�[����ύX���Ă���");
            return;
        }

        //�t�F�[�h�A�E�g���J�n����
        isFadeOut = true;
        //�����x��0��
        fadePanelAlpha = 0.0f;
    }

    /// <summary> �V�[����ύX�����ǂ��� </summary>
    /// <returns>true:�ύX���Ă��� false:�ύX���Ă��Ȃ�</returns>
    public bool IsChangeScene()
    {
        return isFadeOut;
    }
}
