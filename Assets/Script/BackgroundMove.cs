using UnityEngine;
using UnityEngine.UI;

/// <summary> 背景を動かす </summary>
public class BackgroundMove : MonoBehaviour
{
    private const float kMaxLength = 1.0f;
    private const string kPropName = "_MainTex";

    private float offsetSpeed;//移動スピード
    private Material material;//画像を移動させるためのマテリアル

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
            //xとyの値が0～1でリピートするようにする
            var x = Mathf.Repeat(Time.time * offsetSpeed, kMaxLength);
            var offset = new Vector2(x, 0.0f);
            material.SetTextureOffset(kPropName, offset);
        }
    }

    private void OnDestroy()
    {
        //マテリアルのOffsetを戻しておく
        if(material)
        {
            material.SetTextureOffset(kPropName, Vector2.zero);
        }
    }
}
