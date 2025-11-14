using UnityEngine;

public class CatInputManager : MonoBehaviour
{
    public PlayerInputActions PlayerInputActions;
    private PlayerControllerStateMachine playerStateMachine;

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
        playerStateMachine = PlayerControllerStateMachine.Instance;

        // Movement
        PlayerInputActions.Player.Movement.performed += ctx => playerStateMachine.OnMove(ctx);
        PlayerInputActions.Player.Movement.canceled += ctx => playerStateMachine.OnMove(ctx);

        PlayerInputActions.Player.Sprint.performed += ctx => playerStateMachine.OnSprint(ctx);
        PlayerInputActions.Player.Sprint.canceled += ctx => playerStateMachine.OnSprint(ctx);

        PlayerInputActions.Player.Jump.performed += ctx => playerStateMachine.OnJump(ctx);
        PlayerInputActions.Player.Jump.canceled += ctx => playerStateMachine.OnJump(ctx);

        // Interaction
        PlayerInputActions.Player.Echo.performed += ctx => playerStateMachine.OnEcho(ctx);
        PlayerInputActions.Player.Meow.performed += ctx => playerStateMachine.OnMeow(ctx);
    }

    private void OnDisable()
    {
        // Movement
        PlayerInputActions.Player.Movement.performed -= playerStateMachine.OnMove;
        PlayerInputActions.Player.Movement.canceled -=playerStateMachine.OnMove;

        PlayerInputActions.Player.Sprint.performed -= playerStateMachine.OnSprint;
        PlayerInputActions.Player.Sprint.canceled -= playerStateMachine.OnSprint;

        PlayerInputActions.Player.Jump.performed -= playerStateMachine.OnJump;
        PlayerInputActions.Player.Jump.canceled -= playerStateMachine.OnJump;

        // Interaction
        PlayerInputActions.Player.Echo.performed -= playerStateMachine.OnEcho;
        PlayerInputActions.Player.Meow.performed -= playerStateMachine.OnMeow;

        PlayerInputActions.Player.Disable();
    }
}
