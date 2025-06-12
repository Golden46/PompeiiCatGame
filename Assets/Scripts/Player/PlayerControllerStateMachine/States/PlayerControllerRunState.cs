using UnityEngine;

public class PlayerControllerRunState : PlayerControllerBaseState
{
    public PlayerControllerRunState(PlayerControllerStateMachine currentContext, PlayerControllerStateFactory playerControllerStateFactory)
    : base(currentContext, playerControllerStateFactory) { }

    public override void EnterState() 
    {
        Debug.Log("Run");
    }

    public override void UpdateState() 
    { 
        CheckSwitchStates();
        _ctx.CurrentSpeed = _ctx.SprintSpeed;
        _ctx.AppliedMovementX = _ctx.MoveInputX;
        _ctx.AppliedMovementZ = _ctx.MoveInputY;
    }

    public override void ExitState() { }

    public override void CheckSwitchStates() 
    {
        if (!_ctx.IsWalkPressed)
        {
            SwitchState(_factory.Idle());
        }
        else if (_ctx.IsWalkPressed && !_ctx.isRunPressed)
        {
            SwitchState(_factory.Walk());
        }
    }

    public override void InitializeSubState() { }
}
