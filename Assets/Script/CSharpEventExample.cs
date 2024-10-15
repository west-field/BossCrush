using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CSharpEventExample : MonoBehaviour
{
    private PlayerInput playerInput;
    private Vector3 velocity;//移動速度
    bool isMove;
    bool isSubmit;
    bool isCansel;
    bool isShot;
    bool isSlow;
    bool isBomb;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        isMove = false;
        isSubmit = false;
        isCansel = false;
        isShot = false;
        isSlow = false;
        isBomb = false;
    }

    private void OnEnable()
    {
        if (playerInput == null)    return;
        //通知を受け取るためのデリゲート登録
        playerInput.onActionTriggered += OnMove;
        playerInput.onActionTriggered += OnSubmit;
        playerInput.onActionTriggered += OnCancel;
        playerInput.onActionTriggered += OnShot;
        playerInput.onActionTriggered += OnSlow;
        playerInput.onActionTriggered += OnBomb;
    }

    private void OnDisable()
    {
        if (playerInput == null) return;
        //通知を受け取るためのデリゲート登録解除
        playerInput.onActionTriggered -= OnMove;
        playerInput.onActionTriggered -= OnSubmit;
        playerInput.onActionTriggered -= OnCancel;
        playerInput.onActionTriggered -= OnShot;
        playerInput.onActionTriggered -= OnSlow;
        playerInput.onActionTriggered -= OnBomb;
    }

    /// <summary> 移動 </summary>
    /// <param name="context"></param>
    private void OnMove(InputAction.CallbackContext context)
    {
        //Move以外は処理しない
        if (context.action.name != "Move")  return;

        isMove = context.action.IsPressed();
        //isMove = context.action.WasPerformedThisFrame();

        //MoveActionの入力値を取得
        var axis = context.ReadValue<Vector2>();

        axis.Normalize();
        Debug.Log(axis);

        //移動速度を保持
        velocity = new Vector3(axis.x, axis.y, 0.0f);
    }

    /// <summary> 決定 </summary>
    /// <param name="context"></param>
    private void OnSubmit(InputAction.CallbackContext context)
    {
        //Submit以外は処理しない
        if (context.action.name != "Submit") return;
        isSubmit = context.action.WasPressedThisFrame();
    }

    /// <summary> キャンセル </summary>
    /// <param name="context"></param>
    private void OnCancel(InputAction.CallbackContext context)
    {
        //Cancel以外は処理しない
        if (context.action.name != "Cancel") return;
        isCansel = context.action.WasPressedThisFrame();
    }

    /// <summary> 攻撃 </summary>
    /// <param name="context"></param>
    private void OnShot(InputAction.CallbackContext context)
    {
        //Shot以外は処理しない
        if (context.action.name != "Shot") return;
        isShot = context.action.IsPressed();
    }

    /// <summary> スロー </summary>
    /// <param name="context"></param>
    private void OnSlow(InputAction.CallbackContext context)
    {
        //Slow以外は処理しない
        if (context.action.name != "Slow") return;
        isSlow = context.action.IsPressed();
    }

    /// <summary> 爆弾 </summary>
    /// <param name="context"></param>
    private void OnBomb(InputAction.CallbackContext context)
    {
        //Bomb以外は処理しない
        if (context.action.name != "Bomb") return;
        isBomb = context.action.WasPressedThisFrame();
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public bool IsMove()
    {
        return isMove;
    }

    public bool IsSubmit()
    {
        return isSubmit; 
    }

    public bool IsCancel()
    {
        return isCansel;
    }

    public bool IsShot()
    {
        return isShot;
    }
    public bool IsSlow()
    {
        return isSlow;
    }

    public bool IsBomb()
    {
        return isBomb;
    }

    private void Update()
    {
        //if (IsMove())
        //{
        //    Debug.Log("update:移動");
        //}
        //if (IsSubmit())
        //{
        //    Debug.Log("update:決定");
        //}
        //if (IsCancel())
        //{
        //    Debug.Log("update:キャンセル");
        //}
    }
        private void FixedUpdate()
    {
        if (IsMove())
        {
            Debug.Log("移動");
        }
        if (IsSubmit())
        {
            Debug.Log("決定");
        }
        if (IsCancel())
        {
            Debug.Log("キャンセル");
        }
        if(IsShot())
        {
            Debug.Log("攻撃");
        }
        if (IsSlow())
        {
            Debug.Log("スロー");
        }
        if (IsBomb())
        {
            Debug.Log("爆弾");
        }
        isSubmit = false;
        isCansel = false;
        isBomb = false;
    }
}


/*
 IsPressed ボタンを押している間
 started	WasPressedThisFrame	ボタンを押した瞬間
 canceled	WasReleasedThisFrame	ボタンが離れた瞬間 
 */
