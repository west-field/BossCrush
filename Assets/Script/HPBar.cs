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
        //ç≈ëÂÇ…Ç∑ÇÈ
        slider.value = 1;
    }

    /// <summary>
    /// HPÇsliderÇ…îΩâf
    /// </summary>
    /// <param name="nowHP">ç°ÇÃHP</param>
    /// <param name="maxHP">ç≈ëÂHP</param>
    public void HPToSlider(float nowHP,float maxHP)
    {
        slider.value = nowHP / maxHP;
    }
}
