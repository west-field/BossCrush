using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpdateExample : MonoBehaviour
{
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

    private InputActionMap inputActionMap;

    private void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();

        if (playerInput == null) return;
        inputActionMap = playerInput.currentActionMap;
        Debug.Log(inputActionMap.ToString());

    }

    public bool OnPressed(ActionType actionType)
    {
        if(inputActionMap == null) return false;
        if (inputActionMap[actionType.ToString()] != null)
        {
            return inputActionMap[actionType.ToString()].IsPressed();
        }

        return false;
    }

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

    private void FixedUpdate()
    {
        if(OnPressed(ActionType.Move))
        {
            Debug.Log("★★移動が押されています★★");
        }
        if (OnPressed(ActionType.Shot))
        {
            Debug.Log("★★ショットが押されています★★");
        }
        if (OnPressed(ActionType.Slow))
        {
            Debug.Log("★★スローが押されています★★");
        }
    }
}
