using UnityEngine;

public class PlayerControllerIdleState : PlayerControllerBaseState
{
    public PlayerControllerIdleState(PlayerControllerStateMachine currentContext, PlayerControllerStateFactory playerControllerStateFactory)
    : base(currentContext, playerControllerStateFactory) { }

    public override void EnterState() 
    { 
        Debug.Log("Idle"); 
        _ctx.AppliedMovementX = 0;
        _ctx.AppliedMovementZ = 0;
    }

    public override void UpdateState() { CheckSwitchStates(); }

    public override void ExitState() { }

    public override void CheckSwitchStates() 
    {
        if (_ctx.IsWalkPressed && _ctx.isRunPressed)
        {
            SwitchState(_factory.Run());
        }
        else if (_ctx.IsWalkPressed)
        {
            SwitchState(_factory.Walk());
        }
    }

    public override void InitializeSubState() { }
}
