using UnityEngine;
using TMPro;

public class GameClearManager : MonoBehaviour
{
    private MainManager mainManager;
    private UpdateExample updateExample;
    private GameFlagCheck gameFlagCheck;

    /*�X�R�A*/
    private WriteReadToCSV scoreData;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [SerializeField] private AudioSource audioSource;//���艹���Đ�����

    private void Start()
    {
        mainManager = GetComponent<MainManager>();
        updateExample = GetComponent<UpdateExample>();
        gameFlagCheck = GetComponent<GameFlagCheck>();

        //�X�R�A
        scoreData = GetComponent<WriteReadToCSV>();
        scoreData.ReadDataToCSV();
        var data = scoreData.Data();
        //����̃X�R�A
        var score = data[0];
        scoreText.text = score[0];
        //�n�C�X�R�A
        score = data[1];
        highScoreText.text = score[0];

        //�ύX��̃V�[������ݒ�
        mainManager.ChangeSceneName("TitleScene");
    }

    private void Update()
    {
        //�V�[����ύX���Ă���Ƃ��͔�����s��Ȃ��悤��
        if (mainManager.IsChangeScene()) return;

        if (gameFlagCheck.IsPause()) return;

        //����{�^�����������Ƃ�
        if (updateExample.OnTrigger(UpdateExample.ActionType.Submit))
        {
            audioSource.Play();
            //�V�[����ύX����
            mainManager.StartChangeScene();
        }
        else if (updateExample.OnTrigger(UpdateExample.ActionType.Pause))
        {
            gameFlagCheck.Pause(true);
            return;
        }
    }
}
