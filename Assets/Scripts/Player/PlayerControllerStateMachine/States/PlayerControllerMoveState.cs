using UnityEngine;

public class PlayerControllerMoveState : PlayerControllerBaseState
{
    public PlayerControllerMoveState(PlayerControllerStateMachine currentContext, PlayerControllerStateFactory playerControllerStateFactory)
    : base(currentContext, playerControllerStateFactory) { }

    public override void EnterState() { }

    public override void UpdateState() 
    {
        CheckSwitchStates();
        if (_ctx.isRunPressed) _ctx.CurrentSpeed = _ctx.SprintSpeed;
        else _ctx.CurrentSpeed = _ctx.MoveSpeed;
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
    }
}
