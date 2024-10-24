using UnityEngine;
using TMPro;

/// <summary> �Q�[���I�[�o�[�}�l�[�W���[ </summary>
public class GameOverManager : MonoBehaviour
{
    private MainManager mainManager;
    private UpdateExample updateExample;
    private GameFlagCheck gameFlagCheck;

    /*�X�R�A*/
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private AudioSource audioSource;//���艹���Đ�����

    private void Start()
    {
        mainManager = GetComponent<MainManager>();
        updateExample = GetComponent<UpdateExample>();
        gameFlagCheck = GetComponent<GameFlagCheck>();

        //�X�R�A���擾����
        scoreText.text = ScoreManager.score.ToString();

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
