using UnityEngine;

public class PlayerControllerIdleState : PlayerControllerBaseState
{
    public PlayerControllerIdleState(PlayerControllerStateMachine currentContext, PlayerControllerStateFactory playerControllerStateFactory)
    : base(currentContext, playerControllerStateFactory) { }
    
    private float _timer = 6.0f;
    
    public override void EnterState() 
    { 
    }

    public override void UpdateState() 
    { 
        CheckSwitchStates();
        _timer -=  Time.deltaTime;
        
        if (_timer <= 0.0f) _ctx.PlayerAnimator.SetBool("Sit_b", true);
    }

    public override void FixedUpdateState()
    {
    }

    public override void ExitState()
    {
        _ctx.PlayerAnimator.SetBool("Sit_b", false);
        _timer = 6.0f;
    }

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
