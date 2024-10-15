using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CSharpEventExample : MonoBehaviour
{
    private PlayerInput playerInput;
    private Vector3 velocity;//�ړ����x
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
        //�ʒm���󂯎�邽�߂̃f���Q�[�g�o�^
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
        //�ʒm���󂯎�邽�߂̃f���Q�[�g�o�^����
        playerInput.onActionTriggered -= OnMove;
        playerInput.onActionTriggered -= OnSubmit;
        playerInput.onActionTriggered -= OnCancel;
        playerInput.onActionTriggered -= OnShot;
        playerInput.onActionTriggered -= OnSlow;
        playerInput.onActionTriggered -= OnBomb;
    }

    /// <summary> �ړ� </summary>
    /// <param name="context"></param>
    private void OnMove(InputAction.CallbackContext context)
    {
        //Move�ȊO�͏������Ȃ�
        if (context.action.name != "Move")  return;

        isMove = context.action.IsPressed();
        //isMove = context.action.WasPerformedThisFrame();

        //MoveAction�̓��͒l���擾
        var axis = context.ReadValue<Vector2>();

        axis.Normalize();
        Debug.Log(axis);

        //�ړ����x��ێ�
        velocity = new Vector3(axis.x, axis.y, 0.0f);
    }

    /// <summary> ���� </summary>
    /// <param name="context"></param>
    private void OnSubmit(InputAction.CallbackContext context)
    {
        //Submit�ȊO�͏������Ȃ�
        if (context.action.name != "Submit") return;
        isSubmit = context.action.WasPressedThisFrame();
    }

    /// <summary> �L�����Z�� </summary>
    /// <param name="context"></param>
    private void OnCancel(InputAction.CallbackContext context)
    {
        //Cancel�ȊO�͏������Ȃ�
        if (context.action.name != "Cancel") return;
        isCansel = context.action.WasPressedThisFrame();
    }

    /// <summary> �U�� </summary>
    /// <param name="context"></param>
    private void OnShot(InputAction.CallbackContext context)
    {
        //Shot�ȊO�͏������Ȃ�
        if (context.action.name != "Shot") return;
        isShot = context.action.IsPressed();
    }

    /// <summary> �X���[ </summary>
    /// <param name="context"></param>
    private void OnSlow(InputAction.CallbackContext context)
    {
        //Slow�ȊO�͏������Ȃ�
        if (context.action.name != "Slow") return;
        isSlow = context.action.IsPressed();
    }

    /// <summary> ���e </summary>
    /// <param name="context"></param>
    private void OnBomb(InputAction.CallbackContext context)
    {
        //Bomb�ȊO�͏������Ȃ�
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
        //    Debug.Log("update:�ړ�");
        //}
        //if (IsSubmit())
        //{
        //    Debug.Log("update:����");
        //}
        //if (IsCancel())
        //{
        //    Debug.Log("update:�L�����Z��");
        //}
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
        if(IsShot())
        {
            Debug.Log("�U��");
        }
        if (IsSlow())
        {
            Debug.Log("�X���[");
        }
        if (IsBomb())
        {
            Debug.Log("���e");
        }
        isSubmit = false;
        isCansel = false;
        isBomb = false;
    }
}


/*
 IsPressed �{�^���������Ă����
 started	WasPressedThisFrame	�{�^�����������u��
 canceled	WasReleasedThisFrame	�{�^�������ꂽ�u�� 
 */
