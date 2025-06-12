using UnityEngine;

public class PlayerControllerJumpState : PlayerControllerBaseState
{
    public PlayerControllerJumpState(PlayerControllerStateMachine currentContext, PlayerControllerStateFactory catAIStateFactory)
    : base(currentContext, catAIStateFactory) {
        _isRootState = true;
        InitializeSubState();
    }

    public override void EnterState() {
        Debug.Log("Jump");
        _ctx.VelocityY = Mathf.Sqrt(_ctx.JumpHeight * -2f * _ctx.Gravity); }

    public override void UpdateState() 
    { 
        CheckSwitchStates(); 
        ApplyGravity();
    }

    public override void ExitState() { }

    public override void CheckSwitchStates() 
    {
        if (_ctx.CharacterController.isGrounded)
        {
            SwitchState(_factory.Grounded());
        }
    }

    public override void InitializeSubState() 
    {
        if (!_ctx.IsWalkPressed && !_ctx.isRunPressed)
        {
            SetSubState(_factory.Idle());
        }
        else if (_ctx.IsWalkPressed && !_ctx.isRunPressed)
        {
            SetSubState(_factory.Walk());
        }
        else
        {
            SetSubState(_factory.Run());
        }
    }

    private void ApplyGravity()
    {
        if (_ctx.CharacterController.isGrounded && _ctx.VelocityY < 0)
        {
            _ctx.VelocityY = -2f;
        }

        _ctx.VelocityY += _ctx.Gravity * Time.deltaTime;
        _ctx.CharacterController.Move(_ctx.Velocity * Time.deltaTime);
    }
}
