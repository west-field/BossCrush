using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �|�[�Y </summary>
public class PauseScript : MonoBehaviour
{
    /// <summary> �|�[�Y���j���[ </summary>
    enum PauseMenu
    {
        WindowModeChange,
        Back,
        End,

        Max
    }

    private UpdateExample updateExample;
    private GameFlagCheck gameFlagCheck;


    /*�I��*/
    private SelectYesOrNo selectYesOrNo;//�͂�����������I��
    private int selectMenuNum;//���I�����Ă���|�[�Y���j���[
    private PauseMenu pauseMenuSelect;//���I��ł��郁�j���[�̎��

    [SerializeField] private GameObject frame;//�I���t���[��
    [SerializeField] private Transform[] framePosition = new Transform[(int)PauseMenu.Max];//�t���[���̈ʒu

    /*��*/
    private AudioSource audioSource;
    [SerializeField] private AudioClip pauseSound;//�|�[�Y���J���ꂽ�Ƃ��̉�
    [SerializeField] private AudioClip moveSound;//�J�[�\�����ړ�������Ƃ��̉�
    [SerializeField] private AudioClip submitSound;//����{�^�����������Ƃ��̉�
    [SerializeField] private AudioClip canselSound;//�L�����Z���{�^�����������Ƃ��̉�

    private void Start()
    {
        var manager = GameObject.Find("Manager");
        updateExample = manager.GetComponent<UpdateExample>();
        gameFlagCheck = manager.GetComponent<GameFlagCheck>();

        selectYesOrNo = this.transform.GetChild(1).GetComponent<SelectYesOrNo>();
        selectYesOrNo.gameObject.SetActive(false);

        selectMenuNum = 0;
        pauseMenuSelect = PauseMenu.Max;

        var nextFramePos = frame.transform.position;
        nextFramePos.y = framePosition[selectMenuNum].position.y;
        frame.transform.position = nextFramePos;

        audioSource = this.GetComponent<AudioSource>();
        audioSource.PlayOneShot(pauseSound);
    }

    private void Update()
    {
        switch(pauseMenuSelect)
        {
            case PauseMenu.WindowModeChange:
                WindowModeChangeUpdate();
                break;
            case PauseMenu.Back:
                BackUpdate();
                break;
            case PauseMenu.End:
                EndUpdate();
                break;
            default:
                NormalUpdate();
                break;
        }
    }

    /// <summary> �ʏ�(�|�[�Y���j���[��I��ł��Ȃ��Ƃ��̏���) </summary>
    private void NormalUpdate()
    {
        if (updateExample.OnTrigger(UpdateExample.ActionType.Move))
        {
            if (updateExample.GetVelocity().x < 0 || updateExample.GetVelocity().y > 0)
            {
                //��
                selectMenuNum = (selectMenuNum + ((int)PauseMenu.Max - 1)) % (int)PauseMenu.Max;
            }
            else if (updateExample.GetVelocity().x > 0 || updateExample.GetVelocity().y < 0)
            {
                //��
                selectMenuNum = (selectMenuNum + 1) % (int)PauseMenu.Max;
            }
            //�T�E���h��炷
            audioSource.PlayOneShot(moveSound);

            //�t���[���̈ʒu��ύX
            var nextFramePos = frame.transform.position;
            nextFramePos.y = framePosition[selectMenuNum].position.y;
            frame.transform.position = nextFramePos;
        }

        if (updateExample.OnTrigger(UpdateExample.ActionType.Submit))
        {
            audioSource.PlayOneShot(submitSound);//�T�E���h��炷
            selectYesOrNo.gameObject.SetActive(true);//�͂���������I������L�����o�X��\������
            pauseMenuSelect = (PauseMenu)selectMenuNum;//���I�񂾃��j���[�̎�ނ��擾

            //�����ύX
            switch (pauseMenuSelect)
            {
                case PauseMenu.WindowModeChange:
                    selectYesOrNo.TextChange("Screen Change ?");
                    break;
                case PauseMenu.End:
                    selectYesOrNo.TextChange("Game End ?");
                    break;
            }
        }
        else if(updateExample.OnTrigger(UpdateExample.ActionType.Cancel) || 
            updateExample.OnTrigger(UpdateExample.ActionType.Pause))
        {
            audioSource.PlayOneShot(canselSound);
            pauseMenuSelect = PauseMenu.Back;
        }
    }

    /// <summary> �E�B���h�E���[�h��ύX���� </summary>
    private void WindowModeChangeUpdate()
    {
        if (selectYesOrNo.IsYes())
        {
            gameFlagCheck.ChangeFullScreen();
            BackPause();
        }
        else if (selectYesOrNo.IsNo() ||
             updateExample.OnTrigger(UpdateExample.ActionType.Cancel))
        {
            BackPause();
        }
    }

    /// <summary> �߂�@�|�[�Y���I������ </summary>
    private void BackUpdate()
    {
        Destroy(this.gameObject);
        gameFlagCheck.Pause(false);
    }

    /// <summary> �Q�[�����I��� </summary>
    private void EndUpdate()
    {
        if (selectYesOrNo.IsYes())
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
        }
        else if (selectYesOrNo.IsNo() ||
            updateExample.OnTrigger(UpdateExample.ActionType.Cancel))
        {
            BackPause();
        }
    }

    /// <summary> �͂��Ƃ������̃L�����o�X��\�����Ă��鎞�Ƀ|�[�Y��ʂɖ߂鏈�� </summary>
    private void BackPause()
    {
        selectYesOrNo.gameObject.SetActive(false);
        pauseMenuSelect = PauseMenu.Max;
        audioSource.PlayOneShot(canselSound);
    }
}
