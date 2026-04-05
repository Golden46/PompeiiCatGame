using UnityEngine;

public class PlayerControllerMoveState : PlayerControllerBaseState
{
    public PlayerControllerMoveState(PlayerControllerStateMachine currentContext, PlayerControllerStateFactory playerControllerStateFactory)
    : base(currentContext, playerControllerStateFactory) { }

    public override void EnterState() { _ctx.PlayerAnimator.SetBool("IsWalking", true); }

    public override void UpdateState() 
    {
        CheckSwitchStates();
        _ctx.GetOrientation();
    }

    public override void FixedUpdateState()
    {
        _ctx.HandleMovement();
    }

    public override void ExitState()
    {
        _ctx.PlayerAnimator.SetBool("IsWalking", false);
    }

    public override void CheckSwitchStates() 
    {
        if (!_ctx.IsWalkPressed)
        {
            SwitchState(_factory.Idle());
        }

        if (_ctx.IsJumpPressed && _ctx.TargetLedge != null)
        {
            SwitchState(_factory.Jump());
        }
    }
}
