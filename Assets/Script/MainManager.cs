using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary> ���C���}�l�[�W���[ </summary>
public class MainManager : MonoBehaviour
{
    /*�t�F�[�h�C���A�E�g*/
    [SerializeField] private GameObject fadePanel;//�t�F�[�h�p�l��
    private Image fadePanelImageAlpha;//�t�F�[�h�p�l���̓����x��ς��� 
    private float fadePanelAlpha;//�t�F�[�h�p�l���̃��l 
    private float fadeSpeed;//�t�F�[�h�X�s�[�h 
    private bool isFadeIn;//�t�F�[�h�C���t���O 
    private bool isFadeOut;//�t�F�[�h�A�E�g�t���O 

    /*�V�[���ύX*/
    private string nextSceneName;//���̃V�[���� 

    /*�T�E���h*/
    private AudioSource audioSource;//�����t�F�[�h������
    private float sourceFadeSpeed;
    private const float sourceVolume = 1;

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

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0;//���ʂ�0��
        sourceFadeSpeed = fadeSpeed / sourceVolume;
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

        //���ʂ�ύX����
        if(audioSource.volume < sourceVolume)
        {
            audioSource.volume += sourceFadeSpeed;
        }

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

        //���ʂ�ύX����
        if (audioSource.volume > 0)
        {
            audioSource.volume -= sourceFadeSpeed;
        }

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
