using System.Diagnostics;

public abstract class PlayerControllerBaseState
{
    protected PlayerControllerStateMachine _ctx;
    protected PlayerControllerStateFactory _factory;
    public PlayerControllerBaseState(PlayerControllerStateMachine currentContext, PlayerControllerStateFactory playerControllerStateFactory)
    {
        _ctx = currentContext;
        _factory = playerControllerStateFactory;
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void FixedUpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchStates();

    protected void SwitchState(PlayerControllerBaseState newState)
    {
        ExitState(); // Exit current state
        newState.EnterState(); // Enter new state

        _ctx.CurrentState = newState;
    }
}
