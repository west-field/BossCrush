using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary> ポーズ画面ではいかいいえを選ぶ </summary>
public class SelectYesOrNo : MonoBehaviour
{
    private UpdateExample updateExample;

    private bool isYes;

    private bool isSubmit;

    [SerializeField] private GameObject frame;//フレーム
    [SerializeField] private TextMeshProUGUI textMeshPro;//選択する内容を表示するテキスト
    [SerializeField] private Transform[] framePosition = new Transform[2];//はいといいえの場所

    private void Start()
    {
        updateExample = GameObject.Find("Manager").GetComponent<UpdateExample>();

        isYes = false;
        isSubmit = false;
        //いいえの位置にフレームを移動
        frame.transform.position = framePosition[1].position;
    }

    private void OnEnable()
    {
        isYes = false;
        isSubmit = false;
        //いいえの位置にフレームを移動
        frame.transform.position = framePosition[1].position;
    }

    private void Update()
    {
        if (isSubmit) return;

        if(updateExample.OnTrigger(UpdateExample.ActionType.Move))
        {
            isYes = !isYes;
            if(isYes)
            {
                frame.transform.position = framePosition[0].position;
            }
            else
            {
                frame.transform.position = framePosition[1].position;
            }
        }
        if(updateExample.OnTrigger(UpdateExample.ActionType.Submit))
        {
            isSubmit = true;
        }
    }

    /// <summary> はいを選択したか </summary>
    /// <returns></returns>
    public bool IsYes()
    {
        if(isSubmit)
        {
            return isYes;
        }
        return false;
    }

    /// <summary> いいえを選択したか </summary>
    /// <returns></returns>
    public bool IsNo()
    {
        if(isSubmit)
        {
            return !isYes;
        }
        return false;
    }

    /// <summary> 表示するテキストを変更する </summary>
    /// <param name="text"></param>
    public void TextChange(string text)
    {
        textMeshPro.text = text;
    }
}
