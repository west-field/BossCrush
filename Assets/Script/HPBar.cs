using UnityEngine;
using UnityEngine.UI;

/// <summary> HP�o�[ </summary>
public class HPBar : MonoBehaviour
{
    private Slider slider;//�X���C�_�[

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
