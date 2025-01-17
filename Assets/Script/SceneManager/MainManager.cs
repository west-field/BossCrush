using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary> メインマネージャー </summary>
public class MainManager : MonoBehaviour
{
    /*フェードインアウト*/
    [SerializeField] private GameObject fadePanel;//フェードパネル
    private Image fadePanelImageAlpha;//フェードパネルの透明度を変える 
    private float fadePanelAlpha;//フェードパネルのα値 
    private float fadeSpeed;//フェードスピード 
    private bool isFadeIn;//フェードインフラグ 
    private bool isFadeOut;//フェードアウトフラグ 

    /*シーン変更*/
    private string nextSceneName;//次のシーン名 

    /*サウンド*/
    private AudioSource audioSource;//音をフェードさせる
    private float sourceFadeSpeed;//フェードスピード
    private const float kSourceVolume = 1;//最大音量

    private void Start()
    {
        //イメージを取得
        fadePanelImageAlpha = fadePanel.GetComponent<Image>();
        //透明度を1に
        fadePanelAlpha = 1.0f;
        var color = fadePanelImageAlpha.color;
        color.a = fadePanelAlpha;
        fadePanelImageAlpha.color = color;
        //フェードスピード
        fadeSpeed = 0.05f;
        //始めはフェードイン
        isFadeIn = true;
        isFadeOut = false;

        nextSceneName = "TitleScene";

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0;//音量を0に
        sourceFadeSpeed = fadeSpeed / kSourceVolume;
    }


    private void FixedUpdate()
    {
        if (isFadeIn)
        {
            FadeIn();
        }
        else if (isFadeOut)
        {
            FadeOut();
        }
    }

    /// <summary> フェードイン </summary>
    private void FadeIn()
    {
        //透明度を変更
        fadePanelAlpha -= fadeSpeed;

        var color = fadePanelImageAlpha.color;
        color.a = fadePanelAlpha;
        fadePanelImageAlpha.color = color;

        //音量を変更する
        if(audioSource.volume < kSourceVolume)
        {
            audioSource.volume += sourceFadeSpeed;
        }

        if(fadePanelAlpha <= 0.0f)
        {
            //フェードインを終わる
            isFadeIn = false;
            return;
        }
    }

    /// <summary> フェードアウト </summary>
    private void FadeOut()
    {
        //透明度を変更
        fadePanelAlpha += fadeSpeed;

        var color = fadePanelImageAlpha.color;
        color.a = fadePanelAlpha;
        fadePanelImageAlpha.color = color;

        //音量を変更する
        if (audioSource.volume > 0)
        {
            audioSource.volume -= sourceFadeSpeed;
        }

        if (fadePanelAlpha >= 1.0f)
        {
            //フェードインを終わる
            isFadeOut = false;
            SceneManager.LoadScene(nextSceneName);
            Debug.Log(nextSceneName + "にシーンを変更");
            return;
        }
    }

    /// <summary> シーンを変更する </summary>
    /// <param name="name">変更したいシーンの名前</param>
    public void ChangeSceneName(string name)
    {
        Debug.Log(name + "に変更");

        //シーン名を変更
        nextSceneName = name;
    }

    /// <summary> シーンを変更する </summary>
    public void StartChangeScene()
    {
        if (isFadeOut)
        {
            Debug.Log("シーンを変更している");
            return;
        }

        //フェードアウトを開始する
        isFadeOut = true;
        //透明度を0に
        fadePanelAlpha = 0.0f;
    }

    /// <summary> シーンを変更中かどうか </summary>
    /// <returns>true:変更している false:変更していない</returns>
    public bool IsChangeScene()
    {
        return isFadeOut;
    }
}
