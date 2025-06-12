public class PlayerControllerStateFactory
{
    private PlayerControllerStateMachine _context;

    public PlayerControllerStateFactory(PlayerControllerStateMachine currentContext)
    {
        _context = currentContext;
    }

    // Main States
    public PlayerControllerBaseState Grounded() { return new PlayerControllerGroundedState(_context, this); }
    public PlayerControllerBaseState Jump() { return new PlayerControllerJumpState(_context, this); }
    
    // Sub States
    public PlayerControllerBaseState Idle() { return new PlayerControllerIdleState(_context, this);}
    public PlayerControllerBaseState Walk() { return new PlayerControllerWalkState(_context, this); }
    public PlayerControllerBaseState Run() {  return new PlayerControllerRunState(_context, this);}
}
