using System.Diagnostics;

public abstract class PlayerControllerBaseState
{
    protected bool _isRootState = false;
    protected PlayerControllerStateMachine _ctx;
    protected PlayerControllerStateFactory _factory;
    protected PlayerControllerBaseState _currentSubState;
    protected PlayerControllerBaseState _currentSuperState;
    public PlayerControllerBaseState(PlayerControllerStateMachine currentContext, PlayerControllerStateFactory playerControllerStateFactory)
    {
        _ctx = currentContext;
        _factory = playerControllerStateFactory;
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchStates();

    public abstract void InitializeSubState();

    public void UpdateStates() 
    {
        UpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }

    protected void SwitchState(PlayerControllerBaseState newState)
    {
        ExitState(); // Exit current state
        newState.EnterState(); // Enter new state

        if (_isRootState)
        {
            _ctx.CurrentState = newState;
        } else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(PlayerControllerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerControllerBaseState newSubState) 
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
