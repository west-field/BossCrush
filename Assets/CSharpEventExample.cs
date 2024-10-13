using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CSharpEventExample : MonoBehaviour
{
    private PlayerInput playerInput;
    private Vector2 velocity;//移動速度
    bool isMove;
    bool isSubmit;
    bool isCansel;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        if (playerInput == null)    return;
        //通知を受け取るためのデリゲート登録
        playerInput.onActionTriggered += OnMove;
        playerInput.onActionTriggered += OnSubmit;
        playerInput.onActionTriggered += OnCancel;
    }

    private void OnDisable()
    {
        if (playerInput == null) return;
        //通知を受け取るためのデリゲート登録解除
        playerInput.onActionTriggered -= OnMove;
        playerInput.onActionTriggered -= OnSubmit;
        playerInput.onActionTriggered -= OnCancel;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        //Move以外は処理しない
        if (context.action.name != "Move")  return;

        isMove = context.action.IsPressed();
        //isMove = context.action.WasPerformedThisFrame();

        //MoveActionの入力値を取得
        var axis = context.ReadValue<Vector2>();

        //移動速度を保持
        velocity = axis.normalized;

        Debug.Log(velocity);
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        //Submit以外は処理しない
        if (context.action.name != "Submit") return;
        isSubmit = context.action.WasPressedThisFrame();
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        //Cancel以外は処理しない
        if (context.action.name != "Cancel") return;
        isCansel = context.action.WasPressedThisFrame();
    }

    public Vector2 GetVelocity()
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
    private void Update()
    {
        if (IsMove())
        {
            Debug.Log("update:移動");
        }
        if (IsSubmit())
        {
            Debug.Log("update:決定");
        }
        if (IsCancel())
        {
            Debug.Log("update:キャンセル");
        }
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

        isMove = false;
        isSubmit = false;
        isCansel = false;
    }
}


/*
 IsPressed ボタンを押している間
 started	WasPressedThisFrame	ボタンを押した瞬間
 canceled	WasReleasedThisFrame	ボタンが離れた瞬間 
 */
