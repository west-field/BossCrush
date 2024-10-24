using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    private UpdateExample updateExample;
    private GameFlagCheck gameFlagCheck;
    private SelectYesOrNo selectYesOrNo;

    /// <summary> ポーズメニュー </summary>
    enum PauseItem
    {
        FullScreenChange,
        Back,
        End,

        Max
    }

    /*選択*/
    private int selectMenuNum;
    private PauseItem drawType;

    [SerializeField] private GameObject frame;//選択フレーム
    [SerializeField] private Transform[] framePosition = new Transform[(int)PauseItem.Max];//フレームの位置

    /*音*/
    private AudioSource audioSource;
    [SerializeField] private AudioClip pauseSound;
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private AudioClip submitSound;
    [SerializeField] private AudioClip canselSound;

    private void Start()
    {
        var manager = GameObject.Find("Manager");
        updateExample = manager.GetComponent<UpdateExample>();
        gameFlagCheck = manager.GetComponent<GameFlagCheck>();

        selectYesOrNo = this.transform.GetChild(1).GetComponent<SelectYesOrNo>();
        selectYesOrNo.gameObject.SetActive(false);

        selectMenuNum = 0;
        drawType = PauseItem.Max;

        var nextFramePos = frame.transform.position;
        nextFramePos.y = framePosition[selectMenuNum].position.y;
        frame.transform.position = nextFramePos;

        audioSource = this.GetComponent<AudioSource>();
        audioSource.PlayOneShot(pauseSound);
    }

    private void Update()
    {
        switch(drawType)
        {
            case PauseItem.FullScreenChange:
                FullScreenChangeUpdate();
                break;
            case PauseItem.Back:
                BackUpdate();
                break;
            case PauseItem.End:
                EndUpdate();
                break;
            default:
                NormalUpdate();
                break;
        }
    }

    private void NormalUpdate()
    {
        if (updateExample.OnTrigger(UpdateExample.ActionType.Move))
        {
            if (updateExample.GetVelocity().x > 0 || updateExample.GetVelocity().y > 0)
            {
                //上
                selectMenuNum = (selectMenuNum + ((int)PauseItem.Max - 1)) % (int)PauseItem.Max;
            }
            else if (updateExample.GetVelocity().x < 0 || updateExample.GetVelocity().y < 0)
            {
                //下
                selectMenuNum = (selectMenuNum + 1) % (int)PauseItem.Max;
            }
            //サウンドを鳴らす
            audioSource.PlayOneShot(moveSound);

            var nextFramePos = frame.transform.position;
            nextFramePos.y = framePosition[selectMenuNum].position.y;
            frame.transform.position = nextFramePos;
        }

        if (updateExample.OnTrigger(UpdateExample.ActionType.Submit))
        {
            audioSource.PlayOneShot(submitSound);
            selectYesOrNo.gameObject.SetActive(true);
            drawType = (PauseItem)selectMenuNum;
            switch (drawType)
            {
                case PauseItem.FullScreenChange:
                    selectYesOrNo.TextChange("Screen Change ?");
                    break;
                case PauseItem.End:
                    selectYesOrNo.TextChange("Game End ?");
                    break;
            }
        }
        else if(updateExample.OnTrigger(UpdateExample.ActionType.Cancel) || 
            updateExample.OnTrigger(UpdateExample.ActionType.Pause))
        {
            audioSource.PlayOneShot(canselSound);
            drawType = PauseItem.Back;
        }
    }

    private void FullScreenChangeUpdate()
    {
        if (selectYesOrNo.IsYes())
        {
            gameFlagCheck.ChangeFullScreen();
            CanselToBackPause();
        }
        else if (selectYesOrNo.IsNo() ||
             updateExample.OnTrigger(UpdateExample.ActionType.Cancel))
        {
            CanselToBackPause();
        }
    }

    private void BackUpdate()
    {
        Destroy(this.gameObject);
        gameFlagCheck.Pause(false);
    }

    private void EndUpdate()
    {
        if (selectYesOrNo.IsYes())
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }
        else if (selectYesOrNo.IsNo() ||
            updateExample.OnTrigger(UpdateExample.ActionType.Cancel))
        {
            CanselToBackPause();
        }
    }

    private void CanselToBackPause()
    {
        selectYesOrNo.gameObject.SetActive(false);
        drawType = PauseItem.Max;
        audioSource.PlayOneShot(canselSound);
    }
}
