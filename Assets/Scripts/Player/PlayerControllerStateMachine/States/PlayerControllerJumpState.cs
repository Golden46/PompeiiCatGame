using System.Collections;
using UnityEngine;

internal class PlayerControllerJumpState : PlayerControllerBaseState
{
    public PlayerControllerJumpState(PlayerControllerStateMachine currentContext, PlayerControllerStateFactory playerControllerStateFactory)
    : base(currentContext, playerControllerStateFactory) {}

    private float _ledgeGrabT;
    private readonly float _arcHeight = 0.8f;
    private readonly float _grabSpeed = 1.25f;
    private Vector3 _startPosition;

    public override void EnterState()
    {
        _ctx.Rb.isKinematic = true;
        _startPosition = _ctx.transform.position;
        _ledgeGrabT = 0.0f;
        _ctx.PlayerAnimator.SetFloat("Speed_f", .8f);
        _ctx.PlayerAnimator.SetBool("RunJump_b", true);
    }

    public override void UpdateState()
    {
        if (_ledgeGrabT >= 1f)
        {
            _ctx.TargetLedge = null;
            CheckSwitchStates();
        }
    }

    public override void FixedUpdateState()
    {
        _ledgeGrabT += Time.fixedDeltaTime * _grabSpeed;
        _ledgeGrabT = Mathf.Clamp01(_ledgeGrabT);

        var flatPosition = Vector3.Lerp(_startPosition, _ctx.TargetLedge.position, _ledgeGrabT);
        flatPosition.y = Mathf.Lerp(_startPosition.y, _ctx.TargetLedge.position.y, _ledgeGrabT)
                         + _ctx.JumpArcSpeed.Evaluate(_ledgeGrabT) * _arcHeight;

        _ctx.transform.position = flatPosition;
    }

    public override void ExitState()
    {
        _ctx.PlayerAnimator.SetFloat("Speed_f", 0.0f);
        _ctx.PlayerAnimator.SetBool("RunJump_b", false);
        _ctx.TargetLedge = null;
        _ctx.Rb.isKinematic = false; 
    }

    public override void CheckSwitchStates()
    {
        SwitchState(_factory.Idle());
    }
}