using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool JumpInput { get; private set; }
    public InputSystem_Actions inputActions;
    public Vector2 lookInput { get; private set; }
    public bool PauseInput {get; private set; }
    public bool InteractInput;

    //Attack Inputs

    public bool Attack1 {get; private set;}
    public bool Attack2 {get; private set;}
    public bool Attack3 {get; private set;}
    public bool Attack4 {get; private set;}

    [SerializeField]public Vector2 SetTarget;
    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    void Update()
    {
        inputActions.Player.CursourSwitch.performed += ctx => PauseInput = !PauseInput;

        inputActions.Player.Interact.performed += ctx => InteractInput = true;
    }

    public void onMoveInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MoveInput = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            MoveInput = Vector2.zero;
        }
    }

    public void onJumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpInput = true;
        }
        else if (context.canceled)
        {
            JumpInput = false;
        }
    }
    public void onlookInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            lookInput = context.ReadValue<Vector2>();
        }
    }
    public void OnAttack1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Attack1 = true;
        }
        else if (context.canceled)
        {
            Attack1 = false;
        }
    }
    public void OnAttack2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Attack2 = true;
        }
        else if (context.canceled)
        {
            Attack2 = false;
        }
    }
    public void OnAttack3(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Attack3 = true;
        }
        else if (context.canceled)
        {
            Attack3 = false;
        }
    }
    public void OnAttack4(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Attack4 = true;
        }
        else if (context.canceled)
        {
            Attack4 = false;
        }
    }
    public void OnTargetSelect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SetTarget = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            SetTarget = Vector2.zero;
        }
    }
}

public struct MouseSensitivity
{
    public float Horizontal;
    public float Vertical;
}