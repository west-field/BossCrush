using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary> �|�[�Y��ʂł͂�����������I�� </summary>
public class SelectYesOrNo : MonoBehaviour
{
    private UpdateExample updateExample;

    private bool isYes;

    private bool isSubmit;

    [SerializeField] private GameObject frame;//�t���[��
    [SerializeField] private TextMeshProUGUI textMeshPro;//�I��������e��\������e�L�X�g
    [SerializeField] private Transform[] framePosition = new Transform[2];//�͂��Ƃ������̏ꏊ

    private void Start()
    {
        updateExample = GameObject.Find("Manager").GetComponent<UpdateExample>();

        isYes = false;
        isSubmit = false;
        //�������̈ʒu�Ƀt���[�����ړ�
        frame.transform.position = framePosition[1].position;
    }

    private void OnEnable()
    {
        isYes = false;
        isSubmit = false;
        //�������̈ʒu�Ƀt���[�����ړ�
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

    /// <summary> �͂���I�������� </summary>
    /// <returns></returns>
    public bool IsYes()
    {
        if(isSubmit)
        {
            return isYes;
        }
        return false;
    }

    /// <summary> ��������I�������� </summary>
    /// <returns></returns>
    public bool IsNo()
    {
        if(isSubmit)
        {
            return !isYes;
        }
        return false;
    }

    /// <summary> �\������e�L�X�g��ύX���� </summary>
    /// <param name="text"></param>
    public void TextChange(string text)
    {
        textMeshPro.text = text;
    }
}
