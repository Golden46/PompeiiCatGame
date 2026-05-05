using UnityEngine;

public class PlayerControllerMoveState : PlayerControllerBaseState
{
    public PlayerControllerMoveState(PlayerControllerStateMachine currentContext, PlayerControllerStateFactory playerControllerStateFactory)
    : base(currentContext, playerControllerStateFactory) { }

    private float _animatorSpeed = 0.5f;
    
    public override void EnterState() {  }

    public override void UpdateState() 
    {
        CheckSwitchStates();
        _ctx.GetOrientation();

        Speed();
    }

    public override void FixedUpdateState()
    {
        _ctx.HandleMovement();
    }

    private void Speed()
    {
        _ctx.PlayerAnimator.SetFloat("Speed_f", _animatorSpeed);
        if (!_ctx.IsWalkPressed) _animatorSpeed = 0.0f;
        if (_ctx.IsSprintPressed)
        {
            _ctx.CurrentSpeed = _ctx.SprintSpeed;
            _animatorSpeed = 1.0f;
        }
        else
        {
            _ctx.CurrentSpeed = _ctx.MoveSpeed;
            _animatorSpeed = 0.5f;
        }
    }

    public override void ExitState()
    {
        _ctx.PlayerAnimator.SetFloat("Speed_f", 0.0f);
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
