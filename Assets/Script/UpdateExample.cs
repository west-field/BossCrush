using UnityEngine;
using UnityEngine.InputSystem;

/// <summary> ボタン </summary>
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

    /// <summary> 移動 </summary>
    /// <returns></returns>
    public Vector3 GetVelocity()
    {
        var vec2 = inputActionMap[ActionType.Move.ToString()].ReadValue<Vector2>();
        vec2.Normalize();

        return new Vector3(vec2.x, vec2.y, 0.0f);
    }
}
