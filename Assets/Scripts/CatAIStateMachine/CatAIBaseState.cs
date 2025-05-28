public abstract class CatAIBaseState
{
    protected CatAIStateMachine _ctx;
    protected CatAIStateFactory _factory;
    public CatAIBaseState(CatAIStateMachine currentContext, CatAIStateFactory catAIStateFactory)
    {
        _ctx = currentContext;
        _factory = catAIStateFactory;
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchStates();

    public abstract void InitializeSubState();

    void UpdateStates(){}

    protected void SwitchState(CatAIBaseState newState)
    {
        ExitState(); // Exit current state
        newState.EnterState(); // Enter new state

        _ctx.CurrentState = newState;
    }

    protected void SetSuperState(){}

    protected void SetSubState(){}
}
