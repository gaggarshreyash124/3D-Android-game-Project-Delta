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

    public bool Attack1;
    public bool Attack2;
    public bool Attack3;
    public bool Attack4;

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

        //Attacks
        inputActions.Player.Attack1.performed += ctx  => Attack1 = true;
        inputActions.Player.Attack1.canceled += ctx  => Attack1 = false;

        inputActions.Player.Attack2.performed += ctx  => Attack2 = true;
        inputActions.Player.Attack2.canceled += ctx  => Attack2 = false;

        inputActions.Player.Attack3.performed += ctx  => Attack3 = true;
        inputActions.Player.Attack3.canceled += ctx  => Attack3 = false;
        
        inputActions.Player.Attack4.performed += ctx  => Attack4 = true;
        inputActions.Player.Attack4.canceled += ctx  => Attack4 = false;
        

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
}

public struct MouseSensitivity
{
    public float Horizontal;
    public float Vertical;
}