using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpdateExample : MonoBehaviour
{
    /// <summary> �A�N�V�����̃^�C�v </summary>
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

    private InputActionMap inputActionMap;//�A�N�V�������擾���邽��

    private void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();

        if (playerInput == null) return;
        inputActionMap = playerInput.currentActionMap;
        Debug.Log(inputActionMap.ToString());

    }

    /// <summary> �����Ă���� </summary>
    /// <param name="actionType">UpdateExample.ActionType.�擾�������A�N�V�����^�C�v</param>
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

    /// <summary> �������Ƃ� </summary>
    /// <param name="actionType">UpdateExample.ActionType.�擾�������A�N�V�����^�C�v</param>
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
            Debug.Log("���{����������܂�����");
        }
        if(OnTrigger(ActionType.Pause))
        {
            Debug.Log("���|�[�Y��������܂�����");
        }
        if(OnTrigger(ActionType.Submit))
        {
            Debug.Log("�����肪������܂�����");
        }
        if (OnTrigger(ActionType.Cancel))
        {
            Debug.Log("���L�����Z����������܂�����");
        }
    }
}
