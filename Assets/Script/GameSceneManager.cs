using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    private MainManager mainManager;
    private CSharpEventExample example;
    private EnemyStateChange enemyStateChange;

    private void Start()
    {
        mainManager = GetComponent<MainManager>();
        example = GetComponent<CSharpEventExample>();

        enemyStateChange = GameObject.Find("Enemy").GetComponent<EnemyStateChange>();

        //�ύX��̃V�[������ݒ�
        mainManager.ChangeSceneName("GameOverScene");
    }

    private void Update()
    {
        //�V�[����ύX���Ă���Ƃ��͔�����s��Ȃ��悤��
        if (mainManager.IsChangeScene()) return;


#if UNITY_EDITOR
        //����{�^�����������Ƃ�
        if (example.IsSubmit())
        {
            //�V�[����ύX����
            mainManager.StartChangeScene();
        }
#endif
    }

    private void FixedUpdate()
    {
        if(mainManager.IsChangeScene()) return;

        if(enemyStateChange.IsClear())
        {
            mainManager.StartChangeScene();
        }
    }
}
