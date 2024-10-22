using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpdateExample : MonoBehaviour
{
    /// <summary> アクションのタイプ </summary>
    public enum ActionType
    {
        Move,
        Pause,
        Submit,
        Cancel,

        Shot,
        Slow,
        Bomb,

        Max
    }

    private InputActionMap inputActionMap;//アクションを取得するため

    private void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();

        if (playerInput == null) return;
        inputActionMap = playerInput.currentActionMap;
        Debug.Log(inputActionMap.ToString());

    }

    /// <summary> 押している間 </summary>
    /// <param name="actionType">UpdateExample.ActionType.取得したいアクションタイプ</param>
    /// <returns></returns>
    public bool OnPressed(ActionType actionType)
    {
        if(inputActionMap == null) return false;
        if (inputActionMap[actionType.ToString()] != null)
        {
            return inputActionMap[actionType.ToString()].IsPressed();
        }

        return false;
    }

    /// <summary> 押したとき </summary>
    /// <param name="actionType">UpdateExample.ActionType.取得したいアクションタイプ</param>
    /// <returns></returns>
    public bool OnTrigger(ActionType actionType)
    {
        if (inputActionMap == null) return false;
        if (inputActionMap[actionType.ToString()] != null)
        {
            return inputActionMap[actionType.ToString()].triggered;
        }

        return false;
    }

    public Vector3 GetVelocity()
    {
        var vec2 = inputActionMap[ActionType.Move.ToString()].ReadValue<Vector2>();
        vec2.Normalize();

        return new Vector3(vec2.x, vec2.y, 0.0f);
    }

    private void Update()
    {
        if (OnTrigger(ActionType.Bomb))
        {
            Debug.Log("★ボムが押されました★");
        }
        if(OnTrigger(ActionType.Pause))
        {
            Debug.Log("★ポーズが押されました★");
        }
        if(OnTrigger(ActionType.Submit))
        {
            Debug.Log("★決定が押されました★");
        }
        if (OnTrigger(ActionType.Cancel))
        {
            Debug.Log("★キャンセルが押されました★");
        }
    }
}
