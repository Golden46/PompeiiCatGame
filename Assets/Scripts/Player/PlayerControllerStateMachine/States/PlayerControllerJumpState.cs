using System.Collections;
using UnityEngine;

internal class PlayerControllerJumpState : PlayerControllerBaseState
{
    public PlayerControllerJumpState(PlayerControllerStateMachine currentContext, PlayerControllerStateFactory playerControllerStateFactory)
    : base(currentContext, playerControllerStateFactory) {}

    private float _ledgeGrabT;
    private float _arcHeight = 0.4f;
    private float _grabSpeed = 1.25f;
    private Vector3 _startPosition;

    public override void EnterState()
    {
        _ctx.RB.isKinematic = true;
        _startPosition = _ctx.transform.position;
        _ledgeGrabT = 0.0f;
    }

    public override void UpdateState()
    {
        Debug.Log("Jump");

        if (_ledgeGrabT >= 1f)
        {
            CheckSwitchStates();
        }
    }

    public override void FixedUpdateState()
    {
        _ledgeGrabT += Time.fixedDeltaTime * _grabSpeed;
        _ledgeGrabT = Mathf.Clamp01(_ledgeGrabT);

        Vector3 flatPosition = Vector3.Lerp(_startPosition, _ctx.TargetLedge.position, _ledgeGrabT);
        flatPosition.y = Mathf.Lerp(_startPosition.y, _ctx.TargetLedge.position.y, _ledgeGrabT)
                         + _ctx.JumpArcSpeed.Evaluate(_ledgeGrabT) * _arcHeight;

        _ctx.transform.position = flatPosition;
    }

    public override void ExitState()
    {
        _ctx.TargetLedge = null;
        _ctx.RB.isKinematic = false; 
    }

    public override void CheckSwitchStates()
    {
        SwitchState(_factory.Idle());
    }
}