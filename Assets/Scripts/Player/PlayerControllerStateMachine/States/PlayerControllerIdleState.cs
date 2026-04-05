using UnityEngine;

public class PlayerControllerIdleState : PlayerControllerBaseState
{
    public PlayerControllerIdleState(PlayerControllerStateMachine currentContext, PlayerControllerStateFactory playerControllerStateFactory)
    : base(currentContext, playerControllerStateFactory) { }

    public override void EnterState() 
    { 
    }

    public override void UpdateState() 
    { 
        CheckSwitchStates();
        Debug.Log("Idle");
    }

    public override void FixedUpdateState()
    {
    }

    public override void ExitState() { }

    public override void CheckSwitchStates() 
    {
        if (_ctx.IsWalkPressed)
        {
            SwitchState(_factory.Move());
        }

        if (_ctx.IsJumpPressed && _ctx.TargetLedge != null)
        {
            SwitchState(_factory.Jump());
        }
    }
}
