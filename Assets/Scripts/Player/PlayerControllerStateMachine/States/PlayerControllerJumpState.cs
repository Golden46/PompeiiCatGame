using UnityEngine;

public class PlayerControllerJumpState : PlayerControllerBaseState
{
    public PlayerControllerJumpState(PlayerControllerStateMachine currentContext, PlayerControllerStateFactory playerControllerStateFactory)
    : base(currentContext, playerControllerStateFactory) {
        _isRootState = true;
        InitializeSubState();
    }

    public override void EnterState() {
        Debug.Log("Jump");
        _ctx.CharacterController.center = new Vector3(_ctx.CharacterController.center.x, _ctx.CharacterController.center.y * -1 * 2, _ctx.CharacterController.center.z);
        _ctx.VelocityY = Mathf.Sqrt(_ctx.JumpHeight * -2f * _ctx.Gravity); }

    public override void UpdateState() 
    {
        CheckSwitchStates(); 
    }

    public override void ExitState() { }

    public override void CheckSwitchStates() 
    {
        if (_ctx.CharacterController.isGrounded)
        {
            _ctx.CharacterController.center = new Vector3(_ctx.CharacterController.center.x, _ctx.CharacterController.center.y * -1 / 2, _ctx.CharacterController.center.z);
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
}
