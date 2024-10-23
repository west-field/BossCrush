using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectYesOrNo : MonoBehaviour
{
    private UpdateExample updateExample;
    private bool isYes;

    private bool isSubmit;

    [SerializeField] private GameObject frame;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Transform[] framePosition = new Transform[2];

    private void Start()
    {
        updateExample = GameObject.Find("Manager").GetComponent<UpdateExample>();

        isYes = false;
        isSubmit = false;
        frame.transform.position = framePosition[1].position;
    }

    private void OnEnable()
    {
        isYes = false;
        isSubmit = false;
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

    public bool IsYes()
    {
        if(isSubmit)
        {
            return isYes;
        }
        return false;
    }

    public bool IsNo()
    {
        if(isSubmit)
        {
            return !isYes;
        }
        return false;
    }

    public void TextChange(string text)
    {
        textMeshPro.text = text;
    }
}
