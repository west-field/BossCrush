using UnityEngine;
using UnityEngine.UI;

/// <summary> HPバー </summary>
public class HPBar : MonoBehaviour
{
    private Slider slider;//スライダー

    private void Start()
    {
        slider = transform.GetChild(0).GetComponent<Slider>();
        //最大にする
        slider.value = 1;
    }

    /// <summary>
    /// HPをsliderに反映
    /// </summary>
    /// <param name="nowHP">今のHP</param>
    /// <param name="maxHP">最大HP</param>
    public void HPToSlider(float nowHP,float maxHP)
    {
        slider.value = nowHP / maxHP;
    }
}
