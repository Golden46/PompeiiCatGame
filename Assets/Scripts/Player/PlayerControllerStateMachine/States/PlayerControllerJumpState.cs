using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerJumpState : PlayerControllerBaseState
{
    public PlayerControllerJumpState(PlayerControllerStateMachine currentContext, PlayerControllerStateFactory catAIStateFactory)
    : base(currentContext, catAIStateFactory) { }

    public override void EnterState() { _ctx.VelocityY = Mathf.Sqrt(_ctx.JumpHeight * -2f * _ctx.Gravity); }

    public override void UpdateState() { CheckSwitchStates(); }

    public override void ExitState() { }

    public override void CheckSwitchStates() 
    {
        if (_ctx.CharacterController.isGrounded)
        {
            SwitchState(_factory.Grounded());
        }
    }

    public override void InitializeSubState() { }
}
