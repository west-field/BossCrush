using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    private MainManager mainManager;
    private CSharpEventExample example;

    /*�X�R�A*/
    private WriteReadToCSV scoreData;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void Start()
    {
        mainManager = GetComponent<MainManager>();
        example = GetComponent<CSharpEventExample>();

        scoreData = GetComponent<WriteReadToCSV>();
        scoreData.ReadDataToCSV();
        var data = scoreData.Data();
        var score = data[0];
        scoreText.text = score[0];
        score = data[1];
        highScoreText.text = score[0];

        //�ύX��̃V�[������ݒ�
        mainManager.ChangeSceneName("TitleScene");
    }

    private void Update()
    {
        //�V�[����ύX���Ă���Ƃ��͔�����s��Ȃ��悤��
        if (mainManager.IsChangeScene()) return;

        //����{�^�����������Ƃ�
        if (example.IsSubmit())
        {
            //�V�[����ύX����
            mainManager.StartChangeScene();
        }
    }
}