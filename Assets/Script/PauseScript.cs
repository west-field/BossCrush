using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ポーズ </summary>
public class PauseScript : MonoBehaviour
{
    /// <summary> ポーズメニュー </summary>
    enum PauseMenu
    {
        WindowModeChange,
        Back,
        End,

        Max
    }

    private UpdateExample updateExample;
    private GameFlagCheck gameFlagCheck;


    /*選択*/
    private SelectYesOrNo selectYesOrNo;//はいかいいえを選ぶ
    private int selectMenuNum;//今選択しているポーズメニュー
    private PauseMenu pauseMenuSelect;//今選んでいるメニューの種類

    [SerializeField] private GameObject frame;//選択フレーム
    [SerializeField] private Transform[] framePosition = new Transform[(int)PauseMenu.Max];//フレームの位置

    /*音*/
    private AudioSource audioSource;
    [SerializeField] private AudioClip pauseSound;//ポーズが開かれたときの音
    [SerializeField] private AudioClip moveSound;//カーソルを移動させるときの音
    [SerializeField] private AudioClip submitSound;//決定ボタンを押したときの音
    [SerializeField] private AudioClip canselSound;//キャンセルボタンを押したときの音

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

    /// <summary> 通常(ポーズメニューを選んでいないときの処理) </summary>
    private void NormalUpdate()
    {
        if (updateExample.OnTrigger(UpdateExample.ActionType.Move))
        {
            if (updateExample.GetVelocity().x < 0 || updateExample.GetVelocity().y > 0)
            {
                //上
                selectMenuNum = (selectMenuNum + ((int)PauseMenu.Max - 1)) % (int)PauseMenu.Max;
            }
            else if (updateExample.GetVelocity().x > 0 || updateExample.GetVelocity().y < 0)
            {
                //下
                selectMenuNum = (selectMenuNum + 1) % (int)PauseMenu.Max;
            }
            //サウンドを鳴らす
            audioSource.PlayOneShot(moveSound);

            //フレームの位置を変更
            var nextFramePos = frame.transform.position;
            nextFramePos.y = framePosition[selectMenuNum].position.y;
            frame.transform.position = nextFramePos;
        }

        if (updateExample.OnTrigger(UpdateExample.ActionType.Submit))
        {
            audioSource.PlayOneShot(submitSound);//サウンドを鳴らす
            selectYesOrNo.gameObject.SetActive(true);//はいいいえを選択するキャンバスを表示する
            pauseMenuSelect = (PauseMenu)selectMenuNum;//今選んだメニューの種類を取得

            //文字変更
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

    /// <summary> ウィンドウモードを変更する </summary>
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

    /// <summary> 戻る　ポーズを終了する </summary>
    private void BackUpdate()
    {
        Destroy(this.gameObject);
        gameFlagCheck.Pause(false);
    }

    /// <summary> ゲームを終わる </summary>
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
            BackPause();
        }
    }

    /// <summary> はいといいえのキャンバスを表示している時にポーズ画面に戻る処理 </summary>
    private void BackPause()
    {
        selectYesOrNo.gameObject.SetActive(false);
        pauseMenuSelect = PauseMenu.Max;
        audioSource.PlayOneShot(canselSound);
    }
}
