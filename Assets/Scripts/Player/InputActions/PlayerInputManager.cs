using UnityEngine;

public class CatInputManager : MonoBehaviour
{
    public PlayerInputActions PlayerInputActions;

    private void Awake()
    {
        PlayerInputActions ??= new PlayerInputActions();
    }

    private void OnEnable()
    {
        PlayerInputActions.Player.Enable();
    }

    // This is in Start not OnEnable because of the order in which the other scripts are enabled as instances.
    private void Start()
    {
        // Movement
        PlayerInputActions.Player.Movement.performed += ctx => PlayerControllerStateMachine.Instance.OnMove(ctx);
        PlayerInputActions.Player.Movement.canceled += ctx => PlayerControllerStateMachine.Instance.OnMove(ctx);
        PlayerInputActions.Player.Sprint.performed += ctx => PlayerControllerStateMachine.Instance.OnSprint(ctx);
        PlayerInputActions.Player.Sprint.canceled += ctx => PlayerControllerStateMachine.Instance.OnSprint(ctx);
        PlayerInputActions.Player.Jump.performed += ctx => PlayerControllerStateMachine.Instance.OnJump(ctx);
        PlayerInputActions.Player.Jump.canceled += ctx => PlayerControllerStateMachine.Instance.OnJump(ctx);
    }

    private void OnDisable()
    {
        // Movement
        PlayerInputActions.Player.Movement.performed -= PlayerControllerStateMachine.Instance.OnMove;
        PlayerInputActions.Player.Movement.canceled -= PlayerControllerStateMachine.Instance.OnMove;
        PlayerInputActions.Player.Sprint.performed -= PlayerControllerStateMachine.Instance.OnSprint;
        PlayerInputActions.Player.Sprint.canceled -= PlayerControllerStateMachine.Instance.OnSprint;
        PlayerInputActions.Player.Jump.performed -= PlayerControllerStateMachine.Instance.OnJump;
        PlayerInputActions.Player.Jump.canceled -= PlayerControllerStateMachine.Instance.OnJump;

        PlayerInputActions.Player.Disable();
    }
}
