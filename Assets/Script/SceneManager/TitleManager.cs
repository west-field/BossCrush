using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �^�C�g���}�l�[�W���[ </summary>
public class TitleManager : MonoBehaviour
{
    private MainManager mainManager;
    private CSharpEventExample example;

    private void Start()
    {
        mainManager = GetComponent<MainManager>();
        example = GetComponent<CSharpEventExample>();

        //�ύX��̃V�[������ݒ�
        mainManager.ChangeSceneName("GameScene");
    }

    private void Update()
    {
        //�V�[����ύX���Ă���Ƃ��͔�����s��Ȃ��悤��
        if (mainManager.IsChangeScene()) return;

        //����{�^�����������Ƃ�
        if(example.IsSubmit())
        {
            Debug.Log("�V�[����ύX����");
            //�V�[����ύX����
            mainManager.StartChangeScene();
        }
    }
}
