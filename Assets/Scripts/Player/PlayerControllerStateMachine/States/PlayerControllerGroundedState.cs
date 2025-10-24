using UnityEngine;

public class PlayerControllerGroundedState : PlayerControllerBaseState
{
    public PlayerControllerGroundedState(PlayerControllerStateMachine currentContext, PlayerControllerStateFactory playerControllerStateFactory)
    : base(currentContext, playerControllerStateFactory) { 
        _isRootState = true;
        InitializeSubState();
    }

    public override void EnterState() { Debug.Log("Grounded");  }

    public override void UpdateState() { CheckSwitchStates(); }

    public override void ExitState() { }

    public override void CheckSwitchStates() 
    { 
        if (_ctx.IsJumpPressed)
        {
            SwitchState(_factory.Jump());
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
