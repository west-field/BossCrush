using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �^�C�g���}�l�[�W���[ </summary>
public class TitleManager : MonoBehaviour
{
    private MainManager mainManager;
    private UpdateExample updateExample;
    private GameFlagCheck gameFlagCheck;

    [SerializeField] private AudioSource audioSource;//���艹���Đ�����
    [SerializeField] private AudioClip submit;
    [SerializeField] private AudioClip pause;

    private void Start()
    {
        mainManager = GetComponent<MainManager>();
        updateExample = GetComponent<UpdateExample>();
        gameFlagCheck = GetComponent<GameFlagCheck>();

        //�ύX��̃V�[������ݒ�
        mainManager.ChangeSceneName("GameScene");
    }

    private void Update()
    {
        //�V�[����ύX���Ă���Ƃ��͔�����s��Ȃ��悤��
        if (mainManager.IsChangeScene()) return;

        if(gameFlagCheck.IsPause()) return;

        //����{�^�����������Ƃ�
        if(updateExample.OnTrigger(UpdateExample.ActionType.Submit))
        {
            audioSource.PlayOneShot(submit);
            Debug.Log("�V�[����ύX����");
            //�V�[����ύX����
            mainManager.StartChangeScene();
        }
        else if(updateExample.OnTrigger(UpdateExample.ActionType.Pause))
        {
            audioSource.PlayOneShot(pause);
            gameFlagCheck.Pause(true);
            return;
        }
    }
}
