public class PlayerControllerGroundedState : PlayerControllerBaseState
{
    public PlayerControllerGroundedState(PlayerControllerStateMachine currentContext, PlayerControllerStateFactory catAIStateFactory)
    : base(currentContext, catAIStateFactory) { }

    public override void EnterState() {  }

    public override void UpdateState() { CheckSwitchStates(); }

    public override void ExitState() { }

    public override void CheckSwitchStates() 
    { 
        if (_ctx.IsJumpPressed)
        {
            SwitchState(_factory.Jump());
        }
    }

    public override void InitializeSubState() { }
}
