using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CSharpEventExample : MonoBehaviour
{
    private PlayerInput playerInput;
    private Vector2 velocity;//�ړ����x
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
        //�ʒm���󂯎�邽�߂̃f���Q�[�g�o�^
        playerInput.onActionTriggered += OnMove;
        playerInput.onActionTriggered += OnSubmit;
        playerInput.onActionTriggered += OnCancel;
    }

    private void OnDisable()
    {
        if (playerInput == null) return;
        //�ʒm���󂯎�邽�߂̃f���Q�[�g�o�^����
        playerInput.onActionTriggered -= OnMove;
        playerInput.onActionTriggered -= OnSubmit;
        playerInput.onActionTriggered -= OnCancel;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        //Move�ȊO�͏������Ȃ�
        if (context.action.name != "Move")  return;

        isMove = context.action.IsPressed();
        //isMove = context.action.WasPerformedThisFrame();

        //MoveAction�̓��͒l���擾
        var axis = context.ReadValue<Vector2>();

        //�ړ����x��ێ�
        velocity = axis.normalized;

        Debug.Log(velocity);
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        //Submit�ȊO�͏������Ȃ�
        if (context.action.name != "Submit") return;
        isSubmit = context.action.WasPressedThisFrame();
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        //Cancel�ȊO�͏������Ȃ�
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
            Debug.Log("update:�ړ�");
        }
        if (IsSubmit())
        {
            Debug.Log("update:����");
        }
        if (IsCancel())
        {
            Debug.Log("update:�L�����Z��");
        }
    }
        private void FixedUpdate()
    {
        if (IsMove())
        {
            Debug.Log("�ړ�");
        }
        if (IsSubmit())
        {
            Debug.Log("����");
        }
        if (IsCancel())
        {
            Debug.Log("�L�����Z��");
        }

        isMove = false;
        isSubmit = false;
        isCansel = false;
    }
}


/*
 IsPressed �{�^���������Ă����
 started	WasPressedThisFrame	�{�^�����������u��
 canceled	WasReleasedThisFrame	�{�^�������ꂽ�u�� 
 */
