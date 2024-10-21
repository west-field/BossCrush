using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �^�C�g���}�l�[�W���[ </summary>
public class TitleManager : MonoBehaviour
{
    private MainManager mainManager;
    private UpdateExample updateExample;

    [SerializeField] private AudioSource audioSource;//���艹���Đ�����

    private void Start()
    {
        mainManager = GetComponent<MainManager>();
        updateExample = GetComponent<UpdateExample>();

        //�ύX��̃V�[������ݒ�
        mainManager.ChangeSceneName("GameScene");
    }

    private void Update()
    {
        //�V�[����ύX���Ă���Ƃ��͔�����s��Ȃ��悤��
        if (mainManager.IsChangeScene()) return;

        //����{�^�����������Ƃ�
        if(updateExample.OnTrigger(UpdateExample.ActionType.Submit))
        {
            audioSource.Play();
            Debug.Log("�V�[����ύX����");
            //�V�[����ύX����
            mainManager.StartChangeScene();
        }
    }
}
