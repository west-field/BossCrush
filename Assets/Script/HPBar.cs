using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = transform.GetChild(0).GetComponent<Slider>();
        //�ő�ɂ���
        slider.value = 1;
    }

    /// <summary>
    /// HP��slider�ɔ��f
    /// </summary>
    /// <param name="nowHP">����HP</param>
    /// <param name="maxHP">�ő�HP</param>
    public void HPToSlider(float nowHP,float maxHP)
    {
        slider.value = nowHP / maxHP;
    }
}