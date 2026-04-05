public class PlayerControllerStateFactory
{
    private PlayerControllerStateMachine _context;

    public PlayerControllerStateFactory(PlayerControllerStateMachine currentContext)
    {
        _context = currentContext;
    }

    public PlayerControllerBaseState Idle() { return new PlayerControllerIdleState(_context, this);}
    public PlayerControllerBaseState Move() { return new PlayerControllerMoveState(_context, this); }
    public PlayerControllerBaseState Jump() { return new PlayerControllerJumpState(_context, this); }
}
