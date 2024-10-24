using UnityEngine;
using UnityEngine.UI;

/// <summary> �w�i�𓮂��� </summary>
public class BackgroundMove : MonoBehaviour
{
    private const float kMaxLength = 1.0f;
    private const string kPropName = "_MainTex";

    private float offsetSpeed;//�ړ��X�s�[�h
    private Material material;//�摜���ړ������邽�߂̃}�e���A��

    private void Start()
    {
        offsetSpeed = 0.1f;

        if (GetComponent<Image>() is Image i)
        {
            material = i.material;
        }
    }

    private void FixedUpdate()
    {
        if(material)
        {
            //x��y�̒l��0�`1�Ń��s�[�g����悤�ɂ���
            var x = Mathf.Repeat(Time.time * offsetSpeed, kMaxLength);
            var offset = new Vector2(x, 0.0f);
            material.SetTextureOffset(kPropName, offset);
        }
    }

    private void OnDestroy()
    {
        //�}�e���A����Offset��߂��Ă���
        if(material)
        {
            material.SetTextureOffset(kPropName, Vector2.zero);
        }
    }
}
