using UnityEngine;

public class PlayerControllerWalkState : PlayerControllerBaseState
{
    public PlayerControllerWalkState(PlayerControllerStateMachine currentContext, PlayerControllerStateFactory catAIStateFactory)
    : base(currentContext, catAIStateFactory) { }

    public override void EnterState() { }

    public override void UpdateState() { }

    public override void ExitState() { }

    public override void CheckSwitchStates() { }

    public override void InitializeSubState() { }
}
